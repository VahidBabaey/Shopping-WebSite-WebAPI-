using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reddington.Core.Extenstions;
using Reddington.Core.Infrastructure;
using Reddington.Core.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reddington.Framework.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var listtypes = typeof(IApplicationStartup).GetAllClassTypes();
            List<IApplicationStartup> ListObject = new List<IApplicationStartup>();
            foreach (var TypeItem in listtypes)
            {
                var ob = Activator.CreateInstance(TypeItem) as IApplicationStartup;
                ListObject.Add(ob);
            }
            foreach (var item in ListObject)
            {
                item.ConfigureServices(services, configuration);
            }
        var ListTaskTypes = typeof(ITaskSchduler).GetAllClassTypes();
            foreach (var typeTask in ListTaskTypes)
            {
                services.AddScoped(typeTask);
            }
        }
    }

}
