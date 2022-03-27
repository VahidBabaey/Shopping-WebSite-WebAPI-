using Microsoft.AspNetCore.Builder;
using Reddington.Core.Extenstions;
using Reddington.Core.Infrastructure;
using Reddington.Core.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reddington.Framework.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ConfigureRequestPipeline(this IApplicationBuilder application)
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
                item.Configure(application);
            }
            var ListTaskTypes = typeof(ITaskSchduler).GetAllClassTypes();
            foreach (var typeTask in ListTaskTypes)
            {
                var task = application.ApplicationServices.GetService(typeTask) as ITaskSchduler;
                if (task.IsActiveInStartup)
                    task.ExecuteTask();
            }
        }
    }

}
