//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PASModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class Company
    {
        public Company()
        {
            this.AuditTrails = new HashSet<AuditTrail>();
            this.Campaigns = new HashSet<Campaign>();
            this.Fees = new HashSet<Fee>();
            this.PricingTiers = new HashSet<PricingTier>();
        }
    
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDivisionNumber { get; set; }
        public byte[] CompanyImage { get; set; }
        public string CompanyImageType { get; set; }
        public string CompanyContactName { get; set; }
        public string CompanyContactEmail { get; set; }
        public string CompanyAccountManagerName { get; set; }
        public string CompanyAccountManagerEmail { get; set; }
        public string CompanyMentorName { get; set; }
        public string CompanyMentorEmail { get; set; }
        public string CompanyLocation { get; set; }
        public string CompanyAdditionalDetails { get; set; }
        public string CompanyStatus { get; set; }
        public string CompanyUrl { get; set; }
        public decimal CompanyDefaultMargin { get; set; }
    
        public virtual ICollection<AuditTrail> AuditTrails { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<Fee> Fees { get; set; }
        public virtual ICollection<PricingTier> PricingTiers { get; set; }
    }
}