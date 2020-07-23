using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logging.Api.Middleware
{
    public class LogRequestHeader
    {
        private readonly RequestDelegate _next;

        public LogRequestHeader(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlation = context.Request.Headers["CorrelationId"];
            if (correlation.Count > 0)
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<LogRequestHeader>>();
                using (logger.BeginScope("{@CorrelationId}", correlation[0]))
                {
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
