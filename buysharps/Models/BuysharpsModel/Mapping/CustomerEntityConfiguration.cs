using buysharps.Models.BuysharpsModel.Tables;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace buysharps.Models.BuysharpsModel.Mapping
{
    public class CustomerEntityConfiguration : EntityTypeConfiguration<Customer>
    {
        public CustomerEntityConfiguration()
        {
            // Primary Key
            this.HasKey<long>(p => p.Id);

            // Table & Column Mappings
            this.ToTable("customer");
            this.Property(p => p.Id).HasColumnName("customer_id");
            this.Property(p => p.first_name).HasColumnName("customer_firstname");
            this.Property(p => p.last_name).HasColumnName("customer_lastname");
            this.Property(p => p.email).HasColumnName("customer_email");
            this.Property(p => p.created_at).HasColumnName("customer_created");
            this.Property(p => p.note).HasColumnName("customer_note");
            this.Property(p => p.orders_count).HasColumnName("customer_orderscount");
            this.Property(p => p.state).HasColumnName("customer_state");
            this.Property(p => p.phone).HasColumnName("customer_phone");

        }
    }
}