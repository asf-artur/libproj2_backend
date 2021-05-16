using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebApplication2.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            await _next.Invoke(context);

            var errorString = context.Response.StatusCode switch
            {
                403 => "\nAccess Denied",
                404 => "\nNot Found",
                401 => "\nUnauthoried",
                _ => "",
            };

            if (context.Response.StatusCode >= 400)
            {
                _logger.LogError($"from {context.Connection.RemoteIpAddress} and {context.Request.Host} to {context.Request.Path}");
                // await context.Response.WriteAsync(errorString);
            }

        }
    }
}