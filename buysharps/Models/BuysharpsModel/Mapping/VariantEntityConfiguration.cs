using buysharps.Models.BuysharpsModel.Tables;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace buysharps.Models.BuysharpsModel.Mapping
{
    public class VariantEntityConfiguration : EntityTypeConfiguration<Variant>
    {
        public VariantEntityConfiguration()
        {

            // Primary Key
            this.HasKey<long>(p => p.Id);

            // Table & Column Mappings
            this.ToTable("variant");
            this.Property(p => p.Id).HasColumnName("variant_id");
            this.Property(p => p.Price).HasColumnName("variant_price");
            this.Property(p => p.ProductId).HasColumnName("variant_productid");

        }
    }
}