using buysharps.Models.BuysharpsModel.Context;
using buysharps.Models.BuysharpsModel.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace buysharps.Models.BuysharpsModel.UnitOfWork
{
    /// <summary>
    /// 
    /// </summary>
    public class GetRepositories
    {
        private BuySharpsContext BuySharpsContext = new BuySharpsContext();

        public Repository<Customer> CustomerRepository => new Repository<Customer>(BuySharpsContext);
        public Repository<DraftOrder> OrderRepository => new Repository<DraftOrder>(BuySharpsContext);
        public Repository<Product> ProductRepository => new Repository<Product>(BuySharpsContext);

        #region Disposing

        bool disposed = false;
        void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    BuySharpsContext.Dispose();
                }
            }
            disposed = true;
        }

        void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}