using Microsoft.EntityFrameworkCore;
using Reddington.Core;
using Reddington.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reddington.Data
{
    public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        #region Fields
        private readonly IApplcationDbContext _context = null;
        #endregion

        public EFRepository(IApplcationDbContext Context)
        {
            this._context = Context;
        }
        public IQueryable<TEntity> Table => this._context.Set<TEntity>();

        public IQueryable<TEntity> TableNoTracking => this._context.Set<TEntity>().AsNoTracking();

        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            this._context.Set<TEntity>().Remove(entity);
            this._context.SaveChanges();
        }
        public async virtual Task DeleteAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            this._context.Set<TEntity>().Remove(entity);
           await this._context.SaveChangesAsync();
        }

        public virtual TEntity GetByID(params object[] ids)
        {
            return _context.Set<TEntity>().Find(ids);
        }
        public async virtual Task<TEntity> GetByIDAsync(params object[] ids)
        {
             return await _context.Set<TEntity>().FindAsync(ids);
        }
        public virtual TEntity GetByIDNoTracking(params object[] ids)
        {
            var entity = _context.Set<TEntity>().Find(ids);
            if(entity != null)
            {
                this._context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }
        public async virtual Task<TEntity> GetByIDNoTrackingAsync(params object[] ids)
        {
            var entity = await _context.Set<TEntity>().FindAsync(ids);
            if (entity != null)
            {
                this._context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public virtual void Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            this._context.Set<TEntity>().Add(entity);
            this._context.SaveChanges();
        }
        public async virtual Task InsertAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            await this._context.Set<TEntity>().AddAsync(entity);
            await this._context.SaveChangesAsync();
        }

        public virtual void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            this._context.Set<TEntity>().Update(entity);
            this._context.SaveChanges();
        }
        public async virtual Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            this._context.Set<TEntity>().Update(entity);
            await this._context.SaveChangesAsync();
        }
        public List<T> RunFunctionDb<T>(string functionName,List<DbParamter> paramters) where T : new()
        {
            return _context.RunSp<T>(functionName, paramters);
        }
    }
}
