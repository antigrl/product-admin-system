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
    
    public partial class ProductAttachmentType
    {
        public ProductAttachmentType()
        {
            this.AuditTrails = new HashSet<AuditTrail>();
            this.AuditTrails1 = new HashSet<AuditTrail>();
        }
    
        public int ID { get; set; }
        public string Status { get; set; }
        public bool Selected { get; set; }
        public int ProductID { get; set; }
        public int AttachmentTypeID { get; set; }
    
        public virtual AttachmentType AttachmentType { get; set; }
        public virtual ICollection<AuditTrail> AuditTrails { get; set; }
        public virtual ICollection<AuditTrail> AuditTrails1 { get; set; }
    }
}
