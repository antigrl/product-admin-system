﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<DecorationMethod> DecorationMethods { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<FeeName> FeeNames { get; set; }
        public DbSet<PackagingType> PackagingTypes { get; set; }
        public DbSet<PricingTier> PricingTiers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDecoration> ProductDecorations { get; set; }
        public DbSet<ProductSellPrice> ProductSellPrices { get; set; }
        public DbSet<ProductUpcharge> ProductUpcharges { get; set; }
        public DbSet<UpchargeSellPrice> UpchargeSellPrices { get; set; }
        public DbSet<VendorName> VendorNames { get; set; }
        public DbSet<VendorType> VendorTypes { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }
    }
}
