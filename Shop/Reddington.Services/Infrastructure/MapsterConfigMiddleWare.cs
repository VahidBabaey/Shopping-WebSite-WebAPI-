using Reddington.Services.Catalog;
using Mapster;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reddington.Service.Infrastructure
{
    public class MapsterConfigMiddleWare
    {
        private readonly RequestDelegate _next;
       
        public MapsterConfigMiddleWare(RequestDelegate next)
        {
            _next = next;
            
        }

        public async Task InvokeAsync(HttpContext context)
        {
            
            TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);
            TypeAdapterConfig.GlobalSettings.Default.Ignore("Deleted");
            TypeAdapterConfig.GlobalSettings.Default.Ignore("Timestamp");
            TypeAdapterConfig.GlobalSettings.Default.AddDestinationTransform((int? x) => x ?? 0);
            //TypeAdapterConfig.GlobalSettings.Default.EnableNonPublicMembers(true);
            await _next(context);
        }
    }

}
