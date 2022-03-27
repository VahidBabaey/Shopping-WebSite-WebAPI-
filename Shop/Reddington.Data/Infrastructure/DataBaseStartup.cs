using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reddington.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reddington.Data.Infrastructure
{
    public class DataBaseStartup : IApplicationStartup
    {
        public MiddleWarePriority Priority => MiddleWarePriority.AboveNormal;

        public void Configure(IApplicationBuilder app)
        {

        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));

            services.AddDbContextPool<IApplcationDbContext, SqlServerApplicationContext>(
            c => c.UseSqlServer("Data Source=.;Initial Catalog=Shop;Integrated Security=true;").UseLazyLoadingProxies()

            , poolSize: 16); ;
        }
    }
}
