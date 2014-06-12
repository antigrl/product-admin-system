using NPR2._0._8.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NPR2._0._8.Models
{
    public class CampaignMetadata
    {
        public CampaignMetadata()
        {
            // Non-Generated 
            this.CampaignCreatedOnDate = DateTime.Now;
            this.CampaignStatus = MyExtensions.GetEnumDescription(Status.Pending_Approval);
        }

        [HiddenInput]
        [DisplayName("Campaign #")]
        public object CampaignID { get; set; }

        [DisplayName("Campaign Name")]
        [StringLength(150)]
        [Required]
        public object CampaignName { get; set; }

        [Required]
        public object CompanyID { get; set; }

        [DisplayName("Reason")]
        [StringLength(150)]
        [Required]
        public object CampaignReason { get; set; }

        [DisplayName("Product Count")]
        public object CampaignProductCount { get; set; }

        [DisplayName("Account Manager")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public object CampaignAccountManagerComments { get; set; }

        [DisplayName("Merchandiser")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public object CampaignMerchandiserComments { get; set; }

        [DisplayName("Inventory Buyer")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public object CampaignInventoryBuyerComments { get; set; }

        [DisplayName("Mentor")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public object CampaignMentorComments { get; set; }

        [DisplayName("Status")]
        [StringLength(150)]
        [Required]
        public object CampaignStatus { get; set; }

        [DisplayName("Created By")]
        [HiddenInput]
        public object CampaignCreatedBy { get; set; }

        [DisplayName("Created On")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [HiddenInput]
        public object CampaignCreatedOnDate { get; set; }
    }

    public class CompanyMetadata
    {
        public CompanyMetadata()
        {
            // Non-Generated 
            this.CompanyStatus = MyExtensions.GetEnumDescription(Status.Pending_Approval);
        }

        [DisplayName("Company Name")]
        [StringLength(150)]
        [Required]
        public object CompanyName { get; set; }

        [DisplayName("Company-Division")]
        [StringLength(5)]
        [Required]
        public object CompanyDivisionNumber { get; set; }

        [DisplayName("Company Logo")]
        public object CompanyImage { get; set; }

        [DisplayName("Logo Type")]
        [HiddenInput]
        public object CompanyImageType { get; set; }

        [DisplayName("Company Contact")]
        [StringLength(150)]
        [Required]
        public object CompanyContactName { get; set; }

        [DisplayName("Company Contact Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(150)]
        [Required]
        public object CompanyContactEmail { get; set; }

        [DisplayName("Account Manager")]
        [StringLength(150)]
        [Required]
        public object CompanyAccountManagerName { get; set; }

        [DisplayName("Account Manager Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(150)]
        [Required]
        public object CompanyAccountManagerEmail { get; set; }

        [DisplayName("Mentor")]
        [StringLength(150)]
        [Required]
        public object CompanyMentorName { get; set; }

        [DisplayName("Mentor Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(150)]
        [Required]
        public object CompanyMentorEmail { get; set; }

        [DisplayName("Company Location")]
        [StringLength(150)]
        public object CompanyLocation { get; set; }

        [DisplayName("Additional Details")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public object CompanyAdditionalDetails { get; set; }

        [DisplayName("Status")]
        [StringLength(150)]
        public object CompanyStatus { get; set; }

        [DisplayName("Website URL")]
        [DataType(DataType.Url)]
        [StringLength(150)]
        public object CompanyUrl { get; set; }

        [DisplayName("Default Margin")]
        [Required]
        public object CompanyDefaultMargin { get; set; }
    }

    public class DecorationMethodMetadata
    {
        [DisplayName("Decoration Method")]
        [StringLength(150)]
        [Required]
        public object DecorationMethodName { get; set; }
    }

    public class FeeMetadata
    {
        [DisplayName("Fee Name")]
        [Required]
        public object FeeNameID { get; set; }

        [DisplayName("Fee Type")]
        [StringLength(150)]
        [Required]
        public object FeeType { get; set; }

        [DisplayName("Fee Level")]
        public object FeeLevel { get; set; }

        [DisplayName("Inherited?")]
        [Required]
        public bool FeeInherited { get; set; }
        public object FeeInheritedFeeID { get; set; }

        [DisplayName("Fee Calculation")]
        [StringLength(150)]
        public object FeeCalculation { get; set; }

        [DisplayName("Fee Amortized Charge")]
        public object FeeAmortizedCharge { get; set; }

        [DisplayName("Fee Amortized Type")]
        [StringLength(150)]
        public object FeeAmortizedType { get; set; }

        [DisplayName("Fee Percent")]
        public object FeePercent { get; set; }

        [DisplayName("Fee Percent Type")]
        [StringLength(150)]
        public object FeePercentType { get; set; }

        [DisplayName("Fee Dollar Value")]
        public object FeeDollarAmount { get; set; }


        public object ProductID { get; set; }
        public object ProductSellPriceID { get; set; }
    }

    public class FeeNameMetadata
    {
        [DisplayName("Fee Name")]
        [StringLength(150)]
        [Required]
        public object FeeNameName { get; set; }

        [DisplayName("Fee Type")]
        [StringLength(150)]
        public object FeeNameType { get; set; }
    }

    public class PackagingTypeMetadata
    {
        [DisplayName("Packaging Type")]
        [StringLength(150)]
        [Required]
        public object PackagingTypeName { get; set; }
    }

    public class PricingTierMetadata
    {
        [DisplayName("Tier Name")]
        [StringLength(150)]
        [Required]
        public object PricingTierName { get; set; }

        [DisplayName("Tier Level")]
        [Required]
        public object PricingTierLevel { get; set; }
    }

    public class ProductMetadata
    {
        [HiddenInput]
        [DisplayName("Product/NPR #")]
        public object ProductID { get; set; }

        [DisplayName("Product Name")]
        [StringLength(150)]
        [Required]
        public object ProductName { get; set; }

        [DisplayName("Product Image")]
        public object ProductImage { get; set; }

        [HiddenInput]
        public object ProductImageType { get; set; }

        [DisplayName("Description/Copy")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public object ProductDescription { get; set; }

        [DisplayName("Status")]
        [StringLength(150)]
        [Required]
        public object ProductStatus { get; set; }

        [DisplayName("Category")]
        [StringLength(150)]
        public object ProductCategory { get; set; }

        [DisplayName("Unit of Measure")]
        [StringLength(150)]
        [Required]
        public object ProductUnitOfMeasure { get; set; }

        [DisplayName("Packaging Type")]
        [Required]
        public object PackagingTypeID { get; set; }

        [DisplayName("Replacement Item Number")]
        [StringLength(150)]
        public object ProductReplacementItemNumber { get; set; }

        [DisplayName("Selected Sizes")]
        [StringLength(150)]
        public object ProductSelectedSizes { get; set; }

        [DisplayName("Selected Colors")]
        [StringLength(150)]
        public object ProductSelectedColors { get; set; }

        [DisplayName("Presentation Sell")]
        public object ProductPresentationSell { get; set; }

        [DisplayName("Quote Number")]
        public object ProductQuoteNumber { get; set; }

        [DisplayName("Sales Analysis")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public object ProductSalesAnalysis { get; set; }

        [DisplayName("Vendor Name")]
        [Required]
        public object VendorNameID { get; set; }

        [DisplayName("Vendor Type")]
        public object VendorTypeID { get; set; }

        [DisplayName("Vendor Lead Time")]
        [StringLength(150)]
        [Required]
        public object ProductVendorLeadTime { get; set; }

        [DisplayName("Vendor Minimum Order [VM]")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:0.###}", ApplyFormatInEditMode = true)]
        public object ProductVendorMinimumOrder { get; set; }

        [DisplayName("Country of Origin")]
        [StringLength(150)]
        public object ProductCountryOfOrigin { get; set; }

        [DisplayName("Harmonized Code")]
        [StringLength(150)]
        public object ProductHarmonizedCode { get; set; }

        [DisplayName("Initial Order Quantity [IOQ]")]
        [DisplayFormat(DataFormatString = "{0:0.###}", ApplyFormatInEditMode = true)]
        public object ProductInitialOrderQuantity { get; set; }

        [DisplayName("Vendor Item Number")]
        [StringLength(150)]
        [Required]
        public object ProductVendorItemNumber { get; set; }

        [DisplayName("Annual Sales Projection [ASP]")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:0.###}", ApplyFormatInEditMode = true)]
        public object ProductAnnualSalesProjection { get; set; }

        [DisplayName("GCDI Minimum Order [GMO]")]
        [DisplayFormat(DataFormatString = "{0:0.###}", ApplyFormatInEditMode = true)]
        public object ProductGatewayCDIMinumumOrder { get; set; }

        [DisplayName("Product Cost")]
        public object ProductCost { get; set; }

        [DisplayName("Net Cost")]
        public object ProductNetCost { get; set; }

        [DisplayName("Total Cost")]
        public object ProductTotalCost { get; set; }

        [DisplayName("Landed Cost")]
        public object ProductLandedCost { get; set; }

        [DisplayName("GatewayCDI SKU")]
        [StringLength(150)]
        public object ProductGatewayCDISKU { get; set; }

        [DisplayName("Item EDP")]
        [StringLength(150)]
        public object ProductItemEDP { get; set; }

        [DisplayName("Account Manager Comments")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public object ProductAccountManagerComments { get; set; }

        [DisplayName("Merchandiser Comments ")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public object ProductMerchandiserComments { get; set; }

        [DisplayName("Inventory Buyer Comments")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public object ProductInventoryBuyerComments { get; set; }

        [DisplayName("Mentor Comments")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public object ProductMentorComments { get; set; }

        [DisplayName("Campaign")]
        public object CampaignID { get; set; }
    }

    public class ProductDecorationMetadata
    {
        [HiddenInput]
        public object DecorationID { get; set; }

        [DisplayName("Decoration Name")]
        [StringLength(150)]
        [Required]
        public object DecorationName { get; set; }

        [DisplayName("Method")]
        [Required]
        public object DecorationMethodID { get; set; }

        [DisplayName("Color")]
        [StringLength(150)]
        [Required]
        public object DecorationColor { get; set; }

        [DisplayName("Location")]
        [StringLength(150)]
        [Required]
        public object DecorationLocation { get; set; }

        [DisplayName("Size")]
        [StringLength(150)]
        [Required]
        public object DecorationSize { get; set; }

        [DisplayName("Decoration Vendor")]
        [StringLength(150)]
        [Required]
        public object DecorationVendor { get; set; }

        [DisplayName("Decoration Image")]
        public object DecorationImage { get; set; }

        [HiddenInput]
        public object DecorationImageType { get; set; }

        [DisplayName("Design Number")]
        [StringLength(150)]
        public object DecorationEmbroideryDesignNumber { get; set; }

        [DisplayName("Stitch Count")]
        [StringLength(150)]
        public object DecorationEmbroideryStitchCount { get; set; }

        [DisplayName("Color Way")]
        [StringLength(150)]
        public object DecorationEmbroideryColorWay { get; set; }
    }

    public class ProductSellPriceMetadata
    {
        [DisplayName("Sell Price Name")]
        [StringLength(150)]
        [Required]
        public object SellPriceName { get; set; }

        [DisplayName("Sell Price Level")]
        [Required]
        public object SellPriceLevel { get; set; }

        [DisplayName("Margin Percent")]
        [Required]
        public object SellPriceMarginPercent { get; set; }

        [DisplayName("Margin Dollar Amount")]
        [Required]
        public object SellPriceMarginDollarAmount { get; set; }

        [DisplayName("Final Sell Price Amount")]
        [Required]
        public object SellPriceFinalAmount { get; set; }
    }

    public class ProductUpchargeMetadata
    {
        [HiddenInput]
        public object UpchargeID { get; set; }

        [DisplayName("Upcharge Name")]
        [StringLength(150)]
        [Required]
        public object UpchargeName { get; set; }

        [DisplayName("Upcharge Amount")]
        [Required]
        public object UpchargeAmount { get; set; }

        [DisplayName("Upcharge Total Cost")]
        public object UpchargeTotalCost { get; set; }
    }

    public class UpchargeSellPriceMetadata
    {
        [DisplayName("Upcharge Sell Price Name")]
        [StringLength(150)]
        [Required]
        public object UpchargeSellPriceName { get; set; }

        [DisplayName("Upcharge Sell Price Level")]
        [Required]
        public object UpchargeSellPriceLevel { get; set; }

        [DisplayName("Upcharge Sell Price Final Amount")]
        [Required]
        public object UpchargeSellPriceFinalAmount { get; set; }

        [DisplayName("Upcharge Sell Price Locked?")]
        public object UpchargeSellPriceLocked { get; set; }
    }

    public class VendorNameMetadata
    {
        [DisplayName("Vendor Name")]
        [StringLength(150)]
        [Required]
        public object VendorNameName { get; set; }
    }

    public class VendorTypeMetadata
    {
        [DisplayName("Vendor Type")]
        [StringLength(150)]
        [Required]
        public object VendorTypeName { get; set; }
    }
}