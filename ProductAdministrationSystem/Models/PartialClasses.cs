using PAS.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PAS.Models
{
    [MetadataType(typeof(AuditTrailMetadata))]
    public partial class AuditTrail
    {
        public AuditTrail()
        {
        }

        // Campaign 
        public AuditTrail(DateTime dateTime, string userName, Campaign campaign, int id, string comment)
        {
            this.AuditTrailTimeStamp = dateTime;
            this.AuditTrailUserName = userName;
            this.AuditTrailComment = comment;

            if(id > 0)
            {
                this.CampaignID = id;
            }
            else
            {
                this.Campaign = campaign;
            }
        }

        // Company 
        public AuditTrail(DateTime dateTime, string userName, Company company, int id, string comment)
        {
            this.AuditTrailTimeStamp = dateTime;
            this.AuditTrailUserName = userName;
            this.AuditTrailComment = comment;

            if(id > 0)
            {
                this.CompanyID = id;
            }
            else
            {
                this.Company = company;
            }
        }

        // Decoration Method
        public AuditTrail(DateTime dateTime, string userName, DecorationMethod decorationmethod, int id, string comment)
        {
            this.AuditTrailTimeStamp = dateTime;
            this.AuditTrailUserName = userName;
            this.AuditTrailComment = comment;

            if(id > 0)
            {
                this.DecorationMethodID = id;
            }
            else
            {
                this.DecorationMethod = decorationmethod;
            }
        }

        // FeeName 
        public AuditTrail(DateTime dateTime, string userName, FeeName feeName, int id, string comment)
        {
            this.AuditTrailTimeStamp = dateTime;
            this.AuditTrailUserName = userName;
            this.AuditTrailComment = comment;

            if(id > 0)
            {
                this.FeeNameID = id;
            }
            else
            {
                this.FeeName = feeName;
            }
        }

        // PackagingType 
        public AuditTrail(DateTime dateTime, string userName, PackagingType packagingType, int id, string comment)
        {
            this.AuditTrailTimeStamp = dateTime;
            this.AuditTrailUserName = userName;
            this.AuditTrailComment = comment;

            if(id > 0)
            {
                this.PackagingTypeID = id;
            }
            else
            {
                this.PackagingType = packagingType;
            }
        }

        // PricingTier 
        public AuditTrail(DateTime dateTime, string userName, PricingTier pricingTier, int id, string comment)
        {
            this.AuditTrailTimeStamp = dateTime;
            this.AuditTrailUserName = userName;
            this.AuditTrailComment = comment;

            if(id > 0)
            {
                this.PricingTierID = id;
            }
            else
            {
                this.PricingTier = pricingTier;
            }
        }

        // Product 
        public AuditTrail(DateTime dateTime, string userName, Product product, int id, string comment)
        {
            this.AuditTrailTimeStamp = dateTime;
            this.AuditTrailUserName = userName;
            this.AuditTrailComment = comment;

            if(id > 0)
            {
                this.ProductID = id;
            }
            else
            {
                this.Product = product;
            }
        }

        // ProductDecoration 
        public AuditTrail(DateTime dateTime, string userName, ProductDecoration productDecoration, int id, string comment)
        {
            this.AuditTrailTimeStamp = dateTime;
            this.AuditTrailUserName = userName;
            this.AuditTrailComment = comment;

            if(id > 0)
            {
                this.DecorationID = id;
            }
            else
            {
                this.ProductDecoration = productDecoration;
            }
        }

        // VendorName 
        public AuditTrail(DateTime dateTime, string userName, VendorName vendorName, int id, string comment)
        {
            this.AuditTrailTimeStamp = dateTime;
            this.AuditTrailUserName = userName;
            this.AuditTrailComment = comment;

            if(id > 0)
            {
                this.VendorNameID = id;
            }
            else
            {
                this.VendorName = vendorName;
            }
        }

        // VendorType 
        public AuditTrail(DateTime dateTime, string userName, VendorType vendorType, int id, string comment)
        {
            this.AuditTrailTimeStamp = dateTime;
            this.AuditTrailUserName = userName;
            this.AuditTrailComment = comment;

            if(id > 0)
            {
                this.VendorTypeID = id;
            }
            else
            {
                this.VendorType = vendorType;
            }
        }

        // Category 
        public AuditTrail(DateTime dateTime, string userName, Category category, int id, string comment)
        {
            this.AuditTrailTimeStamp = dateTime;
            this.AuditTrailUserName = userName;
            this.AuditTrailComment = comment;

            if (id > 0)
            {
                this.CategoryID = id;
            }
            else
            {
                this.Category = category;
            }
        }

        // Fee 
        public AuditTrail(DateTime dateTime, string userName, Fee fee, int id, string comment)
        {
            this.AuditTrailTimeStamp = dateTime;
            this.AuditTrailUserName = userName;
            this.AuditTrailComment = comment;

            if(id > 0)
            {
                this.FeeID = id;
            }
            else
            {
                this.Fee = fee;
            }
        }

        // ProductSellPrice 
        public AuditTrail(DateTime dateTime, string userName, ProductSellPrice productSellPrice, int id, string comment)
        {
            this.AuditTrailTimeStamp = dateTime;
            this.AuditTrailUserName = userName;
            this.AuditTrailComment = comment;

            if(id > 0)
            {
                this.SellPriceID = id;
            }
            else
            {
                this.ProductSellPrice = productSellPrice;
            }
        }

        // ProductUpcharge 
        public AuditTrail(DateTime dateTime, string userName, ProductUpcharge productUpcharge, int id, string comment)
        {
            this.AuditTrailTimeStamp = dateTime;
            this.AuditTrailUserName = userName;
            this.AuditTrailComment = comment;

            if(id > 0)
            {
                this.UpchargeID = id;
            }
            else
            {
                this.ProductUpcharge = productUpcharge;
            }
        }

        // MajorCategoryOrdering 
        public AuditTrail(DateTime dateTime, string userName, MajorCategoryOrdering majorCategoryOrdering, int id, string comment)
        {
            this.AuditTrailTimeStamp = dateTime;
            this.AuditTrailUserName = userName;
            this.AuditTrailComment = comment;

            if (id > 0)
            {
                this.MajorCategoryOrderingID = id;
            }
            else
            {
                this.MajorCategoryOrdering = majorCategoryOrdering;
            }
        }

        // UpchargeSellPrice
        public AuditTrail(DateTime dateTime, string userName, UpchargeSellPrice upchargeSellPrice, int id, string comment)
        {
            this.AuditTrailTimeStamp = dateTime;
            this.AuditTrailUserName = userName;
            this.AuditTrailComment = comment;

            if(id > 0)
            {
                this.UpchargeSellPriceID = id;
            }
            else
            {
                this.UpchargeSellPrice = upchargeSellPrice;
            }
        }

        public string GetAuditTrailType()
        {
            ObjectType type = ObjectType.Null;

            if (this.Campaign != null)
            {
                type = ObjectType.Campaign;
            }
            else if (this.Category != null)
            {
                type = ObjectType.Category;
            }
            else if (this.Company != null)
            {
                type = ObjectType.Company;
            }
            else if (this.DecorationMethod != null)
            {
                type = ObjectType.Decoration_Method;
            }
            else if (this.Fee != null)
            {
                type = ObjectType.Fee;
            }
            else if (this.FeeName != null)
            {
                type = ObjectType.Fee_Name;
            }
            else if (this.PackagingType != null)
            {
                type = ObjectType.Packaging_Type;
            }
            else if (this.PricingTier != null)
            {
                type = ObjectType.Pricing_Tier;
            }
            else if (this.Product != null)
            {
                type = ObjectType.Product;
            }
            else if (this.ProductDecoration != null)
            {
                type = ObjectType.Product_Decoration;
            }
            else if (this.ProductSellPrice != null)
            {
                type = ObjectType.Product_Sell_Price;
            }
            else if (this.ProductUpcharge != null)
            {
                type = ObjectType.Product_Upcharge;
            }
            else if (this.UpchargeSellPrice != null)
            {
                type = ObjectType.Upcharge_Sell_Price;
            }
            else if (this.VendorName != null)
            {
                type = ObjectType.Vendor_Name;
            }
            else if (this.VendorType != null)
            {
                type = ObjectType.Vendor_Type;
            }
            else if (this.MajorCategoryOrdering != null)
            {
                type = ObjectType.Major_Category_Ordering;
            }

            return MyExtensions.GetEnumDescription(type);
        }

        public int? GetAuditTrailTypeID()
        {
            if (this.Campaign != null)
            {
                return this.CampaignID;
            }
            else if (this.Category != null)
            {
                return this.CategoryID;
            }
            else if (this.Company != null)
            {
                return this.CompanyID;
            }
            else if (this.DecorationMethod != null)
            {
                return this.DecorationMethodID;
            }
            else if (this.Fee != null)
            {
                return this.FeeID;
            }
            else if (this.FeeName != null)
            {
                return this.FeeNameID;
            }
            else if (this.PackagingType != null)
            {
                return this.PackagingTypeID;
            }
            else if (this.PricingTier != null)
            {
                return this.PricingTierID;
            }
            else if (this.Product != null)
            {
                return this.ProductID;
            }
            else if (this.ProductDecoration != null)
            {
                return this.DecorationID;
            }
            else if (this.ProductSellPrice != null)
            {
                return this.SellPriceID;
            }
            else if (this.ProductUpcharge != null)
            {
                return this.UpchargeID;
            }
            else if (this.UpchargeSellPrice != null)
            {
                return this.UpchargeSellPriceID;
            }
            else if (this.VendorName != null)
            {
                return this.VendorNameID;
            }
            else if (this.VendorType != null)
            {
                return this.VendorTypeID;
            }
            else if (this.MajorCategoryOrdering != null)
            {
                return this.MajorCategoryOrderingID;
            }

            return 0;
        }
    }

    [MetadataType(typeof(MajorCategoryOrderingMetedata))]
    public partial class MajorCategoryOrdering
    {
        public void OnCreate(int sortValue, int campaignID, int categoryID, bool showCategory, string categoryRename = null)
        {
            this.SortValue = SortValue;
            this.CampaignID = campaignID;
            this.CategoryID = categoryID;
            this.ShowCategory = showCategory;
            if (string.IsNullOrEmpty(categoryRename) == false)
            {
                this.CategoryRename = categoryRename;
            }
        }
    }

    [MetadataType(typeof(CampaignMetadata))]
    public partial class Campaign
    {
        // Default Constructor additions
        public void OnCreate(string name)
        {
            this.CampaignCreatedOnDate = DateTime.Now;
            this.CampaignStatus = MyExtensions.GetEnumDescription(Status.New);
            this.CampaignCreatedBy = name;
        }
    }

    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {
        // Default Constructor additions
        public void OnCreate()
        {
            this.CategoryStatus = MyExtensions.GetEnumDescription(Status.Active);
        }
    }

    [MetadataType(typeof(CompanyMetadata))]
    public partial class Company
    {
        // Default Constructor additions
        public void OnCreate()
        {
            this.CompanyStatus = MyExtensions.GetEnumDescription(Status.New);
        }
    }

    [MetadataType(typeof(DecorationMethodMetadata))]
    public partial class DecorationMethod
    {
        // Default Constructor additions
        public void OnCreate()
        {
            this.DecorationMethodStatus = MyExtensions.GetEnumDescription(Status.Active);
        }
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
            if((FeeDollarAmount == null || FeeDollarAmount <= 0) && FeeType == MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount))
            {
                yield return new ValidationResult("You must fill out all dollar fee information");
            }
        }

        // Default Constructor additions
        public void OnCreate()
        {
            this.FeeStatus = MyExtensions.GetEnumDescription(Status.Active);
        }
        //Non-Generated Constructors 
        public Fee(int productID)
        {
            this.ProductID = productID;
            this.FeeLevel = 0;
            this.AuditTrails = new HashSet<AuditTrail>();
            this.FeeStatus = MyExtensions.GetEnumDescription(Status.Active);
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
            this.FeeStatus = MyExtensions.GetEnumDescription(Status.Active);
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
            this.FeeStatus = MyExtensions.GetEnumDescription(Status.Active);
        }

        public Fee(int productID, string feeType)
        {
            this.ProductID = productID;
            this.FeeType = feeType;
            this.FeeLevel = 0;
            this.AuditTrails = new HashSet<AuditTrail>();
            this.FeeStatus = MyExtensions.GetEnumDescription(Status.Active);
        }
        public Fee(int productID, string feeType, string feeCalculation)
        {
            this.ProductID = productID;
            this.FeeType = feeType;
            this.FeeCalculation = feeCalculation;
            this.FeeLevel = 0;
            this.AuditTrails = new HashSet<AuditTrail>();
            this.FeeStatus = MyExtensions.GetEnumDescription(Status.Active);
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
            this.FeeStatus = MyExtensions.GetEnumDescription(Status.Active);
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
        // Default Constructor additions
        public void OnCreate()
        {
            this.FeeNameStatus = MyExtensions.GetEnumDescription(Status.Active);
        }
    }

    [MetadataType(typeof(PackagingTypeMetadata))]
    public partial class PackagingType
    {
        // Default Constructor additions
        public void OnCreate()
        {
            this.PackagingTypeStatus = MyExtensions.GetEnumDescription(Status.Active);
        }
    }

    [MetadataType(typeof(PricingTierMetadata))]
    public partial class PricingTier
    {
        // Default Constructor additions
        public void OnCreate()
        {
            this.PricingTierStatus = MyExtensions.GetEnumDescription(Status.Active);
        }
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
        // Default Constructor additions
        public void OnCreate()
        {
            this.DecorationStatus = MyExtensions.GetEnumDescription(Status.Active);
        }
        //Non-Generated Constructors 
        public ProductDecoration(int productID)
        {
            this.ProductID = productID;
            this.AuditTrails = new HashSet<AuditTrail>();
            this.DecorationStatus = "Active";
        }
    }

    [MetadataType(typeof(ProductSellPriceMetadata))]
    public partial class ProductSellPrice
    {
        // Default Constructor additions
        public void OnCreate()
        {
            this.SellPriceStatus = MyExtensions.GetEnumDescription(Status.Active);
        }
        //non-generated Costructor
        public ProductSellPrice(Product product, string name, int level, decimal defaultMargin)
        {
            // TODO: Complete member initialization
            this.Product = product;
            this.SellPriceName = name;
            this.SellPriceLevel = level;
            this.SellPriceMarginPercent = defaultMargin;
            this.AuditTrails = new HashSet<AuditTrail>();
            this.SellPriceStatus = "Active";
        }
    }

    [MetadataType(typeof(ProductUpchargeMetadata))]
    public partial class ProductUpcharge
    {
        // Default Constructor additions
        public void OnCreate()
        {
            this.UpchargeStatus = MyExtensions.GetEnumDescription(Status.Active);
        }
        //Non-Generated Constructors 
        public ProductUpcharge(int productID)
        {
            this.ProductID = productID;
            this.AuditTrails = new HashSet<AuditTrail>();
            this.UpchargeStatus = "Active";
        }
    }

    [MetadataType(typeof(UpchargeSellPriceMetadata))]
    public partial class UpchargeSellPrice
    {
        // Default Constructor additions
        public void OnCreate()
        {
            this.UpchargeSellPriceStatus = MyExtensions.GetEnumDescription(Status.Active);
        }
        // Non-generated Constructor
        public UpchargeSellPrice(ProductUpcharge upcharge, string name, int level)
        {
            // TODO: Complete member initialization
            this.ProductUpcharge = upcharge;
            this.UpchargeSellPriceName = name;
            this.UpchargeSellPriceLevel = level;
            this.AuditTrails = new HashSet<AuditTrail>();
            this.UpchargeSellPriceStatus = "Active";
        }
    }

    [MetadataType(typeof(VendorNameMetadata))]
    public partial class VendorName
    {
        // Default Constructor additions
        public void OnCreate()
        {
            this.VendorNameStatus = MyExtensions.GetEnumDescription(Status.Active);
        }
    }

    [MetadataType(typeof(VendorTypeMetadata))]
    public partial class VendorType
    {
        // Default Constructor additions
        public void OnCreate()
        {
            this.VendorTypeStatus = MyExtensions.GetEnumDescription(Status.Active);
        }
    }
}