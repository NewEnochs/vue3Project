using Microsoft.AspNetCore.Authorization;
using ProjectCore.Web.Services;

namespace ProjectCore.Web.Middlewares
{
    public class BearerTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public BearerTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AuthTokenCache tokenCache)
        {
            if (ShouldSkip(context))
            {
                await _next(context);
                return;
            }

            var authorization = context.Request.Headers.Authorization.ToString();
            if (!authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                await WriteUnauthorizedAsync(context, "请携带Bearer token");
                return;
            }

            var token = authorization["Bearer ".Length..].Trim();
            if (!tokenCache.TryGetLoginUser(token, out var loginUser))
            {
                await WriteUnauthorizedAsync(context, "token无效或已过期");
                return;
            }

            context.Items["LoginUser"] = loginUser;
            await _next(context);
        }

        private static bool ShouldSkip(HttpContext context)
        {
            var path = context.Request.Path;
            var endpoint = context.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null;

            return allowAnonymous
                   || path.StartsWithSegments("/swagger")
                   || path.Equals("/login", StringComparison.OrdinalIgnoreCase)
                   || HttpMethods.IsOptions(context.Request.Method);
        }

        private static async Task WriteUnauthorizedAsync(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "text/plain; charset=utf-8";
            await context.Response.WriteAsync(message);
        }
    }
}