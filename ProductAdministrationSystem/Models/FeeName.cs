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
    
    public partial class FeeName
    {
        public FeeName()
        {
            this.AuditTrails = new HashSet<AuditTrail>();
            this.Fees = new HashSet<Fee>();
        }
    
        public int FeeNameID { get; set; }
        public string FeeNameName { get; set; }
        public string FeeNameType { get; set; }
        public string FeeNameStatus { get; set; }
    
        public virtual ICollection<AuditTrail> AuditTrails { get; set; }
        public virtual ICollection<Fee> Fees { get; set; }
    }
}
