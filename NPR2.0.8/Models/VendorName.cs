//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NPRModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class VendorName
    {
        public VendorName()
        {
            this.AuditTrails = new HashSet<AuditTrail>();
            this.Products = new HashSet<Product>();
        }
    
        public int VendorNameID { get; set; }
        public string VendorNameName { get; set; }
        public string VendorNameStatus { get; set; }
    
        public virtual ICollection<AuditTrail> AuditTrails { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
