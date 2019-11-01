using buysharps.Models.BuysharpsModel.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace buysharps.Models.BuysharpsModel.UnitOfWork
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    interface IRepository<TEntity>
    {
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        IEnumerable<TEntity> GetAll();
        TEntity FindById(long Id);
        void RemoveById(long id);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        DbContext _dbContext;
        DbSet<TEntity> entities;
        public Repository(BuySharpsContext buySharpsContext)
        {
            _dbContext = buySharpsContext;
            entities = buySharpsContext.Set<TEntity>();
        }
        public void Create(TEntity entity)
        {
            entities.Add(entity);
            _dbContext.SaveChanges();
        }  
        
        public void AddRange(IEnumerable<TEntity> entity)
        {
            entities.AddRange(entity);
            _dbContext.SaveChanges();
        }

        public TEntity FindById(long Id)
        {
            return entities.Find(Id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return entities.ToList();
        }

        public void Remove(TEntity entity)
        {
            entities.Remove(entity);
            _dbContext.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            _dbContext.Entry<TEntity>(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void RemoveById(long id)
        {
            var obj = entities.Find(id);
            entities.Remove(obj);
            _dbContext.SaveChanges();
        }
    }
}