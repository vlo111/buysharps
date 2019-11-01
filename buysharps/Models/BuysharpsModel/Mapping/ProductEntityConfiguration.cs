using buysharps.Models.BuysharpsModel.Tables;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace buysharps.Models.BuysharpsModel.Mapping
{
    public class ProductEntityConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductEntityConfiguration()
        {

            // Primary Key
            this.HasKey<long>(p => p.Id);

            // Table & Column Mappings
            this.ToTable("product");
            this.Property(p => p.Id).HasColumnName("prod_id");
            this.Property(p => p.Title).HasColumnName("prod_title");
            this.Property(p => p.body_html).HasColumnName("prod_desc");
            this.Property(p => p.created_at).HasColumnName("prod_created");

            // Relationships
            this.HasMany(p => p.Variants)
                .WithRequired(p => p.Product)
                .HasForeignKey(p => p.ProductId);

            this.HasRequired(p => p.Image)
                .WithOptional(p => p.Product);
        }

    }
}