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
    
    public partial class DecorationMethod
    {
        public DecorationMethod()
        {
            this.AuditTrails = new HashSet<AuditTrail>();
            this.ProductDecorations = new HashSet<ProductDecoration>();
        }
    
        public int DecorationMethodID { get; set; }
        public string DecorationMethodName { get; set; }
        public string DecorationMethodStatus { get; set; }
    
        public virtual ICollection<AuditTrail> AuditTrails { get; set; }
        public virtual ICollection<ProductDecoration> ProductDecorations { get; set; }
    }
}
