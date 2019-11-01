using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using buysharps.Models.BuysharpsModel.Mapping;

namespace buysharps.Models.BuysharpsModel.Context
{

    public partial class BuySharpsContext : DbContext
    {
        public BuySharpsContext()
            : base("name=BuySharpsContext")
        {
        }

        /// <summary>
        /// This is Configuration the tables
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ImageEntityConfiguration());
            modelBuilder.Configurations.Add(new VariantEntityConfiguration());
            modelBuilder.Configurations.Add(new ProductEntityConfiguration());
            modelBuilder.Configurations.Add(new OrderEntityConfiguration());
            modelBuilder.Configurations.Add(new CustomerEntityConfiguration());
        }
    }
}
