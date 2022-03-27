using Reddington.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reddington.Services.Catalog;
using Reddington.Framework.Infrastructure;
using Reddington.Service.Infrastructure;
using Reddington.Framework.Infrastructure.Extensions;
using System.Web.Http.ModelBinding;
using Reddington.Framework.Infrastructure.ModelBinding;
using Reddington.Framwork.Infrastructure.ModelBinders;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Reddington.Framework.Infrastructure.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace Shop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            services.AddScoped<LogFilterAttribute>();

            services.AddMemoryCache();
            services.AddScoped<StockQuantityTask>();
            services.ConfigureApplicationServices(Configuration);
            IMvcBuilder configController = services.AddControllers(option => {
                //option.Filters.Add(typeof(LogFilterAttribute));
                option.ModelBinderProviders.Insert(0, new PersianDateEntityBinderProvider());
            });
            configController.ConfigureApiBehaviorOptions((option) =>
            {

                option.ClientErrorMapping[404].Title = " منبع مورد نظر پیدا نشد ";

                option.InvalidModelStateResponseFactory = (Context) =>
                {

                    var values = Context.ModelState.Values.Where(state => state.Errors.Count != 0)
                  .Select(state => state.Errors.Select(p => new { errorMessage = p.ErrorMessage }));

                    return new BadRequestObjectResult(values);

                };
            });
            services.AddAuthentication("Bearer")
             .AddJwtBearer("Bearer", options =>
             {
                 options.Authority = "https://localhost:6001";
                 options.RequireHttpsMetadata = false;

                 options.Audience = "ShopApi";

                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     NameClaimType = "name",

                 };

             });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(app =>
                {
                    app.UseMiddleware<ErrorHandlerMiddleware>();
                });
                app.UseHsts();
            }
            //app.UseHttpsRedirection();

            app.ConfigureRequestPipeline();
            app.UseRouting();
            //app.UseDefaultFiles();
            //app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //    Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
            //    RequestPath = "/StaticFiles"
            //});

            //app.UseAuthorization();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
