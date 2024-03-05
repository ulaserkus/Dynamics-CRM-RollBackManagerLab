using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Shared.Contants;
using Shared.CustomAttribute;
using Shared.Exception;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Shared.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.GetEndpoint()?.Metadata.GetMetadata<SkipTokenValidationAttribute>() != null)
            {
                await _next(context);
                return;
            }

            var encryptedToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(encryptedToken))
            {
                throw new UnAuthorizedException();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSecureConstants.JwtSecret));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = AppSecureConstants.JwtIssuer,
                ValidAudience = AppSecureConstants.JwtAudience,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero
            };
            var principal = tokenHandler.ValidateToken(encryptedToken, tokenValidationParameters, out _);
            context.User = principal;
            await _next(context);
        }
    }
}
