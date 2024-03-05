using Microsoft.AspNetCore.Builder;
using Shared.Middleware;

namespace Shared.Extension
{
    public static class ApplicationBuilderExtension
    {

        public static IApplicationBuilder UseCustomExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
        public static IApplicationBuilder UseCustomAuthentication(this IApplicationBuilder app)
        {
            return app.UseMiddleware<TokenValidationMiddleware>();
        }
        public static IApplicationBuilder UseLocalHostIpFilter(this IApplicationBuilder app)
        {
            var allowedIp = "::1";
            return app.UseMiddleware<IPFilterMiddleware>(allowedIp);
        }
    }
}
