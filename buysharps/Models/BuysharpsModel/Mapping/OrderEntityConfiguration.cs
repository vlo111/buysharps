using buysharps.Models.BuysharpsModel.Tables;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace buysharps.Models.BuysharpsModel.Mapping
{
    /// <summary>
    /// Configuration Order Tabel
    /// </summary>
    public class OrderEntityConfiguration : EntityTypeConfiguration<DraftOrder>
    {
        public OrderEntityConfiguration()
        {
            // Primary Key
            this.HasKey<long>(p => p.Id);

            // Table & Column Mappings
            this.ToTable("order");
            this.Property(p => p.Id).HasColumnName("order_id");
            this.Property(p => p.Name).HasColumnName("order_name");
            this.Property(p => p.Status).HasColumnName("order_status");
            this.Property(p => p.TotalPrice).HasColumnName("order_price");
            this.Property(p => p.Email).HasColumnName("order_email");
            this.Property(p => p.created_at).HasColumnName("order_created");
            this.Property(p => p.Note).HasColumnName("order_note");

        }
    }
}