using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebApplication2.Middlewares
{
    public class EndpointLoggerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger _logger;

        public EndpointLoggerMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public EndpointLoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            Endpoint endpoint = context.GetEndpoint();
            var pathStartsWithApi = context.Request.Path.Value?.StartsWith("/api")  ?? false;
            if (endpoint != null || !pathStartsWithApi)
            {
                if (endpoint != null)
                {
                    // получаем шаблон маршрута, который ассоциирован с конечной точкой
                    var routePattern = (endpoint as Microsoft.AspNetCore.Routing.RouteEndpoint)?.RoutePattern?.RawText;

                    _logger.LogInformation($"route: {routePattern}");
                }
                else
                {
                    var route = context.Request.Path.Value;
                    var headerReferer = context.Request.Headers["Referer"].ToString();
                    var forReplace = context.Request.Scheme + "://" + context.Request.Host.ToString();
                    headerReferer = headerReferer.Replace(forReplace, "");
                    _logger.LogInformation($"route: {headerReferer}-- {route}");
                }


                // если конечная точка определена, передаем обработку дальше
                await _next.Invoke(context);
            }
            else
            {
                _logger.LogError($"Endpoint null");
                // если конечная точка не определена, завершаем обработку
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("Endpoint is not defined");
            }
        }
    }
}