using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CoinFill.Helpers.RouteMiddlewares
{
    public class IgnoreRouteMiddleware
    {
        private readonly RequestDelegate _next;

        public IgnoreRouteMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.HasValue &&
                context.Request.Path.Value.ToLower().Contains("base"))
            {
                context.Response.StatusCode = 404;
                return;
            }

            await _next.Invoke(context);
        }
    }
}
