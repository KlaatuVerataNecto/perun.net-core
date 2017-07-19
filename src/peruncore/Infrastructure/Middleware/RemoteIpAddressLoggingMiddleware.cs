using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Threading.Tasks;

namespace peruncore.Infrastructure.Middleware
{
    public class RemoteIpAddressLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RemoteIpAddressLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperty("Address", context.Connection.RemoteIpAddress))
            {
                await _next.Invoke(context);
            }
        }
    }
}
