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
    
    public partial class Category
    {
        public Category()
        {
            this.AuditTrails = new HashSet<AuditTrail>();
            this.Products = new HashSet<Product>();
            this.Products1 = new HashSet<Product>();
        }
    
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryType { get; set; }
        public string CategoryStatus { get; set; }
    
        public virtual ICollection<AuditTrail> AuditTrails { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Product> Products1 { get; set; }
    }
}
