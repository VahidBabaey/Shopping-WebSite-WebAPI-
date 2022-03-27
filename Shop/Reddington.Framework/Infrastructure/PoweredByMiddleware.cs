using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reddington.Framework.Infrastructure
{
    public class PoweredByMiddleware
    {
        private readonly RequestDelegate _next;
        public PoweredByMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            httpContext.Response.Headers["X-Powered-By"] = "Raymond Reddington";
            httpContext.Response.Headers["Server"] = "Raymond Reddington";
            return _next.Invoke(httpContext);
        }
    }

}
