using NPRModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAS.Helpers
{
    class FeeCalculator
    {
        private Product _product;
        private decimal upchargeValue = (decimal)0.0;

        public FeeCalculator(Product product)
        {
            this._product = product;
        }

        // ComputeUpcharges calculates all of the upcharges for a product, setting the up Charge sell Prices for each product sell price
        public void ComputeUpcharges()
        {
            foreach (var upcharge in _product.ProductUpcharges.Where(u => u.UpchargeStatus != MyExtensions.GetEnumDescription(Status.Archived)))
            {
                // Compute All Product Costs with the upcharge as an addition to product Cost;
                upchargeValue = upcharge.UpchargeAmount;
                // Run Calculations
                ComputeNetCost();
                ComputeTotalCost();
                // Set the upcharge total cost to the newly calculated total cost that includes the upcharge
                upcharge.UpchargeTotalCost = _product.ProductTotalCost;
                ComputeSellPrices(false);
                // Set the upcharge Sell Prices 
                foreach (var upchargeSellPrice in upcharge.UpchargeSellPrices.Where(u => u.UpchargeSellPriceStatus != MyExtensions.GetEnumDescription(Status.Archived)))
                {
                    upchargeSellPrice.UpchargeSellPriceFinalAmount = _product.ProductSellPrices.Where(p => p.SellPriceLevel == upchargeSellPrice.UpchargeSellPriceLevel).FirstOrDefault().SellPriceFinalAmount;
                }
            }
            upchargeValue = (decimal)0.0;
        }

        // ComputeNetCost calculates all of the non-percent fees and sets the products Net Cost property
        public void ComputeNetCost()
        {
            // Set base net cost to Product Cost
            decimal? netCost = _product.ProductCost + upchargeValue;

            // Add Dollar Fees
            foreach (var fee in _product.Fees.Where(f => f.FeeStatus != MyExtensions.GetEnumDescription(Status.Archived)))
            {
                // Check for Amortized Fee
                if (fee.FeeType == MyExtensions.GetEnumDescription(FeeTypeList.Amortized))
                {
                    CalculateAmortizedFee(fee);
                }
                // If it's a percent fee 
                else if (fee.FeeType == MyExtensions.GetEnumDescription(FeeTypeList.Percent))
                {
                    // continue out of this element 
                    // [This prevent's the percent's dollar amount from being added to the net cost]
                    continue;
                }
                // increase net cost based off the Fee's Dollar amount
                netCost += fee.FeeDollarAmount;
            }

            // Set the products Net cost
            _product.ProductNetCost = netCost;
        }

        // This takes all of the percent fees of the product and calculates the Total cost [Compounded]
        public void ComputeTotalCost()
        {
            // Calculates Total Cost from Net cost [Percent Fee Calculations]
            decimal? totalCost = CalculatePercentFees(_product.Fees.Where(f => f.FeeStatus != MyExtensions.GetEnumDescription(Status.Archived)).ToList(), _product.ProductNetCost);

            // Set the product total to the 'currentTotalCost' based on the calculated cost within the loop 
            // [rounded to the 3rd decimal point]
            _product.ProductTotalCost = RoundNumberToDecimalPlace((decimal)totalCost, 3);
        }

        public void ComputeSellPrices(bool roundToNearestFiveCents)
        {
            // Pull total Cost to use in margin Calculations
            decimal productTotalCost = (decimal)_product.ProductTotalCost;

            foreach (var sellPrice in _product.ProductSellPrices.Where(p => p.SellPriceStatus != MyExtensions.GetEnumDescription(Status.Archived)))
            {
                // Calculate Margin based on the margin on the sell price
                decimal finalSellPrice = productTotalCost / (1 - (sellPrice.SellPriceMarginPercent / 100));
                sellPrice.SellPriceMarginDollarAmount = (finalSellPrice - productTotalCost);

                // Check Sell Price Percent Fee Section
                finalSellPrice = (decimal)CalculatePercentFees(sellPrice.Fees.ToList(), finalSellPrice);

                // Check Sell Price Dollar and Amortized Fees

                foreach (var fee in sellPrice.Fees.Where(f => f.FeeStatus != MyExtensions.GetEnumDescription(Status.Archived)).ToList())
                {
                    // Check for Amortized Fee
                    if (fee.FeeType == MyExtensions.GetEnumDescription(FeeTypeList.Amortized))
                    {
                        CalculateAmortizedFee(fee);
                    }
                    // If it's a percent fee 
                    else if (fee.FeeType == MyExtensions.GetEnumDescription(FeeTypeList.Percent))
                    {
                        // continue out of this element 
                        // [This prevent's the percent's dollar amount from being added to the net cost]
                        continue;
                    }
                    // increase net cost based off the Fee's Dollar amount
                    if (fee.FeeDollarAmount != null)
                    {
                        finalSellPrice += (decimal)fee.FeeDollarAmount;
                    }
                }

                // Set Final sell Price
                decimal roundedSellPrice = RoundNumberToDecimalPlace((decimal)finalSellPrice, 3);
                if (roundToNearestFiveCents)
                {
                    roundedSellPrice = RoundAmountToNearestFiveCents(roundedSellPrice);
                }

                sellPrice.SellPriceFinalAmount = roundedSellPrice;
            }
        }

        public void ComputeAllProductPrices(bool roundToNearestFiveCents)
        {
            // Runs all Functions for calculating Product data. if proudct has a cost
            if (_product.ProductCost != null)
            {
                ComputeUpcharges();
                ComputeNetCost();
                ComputeTotalCost();
                ComputeSellPrices(roundToNearestFiveCents);
            }
        }

        public void ComputeMarginBasedOnSellprice()
        {
            ComputeNetCost();
            ComputeTotalCost();
            foreach (var sellPrice in _product.ProductSellPrices.Where(p => p.SellPriceStatus != MyExtensions.GetEnumDescription(Status.Archived)))
            {
                //Calcuate the margin based on the sell price Final Amount
                decimal finalSellPrice = sellPrice.SellPriceFinalAmount;
                decimal productTotalCost = (decimal)_product.ProductTotalCost;

                decimal baseSellPrice = (decimal)0.0;
                // Calculate Dollar/Amortized fees (first b/c they are calculated last
                baseSellPrice = (decimal)CalculateBaseSellPrice(sellPrice.Fees.ToList(), finalSellPrice);

                // Calculate Percent fees
                baseSellPrice = (decimal)CalculateBaseSellPricePercentFees(sellPrice.Fees.ToList(), baseSellPrice);

                // Calculate Margin
                decimal newMarginPercent = (100 - ((productTotalCost / baseSellPrice) * 100));

                sellPrice.SellPriceMarginPercent = RoundNumberToDecimalPlace(newMarginPercent, 4);
            }
            // After setting the percent calculate product prices again for correct display of percent fees
            ComputeAllProductPrices(false);
        }

        // Area for extracted functions from within the compute functions
        #region HelperFunctions

        public decimal RoundNumberToDecimalPlace(decimal number, int places)
        {
            decimal factor = 1;
            decimal answer = 0;

            for (int index = 1; index <= places; index++)
            {
                factor = factor * (decimal)10.0;
                answer = (Math.Round((number + (decimal).05 / (factor)) * factor)) / factor;
            }

            return answer;
        }

        public decimal RoundAmountToNearestFiveCents(decimal number)
        {
            return Math.Round(number * 20) / 20;
        }

        private void CalculateAmortizedFee(Fee fee)
        {
            // Determime the type of amortized fee and use the value associated 
            if (fee.FeeAmortizedType == MyExtensions.GetEnumDescription(AmortizedTypeList.Annual_Sales_Project))
            {
                fee.FeeDollarAmount = fee.FeeAmortizedCharge / _product.ProductAnnualSalesProjection;
            }
            else if (fee.FeeAmortizedType == MyExtensions.GetEnumDescription(AmortizedTypeList.GatewayCDI_Minimum_Order))
            {
                fee.FeeDollarAmount = fee.FeeAmortizedCharge / _product.ProductGatewayCDIMinumumOrder;
            }
            else if (fee.FeeAmortizedType == MyExtensions.GetEnumDescription(AmortizedTypeList.Initial_Order_Quantity))
            {
                fee.FeeDollarAmount = fee.FeeAmortizedCharge / _product.ProductInitialOrderQuantity;
            }
            else if (fee.FeeAmortizedType == MyExtensions.GetEnumDescription(AmortizedTypeList.Vindor_Minimum))
            {
                fee.FeeDollarAmount = fee.FeeAmortizedCharge / _product.ProductVendorMinimumOrder;
            }
        }

        private decimal? CalculatePercentFees(List<Fee> feeList, decimal? startingValue)
        {
            // create 2 variables for Percent Calculations
            decimal? previousValue = startingValue;
            decimal? currentValue = previousValue;
            foreach (var fee in feeList.Where(f => f.FeeStatus != MyExtensions.GetEnumDescription(Status.Archived)).OrderBy(f => f.FeeLevel))
            {
                // If the fee is a percent calculate
                if (fee.FeeType == MyExtensions.GetEnumDescription(FeeTypeList.Percent))
                {
                    if (fee.FeePercentType == MyExtensions.GetEnumDescription(PercentTypeList.Multiplication))
                    {
                        // Calculate based on multiplication
                        currentValue = ((fee.FeePercent / 100) + 1) * previousValue;
                    }
                    else if (fee.FeePercentType == MyExtensions.GetEnumDescription(PercentTypeList.Division))
                    {
                        // Calculate based on division 
                        currentValue = previousValue / (1 - (fee.FeePercent / 100));
                    }

                    // Set Fee Dollar Amount
                    fee.FeeDollarAmount = currentValue - previousValue;

                    // Set the previous amount to the 'current' amount [for fee dollar amount calculations]
                    previousValue = currentValue;
                }
                // Else if it's any other fee, continue out of element
                else
                {
                    continue;
                }
            }
            // Return currentTotalCost as it will be fully calculated at then end of the foreach
            return currentValue;
        }

        private decimal CalculateBaseSellPricePercentFees(List<Fee> feeList, decimal finalSellPrice)
        {
            decimal? previousValue = finalSellPrice;
            decimal? currentValue = previousValue;
            foreach (var fee in feeList.Where(f => f.FeeStatus != MyExtensions.GetEnumDescription(Status.Archived)))
            {
                // If the fee is a percent calculate

                if (fee.FeeType == MyExtensions.GetEnumDescription(FeeTypeList.Percent))
                {
                    if (fee.FeePercentType == MyExtensions.GetEnumDescription(PercentTypeList.Multiplication))
                    {
                        // Calculate based on division 
                        currentValue = previousValue / ((fee.FeePercent / 100) + 1);
                    }
                    else if (fee.FeePercentType == MyExtensions.GetEnumDescription(PercentTypeList.Division))
                    {
                        // Calculate based on multiplication
                        currentValue = (1 - (fee.FeePercent / 100)) * previousValue;
                    }

                    // Set Fee Dollar Amount
                    fee.FeeDollarAmount = previousValue - currentValue;

                    // Set the previous amount to the 'current' amount [for fee dollar amount calculations]
                    previousValue = currentValue;
                }
                // Else if it's any other fee, continue out of element
                else
                {
                    continue;
                }
            }

            return (decimal)currentValue;
        }

        private decimal CalculateBaseSellPrice(List<Fee> feeList, decimal baseSellPrice)
        {
            foreach (var fee in feeList.Where(f => f.FeeStatus != MyExtensions.GetEnumDescription(Status.Archived)))
            {
                // If the fee is a percent calculate
                if (fee.FeeType != MyExtensions.GetEnumDescription(FeeTypeList.Percent))
                {
                    // increase net cost based off the Fee's Dollar amount
                    if (fee.FeeDollarAmount != null)
                    {
                        baseSellPrice -= (decimal)fee.FeeDollarAmount;
                    }
                }
                // Else if it's any other fee, continue out of element
                else
                {
                    continue;
                }
            }

            return (decimal)baseSellPrice;
        }

        #endregion
    }
}
