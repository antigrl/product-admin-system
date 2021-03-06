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
    
    public partial class ProductDocument
    {
        public ProductDocument()
        {
            this.AuditTrails = new HashSet<AuditTrail>();
        }
    
        public int ID { get; set; }
        public byte[] Document { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public string Status { get; set; }
        public string DocumentFileType { get; set; }
        public int ProductID { get; set; }
        public int AttachmentTypeID { get; set; }
        public string DocumentFileName { get; set; }
    
        public virtual ICollection<AuditTrail> AuditTrails { get; set; }
        public virtual Product Product { get; set; }
        public virtual AttachmentType AttachmentType { get; set; }
    }
}
