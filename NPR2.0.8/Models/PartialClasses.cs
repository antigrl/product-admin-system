using NPR2._0._8.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NPR2._0._8.Models
{
    [MetadataType(typeof(CampaignMetadata))]
    public partial class Campaign
    {
    }

    [MetadataType(typeof(CompanyMetadata))]
    public partial class Company
    {
    }

    [MetadataType(typeof(DecorationMethodMetadata))]
    public partial class DecorationMethod
    {
    }

    [MetadataType(typeof(FeeMetadata))]
    public partial class Fee : IValidatableObject
    {
        // Server side contential validation (Makes page fail on reload)
        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            if((FeeAmortizedCharge == null || FeeAmortizedType == null) && FeeType == MyExtensions.GetEnumDescription(FeeTypeList.Amortized))
            {
                yield return new ValidationResult("You must fill out all amortization information");
            }
            if((FeePercent == null || FeePercentType == null) && FeeType == MyExtensions.GetEnumDescription(FeeTypeList.Percent))
            {
                yield return new ValidationResult("You must fill out all percent fee information");
            }
        }

        //Non-Generated Constructors 
        public Fee(int productID)
        {
            this.ProductID = productID;
            this.FeeLevel = 0;
            this.AuditTrails = new HashSet<AuditTrail>();
        }
        // For Existing Fee Creation
        public Fee(int productID, string feeType, string feeCalculation, int feeNameID, decimal? feeDollarAmount, decimal? feeAmortizedCharge, string feeAmortizedType, decimal? feePercent, string feePercentType, int inheritedID)
        {
            this.ProductID = productID;
            this.FeeType = feeType;
            this.FeeCalculation = feeCalculation;
            this.FeeNameID = feeNameID;
            this.FeeDollarAmount = feeDollarAmount;
            this.FeeAmortizedCharge = feeAmortizedCharge;
            this.FeeAmortizedType = feeAmortizedType;
            this.FeePercent = feePercent;
            this.FeePercentType = feePercentType;
            this.FeeInherited = true;
            this.FeeInheritedFeeID = inheritedID;
            this.FeeLevel = 0;
            this.AuditTrails = new HashSet<AuditTrail>();
        }
        // For sell prices
        public Fee(int feeNameID, ProductSellPrice sellPrice, string feeType, string feeCalculation, decimal? feeDollarAmount, decimal? feeAmortizedCharge, string feeAmortizedType, decimal? feePercent, string feePercentType, int inheritedID)
        {
            this.ProductSellPrice = sellPrice;
            this.FeeType = feeType;
            this.FeeCalculation = feeCalculation;
            this.FeeNameID = feeNameID;
            this.FeeDollarAmount = feeDollarAmount;
            this.FeeAmortizedCharge = feeAmortizedCharge;
            this.FeeAmortizedType = feeAmortizedType;
            this.FeePercent = feePercent;
            this.FeePercentType = feePercentType;
            this.FeeInherited = true;
            this.FeeInheritedFeeID = inheritedID;
            this.FeeLevel = 0;
            this.AuditTrails = new HashSet<AuditTrail>();
        }

        public Fee(int productID, string feeType)
        {
            this.ProductID = productID;
            this.FeeType = feeType;
            this.FeeLevel = 0;
            this.AuditTrails = new HashSet<AuditTrail>();
        }
        public Fee(int productID, string feeType, string feeCalculation)
        {
            this.ProductID = productID;
            this.FeeType = feeType;
            this.FeeCalculation = feeCalculation;
            this.FeeLevel = 0;
            this.AuditTrails = new HashSet<AuditTrail>();
        }
        public Fee(FeeName name, string type, string calculationStep, int sellPriceID, decimal feePercent, decimal feeDollarAmount)
        {
            // TODO: Complete member initialization
            this.FeeName = name;
            this.FeeType = type;
            this.FeeCalculation = calculationStep;
            this.ProductSellPriceID = sellPriceID;
            this.FeePercent = feePercent;
            this.FeeDollarAmount = feeDollarAmount;
            this.FeeLevel = 0;
            this.AuditTrails = new HashSet<AuditTrail>();
        }

        public string GetFeeConnection()
        {
            FeeConnectionList connection = FeeConnectionList.Null;

            if(this.Company != null)
            {
                connection = FeeConnectionList.Company;
            }
            else if(this.Campaign != null)
            {
                connection = FeeConnectionList.Campaign;
            }
            else if(this.PricingTier != null)
            {
                connection = FeeConnectionList.Pricing_Tier;
            }
            else if(this.Product != null)
            {
                connection = FeeConnectionList.Product;
            }
            else if(this.ProductSellPrice != null)
            {
                connection = FeeConnectionList.SellPrice;
            }

            return MyExtensions.GetEnumDescription(connection);
        }
    }

    [MetadataType(typeof(FeeNameMetadata))]
    public partial class FeeName
    {
    }

    [MetadataType(typeof(PackagingTypeMetadata))]
    public partial class PackagingType
    {
    }

    [MetadataType(typeof(PricingTierMetadata))]
    public partial class PricingTier
    {
    }

    [MetadataType(typeof(ProductMetadata))]
    public partial class Product : IValidatableObject
    {
        // Server side contential validation (Makes page fail on reload)
        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            foreach(Fee fee in Fees)
            {
                // Check Initial Order Quantity
                if(fee.FeeType == MyExtensions.GetEnumDescription(FeeTypeList.Amortized))
                {
                    if(fee.FeeAmortizedType == MyExtensions.GetEnumDescription(AmortizedTypeList.Initial_Order_Quantity) && (ProductInitialOrderQuantity <= 0 || ProductInitialOrderQuantity == null))
                    {
                        yield return new ValidationResult("You must input an Initial Order Quantity[IOQ] to Amortize your fee.");
                    }
                    // Check GatewayCDI Minimum Order
                    else if(fee.FeeAmortizedType == MyExtensions.GetEnumDescription(AmortizedTypeList.GatewayCDI_Minimum_Order) && (ProductGatewayCDIMinumumOrder <= 0 || ProductGatewayCDIMinumumOrder == null))
                    {
                        yield return new ValidationResult("You must input a GatewayCDI Minimum Order[GMO] to Amortize your fee.");
                    }
                }
            }
        }
    }

    [MetadataType(typeof(ProductDecorationMetadata))]
    public partial class ProductDecoration
    {
        //Non-Generated Constructors 
        public ProductDecoration(int productID)
        {
            this.ProductID = productID;
            this.AuditTrails = new HashSet<AuditTrail>();
        }
    }

    [MetadataType(typeof(ProductSellPriceMetadata))]
    public partial class ProductSellPrice
    {
        //non-generated Costructor
        public ProductSellPrice(Product product, string name, int level, decimal defaultMargin)
        {
            // TODO: Complete member initialization
            this.Product = product;
            this.SellPriceName = name;
            this.SellPriceLevel = level;
            this.SellPriceMarginPercent = defaultMargin;
            this.AuditTrails = new HashSet<AuditTrail>();
        }
    }

    [MetadataType(typeof(ProductUpchargeMetadata))]
    public partial class ProductUpcharge
    {
    }

    [MetadataType(typeof(UpchargeSellPriceMetadata))]
    public partial class UpchargeSellPrice
    {
        public UpchargeSellPrice(ProductUpcharge upcharge, string name, int level)
        {
            // TODO: Complete member initialization
            this.ProductUpcharge = upcharge;
            this.UpchargeSellPriceName = name;
            this.UpchargeSellPriceLevel = level;
            this.AuditTrails = new HashSet<AuditTrail>();
        }
    }

    [MetadataType(typeof(VendorNameMetadata))]
    public partial class VendorName
    {
    }

    [MetadataType(typeof(VendorTypeMetadata))]
    public partial class VendorType
    {
    }
}