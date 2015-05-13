using PAS.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PAS.Models
{
    public class AuditTrailMetadata
    {
        [DisplayName("Time Stamp")]
        public object AuditTrailTimeStamp { get; set; }

        [DisplayName("User")]
        public object AuditTrailUserName { get; set; }

        [DisplayName("Commet")]
        public object AuditTrailComment{ get; set; }
    }

    public class MajorCategoryOrderingMetedata
    {
        [DisplayName("Sort Value")]
        public object SortValue { get; set; }

        [DisplayName("Category Rename")]
        public object CategoryRename { get; set; }
    }

    public class CampaignMetadata
    {
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

    public class CategoryMetadata
    {
        [DisplayName("Category Name")]
        [StringLength(150)]
        [Required]
        public object CategoryName { get; set; }

        [DisplayName("Category Type")]
        [StringLength(150)]
        [Required]
        public object CategoryType { get; set; }

        [DisplayName("Status")]
        [StringLength(150)]
        [Required]
        public object CategoryStatus { get; set; }
    }

    public class CompanyMetadata
    {
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

        [DisplayName("Status")]
        [StringLength(150)]
        [Required]
        public object DecorationMethodStatus { get; set; }
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
        [DisplayFormat(DataFormatString = "{0:C}")]
        public object FeeDollarAmount { get; set; }

        [DisplayName("Status")]
        [StringLength(150)]
        [Required]
        public object FeeStatus { get; set; }

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

        [DisplayName("Status")]
        [StringLength(150)]
        [Required]
        public object FeeNameStatus { get; set; }
    }

    public class PackagingTypeMetadata
    {
        [DisplayName("Packaging Type")]
        [StringLength(150)]
        [Required]
        public object PackagingTypeName { get; set; }

        [DisplayName("Status")]
        [StringLength(150)]
        [Required]
        public object PackagingTypeStatus { get; set; }
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

        [DisplayName("Status")]
        [StringLength(150)]
        [Required]
        public object PricingTierStatus { get; set; }
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
        [Required]
        public object ProductSelectedColors { get; set; }

        [DisplayName("Presentation Sell")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public object ProductPresentationSell { get; set; }

        [DisplayName("Quote Number")]
        public object ProductQuoteNumber { get; set; }

        [DisplayName("Sales History")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public object ProductSalesHistory { get; set; }

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
        [DisplayFormat(DataFormatString = "{0:C}")]
        public object ProductCost { get; set; }

        [DisplayName("Net Cost")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public object ProductNetCost { get; set; }

        [DisplayName("Total Cost")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public object ProductTotalCost { get; set; }

        [DisplayName("Landed Cost")]
        public object ProductLandedCost { get; set; }

        [DisplayName("GatewayCDI SKU")]
        [StringLength(150)]
        public object ProductGatewayCDISKU { get; set; }

        [DisplayName("Item EDP")]
        [StringLength(150)]
        public object ProductItemEDP { get; set; }

        [DisplayName("Account Manager")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public object ProductAccountManagerComments { get; set; }

        [DisplayName("Merchandiser ")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public object ProductMerchandiserComments { get; set; }

        [DisplayName("Inventory Buyer")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public object ProductInventoryBuyerComments { get; set; }

        [DisplayName("Mentor")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public object ProductMentorComments { get; set; }

        [DisplayName("Campaign")]
        public object CampaignID { get; set; }

        [DisplayName("Major Category")]
        [Required]
        public object MajorCategoryID { get; set; }

        [DisplayName("Minor Category")]
        public object MinorCategoryID { get; set; }
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

        [DisplayName("Status")]
        [StringLength(150)]
        [Required]
        public object DecorationStatus { get; set; }
    }

    public class ProductDocumentMetadata
    {
        [HiddenInput]
        [DisplayName("ID")]
        public object ID { get; set; }

        [DisplayName("Document")]
        public object Document { get; set; }

        [DisplayName("Expiration Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public object ExpirationDate { get; set; }

        [DisplayName("Status")]
        [StringLength(150)]
        [Required]
        public object Status { get; set; }

        [DisplayName("Attachment Type ID")]
        [Required]
        public object AttachmentTypeID { get; set; }

        [DisplayName("File Type")]
        [StringLength(150)]
        public object DocumentFileType { get; set; }
        
        [DisplayName("Product ID")]
        [Required]
        public object ProductID { get; set; }
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
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required]
        public object SellPriceMarginDollarAmount { get; set; }

        [DisplayName("Final Sell Price Amount")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required]
        public object SellPriceFinalAmount { get; set; }

        [DisplayName("Status")]
        [StringLength(150)]
        [Required]
        public object SellPriceStatus { get; set; }
    }

    public class ProductAttachmentTypeMetadata
    {
        [DisplayName("Selected")]
        [Required]
        public object Selected { get; set; }

        [DisplayName("Status")]
        [StringLength(150)]
        [Required]
        public object Status { get; set; }
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
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required]
        public object UpchargeAmount { get; set; }

        [DisplayName("Upcharge Total Cost")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public object UpchargeTotalCost { get; set; }

        [DisplayName("Status")]
        [StringLength(150)]
        [Required]
        public object UpchargeStatus { get; set; }
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
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required]
        public object UpchargeSellPriceFinalAmount { get; set; }

        [DisplayName("Upcharge Sell Price Locked?")]
        public object UpchargeSellPriceLocked { get; set; }

        [DisplayName("Status")]
        [StringLength(150)]
        [Required]
        public object UpchargeSellPriceStatus { get; set; }

        [DisplayName("Upcharge Sell Price Final Rounded Amount")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public object UpchargeSellPriceFinalRoundedAmount { get; set; }
    }

    public class AttachmentTypeMetadata
    {
        [DisplayName("Status")]
        [StringLength(150)]
        [Required]
        public object Status { get; set; }

        [DisplayName("Type Name")]
        [StringLength(150)]
        [Required]
        public object TypeName { get; set; }
    }

    public class VendorNameMetadata
    {
        [DisplayName("Vendor Name")]
        [StringLength(150)]
        [Required]
        public object VendorNameName { get; set; }

        [DisplayName("Status")]
        [StringLength(150)]
        [Required]
        public object VendorNameStatus { get; set; }
    }

    public class VendorTypeMetadata
    {
        [DisplayName("Vendor Type")]
        [StringLength(150)]
        [Required]
        public object VendorTypeName { get; set; }

        [DisplayName("Status")]
        [StringLength(150)]
        [Required]
        public object VendorTypeStatus { get; set; }
    }
}