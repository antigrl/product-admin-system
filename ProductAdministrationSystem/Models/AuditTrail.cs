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
    
    public partial class AuditTrail
    {
        public int AuditTrailID { get; set; }
        public System.DateTime AuditTrailTimeStamp { get; set; }
        public string AuditTrailUserName { get; set; }
        public string AuditTrailComment { get; set; }
        public Nullable<int> CampaignID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> DecorationMethodID { get; set; }
        public Nullable<int> FeeID { get; set; }
        public Nullable<int> FeeNameID { get; set; }
        public Nullable<int> PackagingTypeID { get; set; }
        public Nullable<int> PricingTierID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> DecorationID { get; set; }
        public Nullable<int> SellPriceID { get; set; }
        public Nullable<int> UpchargeID { get; set; }
        public Nullable<int> VendorNameID { get; set; }
        public Nullable<int> VendorTypeID { get; set; }
        public Nullable<int> UpchargeSellPriceID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> MajorCategoryOrderingID { get; set; }
    
        public virtual AuditTrail AuditTrail1 { get; set; }
        public virtual AuditTrail AuditTrail2 { get; set; }
        public virtual Campaign Campaign { get; set; }
        public virtual Category Category { get; set; }
        public virtual Company Company { get; set; }
        public virtual DecorationMethod DecorationMethod { get; set; }
        public virtual Fee Fee { get; set; }
        public virtual FeeName FeeName { get; set; }
        public virtual PackagingType PackagingType { get; set; }
        public virtual PricingTier PricingTier { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductDecoration ProductDecoration { get; set; }
        public virtual ProductSellPrice ProductSellPrice { get; set; }
        public virtual ProductUpcharge ProductUpcharge { get; set; }
        public virtual UpchargeSellPrice UpchargeSellPrice { get; set; }
        public virtual VendorName VendorName { get; set; }
        public virtual VendorType VendorType { get; set; }
        public virtual MajorCategoryOrdering MajorCategoryOrdering { get; set; }
    }
}
