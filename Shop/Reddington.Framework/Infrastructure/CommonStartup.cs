using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Reddington.Core.Infrastructure;
using Reddington.Service.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Reddington.Framework.Infrastructure
{
    public class CommonStartup : IApplicationStartup
    {
        public MiddleWarePriority Priority => MiddleWarePriority.Normal;

        public void Configure(IApplicationBuilder app)
        {
           // IWebHostEnvironment env = app.ApplicationServices.GetService(typeof(IWebHostEnvironment)) as IWebHostEnvironment;


            //app.UseHttpsRedirection();
            app.UseMiddleware<PoweredByMiddleware>();


            app.UseSwagger();


            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop API V1");
            });



        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IErrorHandler, ErrorHandler>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "ورژن یک",
                    Title = " وب سرویس فروشگاه",
                    Description = " در این وب سرویس مشخصات فروشگاه شرح داده می شود",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Reddington Team",
                        Email = string.Empty,
                        
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        
                    }
                });
            var xmlFile = $"Shop.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
            });

        }
    }
}
