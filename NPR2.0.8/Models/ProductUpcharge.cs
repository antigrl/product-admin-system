//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NPR2._0._8.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductUpcharge
    {
        public ProductUpcharge()
        {
            this.UpchargeSellPrices = new HashSet<UpchargeSellPrice>();
            this.AuditTrails = new HashSet<AuditTrail>();
        }
    
        public int UpchargeID { get; set; }
        public string UpchargeName { get; set; }
        public decimal UpchargeAmount { get; set; }
        public Nullable<decimal> UpchargeTotalCost { get; set; }
        public Nullable<int> ProductID { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual ICollection<UpchargeSellPrice> UpchargeSellPrices { get; set; }
        public virtual ICollection<AuditTrail> AuditTrails { get; set; }
    }
}
