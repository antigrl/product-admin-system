using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPRModels;
using System.Collections.Generic;
using PAS.Helpers;
using System.Linq;

// Usually start with a comment and allows you to test and have a hard goal to achieve

// Use Visual Stuido as a whiteboard, can rename later

namespace PAS.Tests.Features
{
    [TestClass]
    public class ProductCalculationTests
    {
        [TestMethod]
        public void Calculate_Product_Net_Cost_With_Dollar_Amount_Fee()
        {
            var product = new Product();
            product.ProductCost = (decimal)1;
            product.Fees = new List<Fee>();
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = 1 });

            var feeCalculator = new FeeCalculator(product);
            feeCalculator.ComputeNetCost();

            Assert.AreEqual(1, product.ProductCost);

            Assert.AreEqual(2, product.ProductNetCost);

        }

        [TestMethod]
        public void Test_RoundNumberToDecimalPlace_Method()
        {
            var product = new Product();
            product.ProductCost = (decimal)1;
            var feeCalculator = new FeeCalculator(product);

            decimal test = (decimal).6849842136841245;
            Assert.AreEqual((decimal).68498, feeCalculator.RoundNumberToDecimalPlace(test, 5));

            Assert.AreEqual((decimal).685, feeCalculator.RoundNumberToDecimalPlace(test, 3));

        }

        [TestMethod]
        public void Calculate_Product_Net_Cost_With_Amortized_And_Dollar_Amount_Fee()
        {
            var product = new Product() { ProductCost = 1, ProductAnnualSalesProjection = 100 };
            product.Fees = new List<Fee>();
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = 1 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Amortized), FeeAmortizedCharge = 100, FeeAmortizedType = MyExtensions.GetEnumDescription(AmortizedTypeList.Annual_Sales_Project)});

            var feeCalculator = new FeeCalculator(product);
            feeCalculator.ComputeNetCost();

            Assert.AreEqual(3, product.ProductNetCost);
        }

        [TestMethod]
        public void Calculate_Product_Total_Cost_With_Multiplication_Percent_Amortized_And_Dollar_Amount_Fee()
        {
            var product = new Product() { ProductCost = 1, ProductAnnualSalesProjection = 100 };
            product.Fees = new List<Fee>();
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = 1 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Amortized), FeeAmortizedCharge = 100, FeeAmortizedType = MyExtensions.GetEnumDescription(AmortizedTypeList.Annual_Sales_Project) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication)});

            var feeCalculator = new FeeCalculator(product);
            feeCalculator.ComputeNetCost();

            Assert.AreEqual(3, product.ProductNetCost);

            feeCalculator.ComputeTotalCost();

            Assert.AreEqual((decimal?)3.3, product.ProductTotalCost);
        }

        [TestMethod]
        public void Calculate_Product_Total_Cost_With_Division_Percent_Amortized_And_Dollar_Amount_Fee()
        {
            var product = new Product() { ProductCost = 1, ProductAnnualSalesProjection = 100 };
            product.Fees = new List<Fee>();
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = 1 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Amortized), FeeAmortizedCharge = 100, FeeAmortizedType = MyExtensions.GetEnumDescription(AmortizedTypeList.Annual_Sales_Project) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Division) });

            var feeCalculator = new FeeCalculator(product);
            feeCalculator.ComputeNetCost();

            Assert.AreEqual(3, product.ProductNetCost);

            feeCalculator.ComputeTotalCost();

            // Created to get same value though division
            decimal value = ((decimal)3 / (decimal).9);
            value = feeCalculator.RoundNumberToDecimalPlace(value, 3);
            Assert.AreEqual(value, product.ProductTotalCost);
        }

        [TestMethod]
        public void Calculate_Product_Total_Cost_With_Multiplication_And_Division_Percent_Amortized_And_Dollar_Amount_Fee()
        {
            var product = new Product() { ProductCost = 1, ProductAnnualSalesProjection = 100 };
            product.Fees = new List<Fee>();
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = 1 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Amortized), FeeAmortizedCharge = 100, FeeAmortizedType = MyExtensions.GetEnumDescription(AmortizedTypeList.Annual_Sales_Project) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Division) });

            var feeCalculator = new FeeCalculator(product);
            feeCalculator.ComputeNetCost();

            Assert.AreEqual(3, product.ProductNetCost);

            feeCalculator.ComputeTotalCost();

            // Created to get same value though division
            decimal value = ((decimal)3 * (decimal)1.1) / (decimal).9;
            value = feeCalculator.RoundNumberToDecimalPlace(value, 3);
            Assert.AreEqual(value, product.ProductTotalCost);
        }

        [TestMethod]
        public void Calculate_Product_Total_Cost_With_Division_And_Multiplication_Percent_Amortized_And_Dollar_Amount_Fee()
        {
            var product = new Product() { ProductCost = 1, ProductAnnualSalesProjection = 100 };
            product.Fees = new List<Fee>();
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = 1 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Amortized), FeeAmortizedCharge = 100, FeeAmortizedType = MyExtensions.GetEnumDescription(AmortizedTypeList.Annual_Sales_Project) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Division) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });

            var feeCalculator = new FeeCalculator(product);
            feeCalculator.ComputeNetCost();

            Assert.AreEqual(3, product.ProductNetCost);

            feeCalculator.ComputeTotalCost();

            // Created to get same value though division
            decimal value = ((decimal)3 / (decimal).9) * (decimal)1.1;
            value = feeCalculator.RoundNumberToDecimalPlace(value, 3);
            Assert.AreEqual(value, product.ProductTotalCost);
        }
       
        [TestMethod]
        public void Calculate_Product_Sell_Prices_With_Division_And_Multiplication_Percent_Amortized_And_Dollar_Amount_Fee()
        {
            var product = new Product() { ProductCost = 1, ProductAnnualSalesProjection = 100 };
            product.Fees = new List<Fee>();
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = 1 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Amortized), FeeAmortizedCharge = 100, FeeAmortizedType = MyExtensions.GetEnumDescription(AmortizedTypeList.Annual_Sales_Project) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Division) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
            product.ProductSellPrices = new List<ProductSellPrice>();
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Employee", SellPriceMarginPercent = 50, SellPriceLevel = 1 });
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Retail", SellPriceMarginPercent = 50, SellPriceLevel = 2 });
            
            var feeCalculator = new FeeCalculator(product);
            feeCalculator.ComputeNetCost();

            Assert.AreEqual(3, product.ProductNetCost);

            feeCalculator.ComputeTotalCost();

            // Created to get same value though division
            decimal value = ((decimal)3 / (decimal).9) * (decimal)1.1;
            value = feeCalculator.RoundNumberToDecimalPlace(value, 3);
            Assert.AreEqual(value, product.ProductTotalCost);

            // New Part of Test
            feeCalculator.ComputeSellPrices(false);

            foreach(var sellPrice in product.ProductSellPrices)
            {
                Assert.AreEqual(value / (decimal)(.50), sellPrice.SellPriceFinalAmount);
            }
        }
        
        [TestMethod]
        public void Calculate_Product_Sell_Prices_And_Fees_With_Division_And_Multiplication_Percent_Amortized_And_Dollar_Amount_Fee()
        {
            var product = new Product() { ProductCost = 1, ProductAnnualSalesProjection = 100 };
            product.Fees = new List<Fee>();
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = 1 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Amortized), FeeAmortizedCharge = 100, FeeAmortizedType = MyExtensions.GetEnumDescription(AmortizedTypeList.Annual_Sales_Project) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Division) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
            product.ProductSellPrices = new List<ProductSellPrice>();
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Employee", SellPriceMarginPercent = 50, SellPriceLevel = 1 });
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Retail", SellPriceMarginPercent = 50, SellPriceLevel = 2 });

            foreach(var sellPrice in product.ProductSellPrices)
            {
                sellPrice.Fees = new List<Fee>();
                if(sellPrice.SellPriceName == "Retail")
                {
                    sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 20, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
                }
            }


            var feeCalculator = new FeeCalculator(product);
            feeCalculator.ComputeNetCost();

            Assert.AreEqual(3, product.ProductNetCost);            
            feeCalculator.ComputeTotalCost();

            // Created to get same value though division
            decimal value = ((decimal)3 / (decimal).9) * (decimal)1.1;
            value = feeCalculator.RoundNumberToDecimalPlace(value, 3);
            Assert.AreEqual(value, product.ProductTotalCost);

            // New Part of Test
            feeCalculator.ComputeSellPrices(false);

            foreach(var sellPrice in product.ProductSellPrices)
            {
                decimal sellPriceValue = value / (decimal)(.50);
                foreach(var fee in sellPrice.Fees)
                {
                    if(fee.FeePercentType == MyExtensions.GetEnumDescription(PercentTypeList.Multiplication))
                    {
                        sellPriceValue = sellPriceValue * (decimal)1.2;
                    }
                    else
                    {
                        sellPriceValue = sellPriceValue / (decimal).8;
                    }
                }

                Assert.AreEqual(feeCalculator.RoundNumberToDecimalPlace(sellPriceValue, 3), sellPrice.SellPriceFinalAmount);
            }
        }

        [TestMethod]
        public void Calculate_Product_Sell_Prices_With_Dollar_And_Amoritized_SellPrice_Fees()
        {
            var product = new Product() { ProductCost = 1, ProductAnnualSalesProjection = 100 };
            product.Fees = new List<Fee>();
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = 1 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Amortized), FeeAmortizedCharge = 100, FeeAmortizedType = MyExtensions.GetEnumDescription(AmortizedTypeList.Annual_Sales_Project) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Division) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
            product.ProductSellPrices = new List<ProductSellPrice>();
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Employee", SellPriceMarginPercent = 50, SellPriceLevel = 1 });
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Retail", SellPriceMarginPercent = 50, SellPriceLevel = 2 });

            foreach(var sellPrice in product.ProductSellPrices)
            {
                sellPrice.Fees = new List<Fee>();
                if(sellPrice.SellPriceName == "Retail")
                {
                    sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 20, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
                }
                sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = 1 });
                sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Amortized), FeeAmortizedCharge = 100, FeeAmortizedType = MyExtensions.GetEnumDescription(AmortizedTypeList.Annual_Sales_Project) });
            }


            var feeCalculator = new FeeCalculator(product);
            feeCalculator.ComputeNetCost();

            Assert.AreEqual(3, product.ProductNetCost);
            feeCalculator.ComputeTotalCost();

            // Created to get same value though division
            decimal value = ((decimal)3 / (decimal).9) * (decimal)1.1;
            value = feeCalculator.RoundNumberToDecimalPlace(value, 3);
            Assert.AreEqual(value, product.ProductTotalCost);

            feeCalculator.ComputeSellPrices(false);

            foreach(var sellPrice in product.ProductSellPrices)
            {
                decimal sellPriceValue = value / (decimal)(.50);
                foreach(var fee in sellPrice.Fees)
                {
                    if(fee.FeeType == MyExtensions.GetEnumDescription(FeeTypeList.Percent))
                    {
                        if(fee.FeePercentType == MyExtensions.GetEnumDescription(PercentTypeList.Multiplication))
                        {
                            sellPriceValue = sellPriceValue * (decimal)1.2;
                        }
                        else
                        {
                            sellPriceValue = sellPriceValue / (decimal).8;
                        }
                    }
                    else
                    {
                        // Each dollar fee is 1 dollar
                        sellPriceValue += 1;
                    }
                }

                Assert.AreEqual(feeCalculator.RoundNumberToDecimalPlace(sellPriceValue, 3), sellPrice.SellPriceFinalAmount);
            }
        }

        [TestMethod]
        public void Calculate_Product_Sell_Price_With_Only_Dollar_Fees()
        {
            var product = new Product() { ProductCost = 1, ProductAnnualSalesProjection = 100 };
            product.Fees = new List<Fee>();
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = 1 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Amortized), FeeAmortizedCharge = 100, FeeAmortizedType = MyExtensions.GetEnumDescription(AmortizedTypeList.Annual_Sales_Project) });

            var feeCalculator = new FeeCalculator(product);
            feeCalculator.ComputeNetCost();

            Assert.AreEqual(3, product.ProductNetCost);

            feeCalculator.ComputeTotalCost();

            Assert.AreEqual(3, product.ProductTotalCost);
        }

        [TestMethod]
        public void Compute_Product_Method()
        {
            // Data Input
            var product = new Product() { ProductCost = 1, ProductAnnualSalesProjection = 100 };
            product.Fees = new List<Fee>();
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = 1 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Amortized), FeeAmortizedCharge = 100, FeeAmortizedType = MyExtensions.GetEnumDescription(AmortizedTypeList.Annual_Sales_Project) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Division) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
            product.ProductSellPrices = new List<ProductSellPrice>();
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Employee", SellPriceMarginPercent = 50, SellPriceLevel = 1 });
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Retail", SellPriceMarginPercent = 50, SellPriceLevel = 2 });

            foreach(var sellPrice in product.ProductSellPrices)
            {
                sellPrice.Fees = new List<Fee>();
                if(sellPrice.SellPriceName == "Retail")
                {
                    sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 20, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
                }
            }

            // Run Calculator
            var feeCalculator = new FeeCalculator(product);
            feeCalculator.ComputeAllProductPrices(false);

            // Created to get same value though division
            decimal value = ((decimal)3 / (decimal).9) * (decimal)1.1;
            value = feeCalculator.RoundNumberToDecimalPlace(value, 3);
            Assert.AreEqual(value, product.ProductTotalCost);
            
            foreach(var sellPrice in product.ProductSellPrices)
            {
                decimal sellPriceValue = value / (decimal)(.50);
                foreach(var fee in sellPrice.Fees)
                {
                    if(fee.FeePercentType == MyExtensions.GetEnumDescription(PercentTypeList.Multiplication))
                    {
                        sellPriceValue = sellPriceValue * (decimal)1.2;
                    }
                    else
                    {
                        sellPriceValue = sellPriceValue / (decimal).8;
                    }
                }

                Assert.AreEqual(feeCalculator.RoundNumberToDecimalPlace(sellPriceValue, 3), sellPrice.SellPriceFinalAmount);
            }
        }

        [TestMethod]
        public void Compute_Product_And_Calculate_Margin_Based_On_SellPrice()
        {
            // Data Input
            var product = new Product() { ProductCost = 1, ProductAnnualSalesProjection = 100 };
            product.Fees = new List<Fee>();
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = 1 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Amortized), FeeAmortizedCharge = 100, FeeAmortizedType = MyExtensions.GetEnumDescription(AmortizedTypeList.Annual_Sales_Project) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Division) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
            product.ProductSellPrices = new List<ProductSellPrice>();
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Employee", SellPriceMarginPercent = 50, SellPriceLevel = 1 });
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Retail", SellPriceMarginPercent = 50, SellPriceLevel = 2 });

            foreach(var sellPrice in product.ProductSellPrices)
            {
                sellPrice.Fees = new List<Fee>();
                if(sellPrice.SellPriceName == "Retail")
                {
                    sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 20, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
                }
            }

            // Run Calculator
            var feeCalculator = new FeeCalculator(product);
            feeCalculator.ComputeAllProductPrices(false);

            // Created to get same value though division
            decimal value = ((decimal)3 / (decimal).9) * (decimal)1.1;
            value = feeCalculator.RoundNumberToDecimalPlace(value, 3);
            Assert.AreEqual(value, product.ProductTotalCost);

            foreach(var sellPrice in product.ProductSellPrices)
            {
                decimal sellPriceValue = value / (decimal)(.50);
                foreach(var fee in sellPrice.Fees)
                {
                    if(fee.FeePercentType == MyExtensions.GetEnumDescription(PercentTypeList.Multiplication))
                    {
                        sellPriceValue = sellPriceValue * (decimal)1.2;
                    }
                    else
                    {
                        sellPriceValue = sellPriceValue / (decimal).8;
                    }
                }

                Assert.AreEqual(feeCalculator.RoundNumberToDecimalPlace(sellPriceValue, 3), sellPrice.SellPriceFinalAmount);

                // Set Sell price for new test
                sellPrice.SellPriceFinalAmount = (decimal)7.75;
            }

            feeCalculator.ComputeMarginBasedOnSellprice();

            int count = 1;
            foreach(var sellPrice in product.ProductSellPrices)
            {
                //Calcuate the margin based on the sell price Final Amount
                decimal finalSellPrice = sellPrice.SellPriceFinalAmount;
                decimal productTotalCost = (decimal)product.ProductTotalCost;

                // Calculate fees
                decimal baseSellPrice = finalSellPrice;

                // Calculate Margin
                decimal newMarginPercent = (100 - ((productTotalCost / baseSellPrice) * 100));


                // Calculate Margin Values
                if(count == 1)
                {
                    Assert.AreEqual((decimal)52.6839, sellPrice.SellPriceMarginPercent);
                    Assert.AreEqual((decimal)7.75, sellPrice.SellPriceFinalAmount);
                }
                if(count == 2)
                {
                    Assert.AreEqual((decimal)7.75, sellPrice.SellPriceFinalAmount);
                    Assert.AreEqual((decimal)43.2207, sellPrice.SellPriceMarginPercent);
                }
                count++;
            }
        }

        [TestMethod]
        public void Compute_Product_And_Calculate_Margin_Based_On_SellPrice_And_Round_To_Nearest_Five_Cent()
        {
            // Data Input
            var product = new Product() { ProductCost = 1, ProductAnnualSalesProjection = 100 };
            product.Fees = new List<Fee>();
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = 1 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Amortized), FeeAmortizedCharge = 100, FeeAmortizedType = MyExtensions.GetEnumDescription(AmortizedTypeList.Annual_Sales_Project) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Division) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
            product.ProductSellPrices = new List<ProductSellPrice>();
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Employee", SellPriceMarginPercent = 50, SellPriceLevel = 1 });
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Retail", SellPriceMarginPercent = 50, SellPriceLevel = 2 });

            foreach (var sellPrice in product.ProductSellPrices)
            {
                sellPrice.Fees = new List<Fee>();
                if (sellPrice.SellPriceName == "Retail")
                {
                    sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 20, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
                }
            }

            // Run Calculator
            var feeCalculator = new FeeCalculator(product);
            feeCalculator.ComputeAllProductPrices(true);

            // Created to get same value though division
            decimal value = ((decimal)3 / (decimal).9) * (decimal)1.1;
            value = feeCalculator.RoundNumberToDecimalPlace(value, 3);
            Assert.AreEqual(value, product.ProductTotalCost);

            foreach (var sellPrice in product.ProductSellPrices)
            {
                decimal sellPriceValue = value / (decimal)(.50);
                foreach (var fee in sellPrice.Fees)
                {
                    if (fee.FeePercentType == MyExtensions.GetEnumDescription(PercentTypeList.Multiplication))
                    {
                        sellPriceValue = sellPriceValue * (decimal)1.2;
                    }
                    else
                    {
                        sellPriceValue = sellPriceValue / (decimal).8;
                    }
                }

                Assert.AreEqual(feeCalculator.RoundAmountToNearestFiveCents(feeCalculator.RoundNumberToDecimalPlace(sellPriceValue, 3)), sellPrice.SellPriceFinalAmount);

                // Set Sell price for new test
                sellPrice.SellPriceFinalAmount = (decimal)7.75;
            }

            feeCalculator.ComputeMarginBasedOnSellprice();

            int count = 1;
            foreach (var sellPrice in product.ProductSellPrices)
            {
                //Calcuate the margin based on the sell price Final Amount
                decimal finalSellPrice = sellPrice.SellPriceFinalAmount;
                decimal productTotalCost = (decimal)product.ProductTotalCost;

                // Calculate fees
                decimal baseSellPrice = finalSellPrice;

                // Calculate Margin
                decimal newMarginPercent = (100 - ((productTotalCost / baseSellPrice) * 100));


                // Calculate Margin Values
                if (count == 1)
                {
                    Assert.AreEqual((decimal)52.6839, sellPrice.SellPriceMarginPercent);
                    Assert.AreEqual((decimal)7.75, sellPrice.SellPriceFinalAmount);
                }
                if (count == 2)
                {
                    Assert.AreEqual((decimal)7.75, sellPrice.SellPriceFinalAmount);
                    Assert.AreEqual((decimal)43.2207, sellPrice.SellPriceMarginPercent);
                }
                count++;
            }
        }

        [TestMethod]
        public void Compute_Product_And_Calculate_Margin_Based_On_SellPrice_Using_Same_Sell_Price()
        {
            // Data Input
            var product = new Product() { ProductCost = 1, ProductAnnualSalesProjection = 100 };
            product.Fees = new List<Fee>();
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = 1 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Amortized), FeeAmortizedCharge = 100, FeeAmortizedType = MyExtensions.GetEnumDescription(AmortizedTypeList.Annual_Sales_Project) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Division) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
            product.ProductSellPrices = new List<ProductSellPrice>();
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Employee", SellPriceMarginPercent = 50, SellPriceLevel = 1 });
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Retail", SellPriceMarginPercent = 50, SellPriceLevel = 2 });

            foreach(var sellPrice in product.ProductSellPrices)
            {
                sellPrice.Fees = new List<Fee>();
                if(sellPrice.SellPriceName == "Retail")
                {
                    sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 20, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
                }
            }

            // Run Calculator
            var feeCalculator = new FeeCalculator(product);
            feeCalculator.ComputeAllProductPrices(false);

            // Created to get same value though division
            decimal value = ((decimal)3 / (decimal).9) * (decimal)1.1;
            value = feeCalculator.RoundNumberToDecimalPlace(value, 3);
            Assert.AreEqual(value, product.ProductTotalCost);

            foreach(var sellPrice in product.ProductSellPrices)
            {
                decimal sellPriceValue = value / (decimal)(.50);
                foreach(var fee in sellPrice.Fees)
                {
                    if(fee.FeePercentType == MyExtensions.GetEnumDescription(PercentTypeList.Multiplication))
                    {
                        sellPriceValue = sellPriceValue * (decimal)1.2;
                    }
                    else
                    {
                        sellPriceValue = sellPriceValue / (decimal).8;
                    }
                }

                Assert.AreEqual(feeCalculator.RoundNumberToDecimalPlace(sellPriceValue, 3), sellPrice.SellPriceFinalAmount);

                // Do Not change final sell price and values SHOULD be the same
                //sellPrice.SellPriceFinalAmount = (decimal)7.75;
            }

            feeCalculator.ComputeMarginBasedOnSellprice();

            foreach(var sellPrice in product.ProductSellPrices)
            {
                // Calculate Margin Values
                decimal sellPriceValue = value / (decimal)(.50);
                foreach(var fee in sellPrice.Fees)
                {
                    if(fee.FeePercentType == MyExtensions.GetEnumDescription(PercentTypeList.Multiplication))
                    {
                        sellPriceValue = sellPriceValue * (decimal)1.2;
                    }
                    else
                    {
                        sellPriceValue = sellPriceValue / (decimal).8;
                    }
                }

                Assert.AreEqual(feeCalculator.RoundNumberToDecimalPlace(sellPriceValue, 3), sellPrice.SellPriceFinalAmount);
            }
        }
        
        [TestMethod]
        public void Compute_Product_And_Calculate_Margin_Test_Product_From_Reid()
        {
            // Data Input
            var product = new Product() { ProductCost = (decimal)2.94, ProductGatewayCDIMinumumOrder = 144 };
            product.Fees = new List<Fee>();
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = (decimal).4 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = (decimal).6 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = (decimal).2 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = (decimal).09 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = (decimal).1 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = (decimal).7 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = (decimal).2 });

            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Amortized), FeeAmortizedCharge = 12, FeeAmortizedType = MyExtensions.GetEnumDescription(AmortizedTypeList.GatewayCDI_Minimum_Order) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = (decimal)7.33, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });

            product.ProductSellPrices = new List<ProductSellPrice>();
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Employee/Corporate", SellPriceMarginPercent = 35, SellPriceLevel = 1 });
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Dealer", SellPriceMarginPercent = 35, SellPriceLevel = 2 });
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Retail", SellPriceMarginPercent = 35, SellPriceLevel = 3 });

            foreach(var sellPrice in product.ProductSellPrices)
            {
                sellPrice.Fees = new List<Fee>();
                sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = (decimal)1.5, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
                if(sellPrice.SellPriceName == "Dealer")
                {
                    sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Division) });
                    sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = (decimal)2.3, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
                }

                if(sellPrice.SellPriceName == "Retail")
                {
                    sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = (decimal)8.89, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Division) });
                    sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 32, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Division) });
                }
            }

            // Run Calculator
            var feeCalculator = new FeeCalculator(product);
            feeCalculator.ComputeAllProductPrices(false);
            
            // Check Values
            Assert.AreEqual((decimal)5.703,feeCalculator.RoundNumberToDecimalPlace((decimal)product.ProductTotalCost, 3));

            foreach(var sellPrice in product.ProductSellPrices)
            {
                if(sellPrice.SellPriceName == "Employee/Corporate")
                {
                    Assert.AreEqual((decimal)8.906, sellPrice.SellPriceFinalAmount);
                }
                if(sellPrice.SellPriceName == "Dealer")
                {
                    Assert.AreEqual((decimal)10.123, sellPrice.SellPriceFinalAmount);
                }
                if(sellPrice.SellPriceName == "Retail")
                {
                    Assert.AreEqual((decimal)14.374,sellPrice.SellPriceFinalAmount);
                }
            }

            feeCalculator.ComputeMarginBasedOnSellprice();

            foreach(var sellPrice in product.ProductSellPrices)
            {
                if(sellPrice.SellPriceName == "Employee/Corporate")
                {
                    Assert.AreEqual((decimal)8.906, Math.Truncate((decimal)sellPrice.SellPriceFinalAmount * 1000) / 1000);
                }
                if(sellPrice.SellPriceName == "Dealer")
                {
                    Assert.AreEqual((decimal)10.123, Math.Truncate((decimal)sellPrice.SellPriceFinalAmount * 1000) / 1000);
                }
                if(sellPrice.SellPriceName == "Retail")
                {
                    Assert.AreEqual((decimal)14.374, Math.Truncate((decimal)sellPrice.SellPriceFinalAmount * 1000) / 1000);
                }
            }
        }

        [TestMethod]
        public void Compute_Product_And_Calculate_Margin_Test_Product_With_Dollar_And_Amortized_SellPrice_Fees()
        {
            // Data Input
            var product = new Product() { ProductCost = (decimal)2.94, ProductGatewayCDIMinumumOrder = 144 };
            product.Fees = new List<Fee>();
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = (decimal).4 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = (decimal).6 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = (decimal).2 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = (decimal).09 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = (decimal).1 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = (decimal).7 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = (decimal).2 });

            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Amortized), FeeAmortizedCharge = 12, FeeAmortizedType = MyExtensions.GetEnumDescription(AmortizedTypeList.GatewayCDI_Minimum_Order) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = (decimal)7.33, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });

            product.ProductSellPrices = new List<ProductSellPrice>();
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Employee/Corporate", SellPriceMarginPercent = 35, SellPriceLevel = 1 });
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Dealer", SellPriceMarginPercent = 35, SellPriceLevel = 2 });
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Retail", SellPriceMarginPercent = 35, SellPriceLevel = 3 });

            foreach(var sellPrice in product.ProductSellPrices)
            {
                sellPrice.Fees = new List<Fee>();
                sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = (decimal)1.5, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
                if(sellPrice.SellPriceName == "Dealer")
                {
                    sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Division) });
                    sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = (decimal)2.3, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
                    sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = (decimal)2 });
                }

                if(sellPrice.SellPriceName == "Retail")
                {
                    sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = (decimal)8.89, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Division) });
                    sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 32, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Division) });
                }
            }

            // Run Calculator
            var feeCalculator = new FeeCalculator(product);
            feeCalculator.ComputeAllProductPrices(false);

            // Check Values
            Assert.AreEqual((decimal)5.703, feeCalculator.RoundNumberToDecimalPlace((decimal)product.ProductTotalCost, 3));

            foreach(var sellPrice in product.ProductSellPrices)
            {
                if(sellPrice.SellPriceName == "Employee/Corporate")
                {
                    Assert.AreEqual((decimal)8.906, sellPrice.SellPriceFinalAmount);
                }
                if(sellPrice.SellPriceName == "Dealer")
                {
                    Assert.AreEqual((decimal)12.123, sellPrice.SellPriceFinalAmount);
                }
                if(sellPrice.SellPriceName == "Retail")
                {
                    Assert.AreEqual((decimal)14.374, sellPrice.SellPriceFinalAmount);
                }
            }

            feeCalculator.ComputeMarginBasedOnSellprice();

            foreach(var sellPrice in product.ProductSellPrices)
            {
                if(sellPrice.SellPriceName == "Employee/Corporate")
                {
                    Assert.AreEqual((decimal)8.906, Math.Truncate((decimal)sellPrice.SellPriceFinalAmount * 1000) / 1000);
                }
                if(sellPrice.SellPriceName == "Dealer")
                {
                    Assert.AreEqual((decimal)12.123, Math.Truncate((decimal)sellPrice.SellPriceFinalAmount * 1000) / 1000);
                }
                if(sellPrice.SellPriceName == "Retail")
                {
                    Assert.AreEqual((decimal)14.374, Math.Truncate((decimal)sellPrice.SellPriceFinalAmount * 1000) / 1000);
                }
            }
        }

        [TestMethod]
        public void Compute_Product_With_Upcharge()
        {
            // Data Input
            var product = new Product() { ProductCost = 1, ProductAnnualSalesProjection = 100 };
            product.ProductUpcharges = new List<ProductUpcharge>();
            ProductUpcharge upcharge1 = new ProductUpcharge();
            upcharge1.UpchargeAmount = 2;
            upcharge1.UpchargeSellPrices = new List<UpchargeSellPrice>();
            upcharge1.UpchargeSellPrices.Add(new UpchargeSellPrice() { UpchargeSellPriceName = "Employee", UpchargeSellPriceLevel = 1 });
            upcharge1.UpchargeSellPrices.Add(new UpchargeSellPrice() { UpchargeSellPriceName = "Retail", UpchargeSellPriceLevel = 2 });
            
            product.ProductUpcharges.Add(upcharge1);
            product.Fees = new List<Fee>();
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount), FeeDollarAmount = 1 });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Amortized), FeeAmortizedCharge = 100, FeeAmortizedType = MyExtensions.GetEnumDescription(AmortizedTypeList.Annual_Sales_Project) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Division) });
            product.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 10, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
            product.ProductSellPrices = new List<ProductSellPrice>();
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Employee", SellPriceMarginPercent = 50, SellPriceLevel = 1 });
            product.ProductSellPrices.Add(new ProductSellPrice() { SellPriceName = "Retail", SellPriceMarginPercent = 50, SellPriceLevel = 2 });

            foreach(var sellPrice in product.ProductSellPrices)
            {
                sellPrice.Fees = new List<Fee>();
                if(sellPrice.SellPriceName == "Retail")
                {
                    sellPrice.Fees.Add(new Fee() { FeeType = MyExtensions.GetEnumDescription(FeeTypeList.Percent), FeePercent = 20, FeePercentType = MyExtensions.GetEnumDescription(PercentTypeList.Multiplication) });
                }
            }

            // Run Calculator
            var feeCalculator = new FeeCalculator(product);
            feeCalculator.ComputeAllProductPrices(false);

            // Created to get same value though division
            decimal value = ((decimal)3 / (decimal).9) * (decimal)1.1;
            value = feeCalculator.RoundNumberToDecimalPlace(value, 3);
            Assert.AreEqual(value, product.ProductTotalCost);
            
            foreach(var sellPrice in product.ProductSellPrices)
            {
                decimal sellPriceValue = value / (decimal)(.50);
                foreach(var fee in sellPrice.Fees)
                {
                    if(fee.FeePercentType == MyExtensions.GetEnumDescription(PercentTypeList.Multiplication))
                    {
                        sellPriceValue = sellPriceValue * (decimal)1.2;
                    }
                    else
                    {
                        sellPriceValue = sellPriceValue / (decimal).8;
                    }
                }

                Assert.AreEqual(feeCalculator.RoundNumberToDecimalPlace(sellPriceValue, 3), sellPrice.SellPriceFinalAmount);
            }

            // Check Upcharge values
            foreach(var upcharge in product.ProductUpcharges)
            {
                // Created to get same value though division
                decimal newValue = (decimal)1;// cost
                newValue += 2; // upcharge
                newValue += 2; // dollar charges
                newValue = (decimal)(newValue / ((decimal)1 - (decimal)((decimal)10 / (decimal)100))); // divide
                newValue = (decimal)(((decimal)10 / (decimal)100) + (decimal)1) * newValue; // multiply
                newValue = feeCalculator.RoundNumberToDecimalPlace(newValue, 3);

                Assert.AreEqual(newValue, upcharge.UpchargeTotalCost);

                foreach(var upchargeSellPrice in upcharge.UpchargeSellPrices)
                {
                    decimal sellPriceValue = newValue / (decimal)(.50);
                    foreach(var fee in product.ProductSellPrices.Where(p => p.SellPriceLevel == upchargeSellPrice.UpchargeSellPriceLevel).FirstOrDefault().Fees)
                    {
                        if(fee.FeePercentType == MyExtensions.GetEnumDescription(PercentTypeList.Multiplication))
                        {
                            sellPriceValue = sellPriceValue * (decimal)1.2;
                        }
                        else
                        {
                            sellPriceValue = sellPriceValue / (decimal).8;
                        }
                    }
                    Assert.AreEqual(feeCalculator.RoundNumberToDecimalPlace(sellPriceValue, 3), upchargeSellPrice.UpchargeSellPriceFinalAmount);
                }
            }
        }
    }
}
