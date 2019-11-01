using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using buysharps.Models.BuysharpsModel.Tables;

namespace buysharps.Models.BuysharpsModel.Mapping
{
    public class ImageEntityConfiguration : EntityTypeConfiguration<Image>
    {
        public ImageEntityConfiguration()
        {
            // Primary Key
            this.HasKey<long>(p => p.Id);

            // Table & Column Mappings
            this.ToTable("image");
            this.Property(p => p.Id).HasColumnName("customer_id");
            this.Property(p => p.ProductId).HasColumnName("customer_productid");
            this.Property(p => p.Height).HasColumnName("image_height");
            this.Property(p => p.Width).HasColumnName("image_width");
            this.Property(p => p.Src).HasColumnName("image_src");
            this.Property(p => p.Alt).HasColumnName("image_alt");

        }
    }
}