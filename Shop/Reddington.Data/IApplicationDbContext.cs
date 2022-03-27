using Reddington.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;
using System.Threading;

namespace Reddington.Data
{
    public interface IApplcationDbContext
    {

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry Entry(object entity);
        List<T> RunSp<T>(string StoreName, List<DbParamter> ListParamert) where T : new();
    }

}
