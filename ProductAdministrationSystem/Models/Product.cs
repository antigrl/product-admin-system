//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PAS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public Product()
        {
            this.AuditTrails = new HashSet<AuditTrail>();
            this.Fees = new HashSet<Fee>();
            this.ProductDecorations = new HashSet<ProductDecoration>();
            this.ProductSellPrices = new HashSet<ProductSellPrice>();
            this.ProductUpcharges = new HashSet<ProductUpcharge>();
            this.ProductDocuments = new HashSet<ProductDocument>();
            this.ProductAttachmentTypes = new HashSet<ProductAttachmentType>();
        }
    
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public byte[] ProductImage { get; set; }
        public string ProductImageType { get; set; }
        public string ProductDescription { get; set; }
        public string ProductStatus { get; set; }
        public string ProductUnitOfMeasure { get; set; }
        public int PackagingTypeID { get; set; }
        public string ProductReplacementItemNumber { get; set; }
        public string ProductSelectedSizes { get; set; }
        public string ProductSelectedColors { get; set; }
        public Nullable<decimal> ProductPresentationSell { get; set; }
        public string ProductQuoteNumber { get; set; }
        public string ProductSalesHistory { get; set; }
        public int VendorNameID { get; set; }
        public Nullable<int> VendorTypeID { get; set; }
        public string ProductVendorLeadTime { get; set; }
        public decimal ProductVendorMinimumOrder { get; set; }
        public string ProductCountryOfOrigin { get; set; }
        public string ProductHarmonizedCode { get; set; }
        public Nullable<decimal> ProductInitialOrderQuantity { get; set; }
        public string ProductVendorItemNumber { get; set; }
        public Nullable<decimal> ProductAnnualSalesProjection { get; set; }
        public Nullable<decimal> ProductGatewayCDIMinumumOrder { get; set; }
        public Nullable<decimal> ProductCost { get; set; }
        public Nullable<decimal> ProductNetCost { get; set; }
        public Nullable<decimal> ProductTotalCost { get; set; }
        public Nullable<decimal> ProductLandedCost { get; set; }
        public string ProductGatewayCDISKU { get; set; }
        public string ProductItemEDP { get; set; }
        public string ProductAccountManagerComments { get; set; }
        public string ProductMerchandiserComments { get; set; }
        public string ProductInventoryBuyerComments { get; set; }
        public string ProductMentorComments { get; set; }
        public int CampaignID { get; set; }
        public int MajorCategoryID { get; set; }
        public Nullable<int> MinorCategoryID { get; set; }
        public Nullable<int> ProductSortValue { get; set; }
    
        public virtual ICollection<AuditTrail> AuditTrails { get; set; }
        public virtual Campaign Campaign { get; set; }
        public virtual Category Category { get; set; }
        public virtual Category Category1 { get; set; }
        public virtual ICollection<Fee> Fees { get; set; }
        public virtual PackagingType PackagingType { get; set; }
        public virtual VendorName VendorName { get; set; }
        public virtual VendorType VendorType { get; set; }
        public virtual ICollection<ProductDecoration> ProductDecorations { get; set; }
        public virtual ICollection<ProductSellPrice> ProductSellPrices { get; set; }
        public virtual ICollection<ProductUpcharge> ProductUpcharges { get; set; }
        public virtual ICollection<ProductDocument> ProductDocuments { get; set; }
        public virtual ICollection<ProductAttachmentType> ProductAttachmentTypes { get; set; }
    }
}
