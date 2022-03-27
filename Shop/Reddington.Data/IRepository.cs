using Reddington.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reddington.Data
{
    public partial interface IRepository<TEntity> where TEntity : Entity
    {
        TEntity GetByID(params object[] ids);
        TEntity GetByIDNoTracking(params object[] ids);

        void Insert(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<TEntity> GetByIDAsync(params object[] ids);
        Task<TEntity> GetByIDNoTrackingAsync(params object[] ids);
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);

        IQueryable<TEntity> Table { get; }

        IQueryable<TEntity> TableNoTracking { get; }


    }
}
