namespace HttpServer.Middlewares
{
    using System.Net;
    using System.Threading.Tasks;

    using HttpServer.Services.Interfaces;

    using Microsoft.AspNetCore.Http;

    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IAuthService authService)
        {
            if (context.Request.Path.StartsWithSegments("/session/login") || context.Request.Path.Value.Contains("/image"))
            {
                await _next.Invoke(context);
                return;
            }

            string authHeader = context.Request.Headers["Authorization"];

            if (authHeader != null)
            {
                if (authService.CheckAuthorized(authHeader))
                {
                    await _next.Invoke(context);
                    return;
                }
            }

            //// Return authentication type (causes browser to show login dialog)
            //context.Response.Headers["WWW-Authenticate"] = "Token";

            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}