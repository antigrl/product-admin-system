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
    
    public partial class UpchargeSellPrice
    {
        public UpchargeSellPrice()
        {
            this.AuditTrails = new HashSet<AuditTrail>();
        }
    
        public int UpchargeSellPriceID { get; set; }
        public string UpchargeSellPriceName { get; set; }
        public int UpchargeSellPriceLevel { get; set; }
        public decimal UpchargeSellPriceFinalAmount { get; set; }
        public bool UpchargeSellPriceLocked { get; set; }
        public int UpchargeID { get; set; }
        public string UpchargeSellPriceStatus { get; set; }
        public Nullable<decimal> UpchargeSellPriceFinalRoundedAmount { get; set; }
    
        public virtual ICollection<AuditTrail> AuditTrails { get; set; }
        public virtual ProductUpcharge ProductUpcharge { get; set; }
    }
}
