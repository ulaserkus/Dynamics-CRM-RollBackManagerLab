using Microsoft.AspNetCore.Http;
using Shared.Exception;
using System.Net;

namespace Shared.Middleware
{
    public class IPFilterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _allowedIP;

        public IPFilterMiddleware(RequestDelegate next, string allowedIP)
        {
            _next = next;
            _allowedIP = allowedIP;
        }

        public async Task Invoke(HttpContext context)
        {
            var clientIP = context.Connection.RemoteIpAddress;

            // İzin verilen IP ile gelen isteği kontrol et
            if (!IsIPAllowed(clientIP, _allowedIP))
            {
                throw new ForbiddenIPException();
            }

            await _next(context);
        }

        private bool IsIPAllowed(IPAddress clientIP, string allowedIP)
        {
            return clientIP.ToString() == allowedIP;
        }
    }
}
