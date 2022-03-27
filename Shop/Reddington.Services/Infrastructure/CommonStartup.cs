using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reddington.Core.Infrastructure;
using Reddington.Service.Infrastructure;
using Reddington.Service.Media;
using Reddington.Services.Catalog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reddington.Services.Infrastructure
{
    public class CommonStartup : IApplicationStartup
    {
        public MiddleWarePriority Priority => MiddleWarePriority.Normal;

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<MapsterConfigMiddleWare>();
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPictureService, PictureService>();
        }
    }
}
