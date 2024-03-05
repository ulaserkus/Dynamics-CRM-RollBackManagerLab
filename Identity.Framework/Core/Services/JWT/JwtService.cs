using Identity.Framework.Data.Model;
using Microsoft.IdentityModel.Tokens;
using Shared.Contants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Framework.Core.Services.JWT
{
    public class JwtService : IJwtService
    {
        public JwtService()
        {
        }
        public string GenerateToken(User user, int expirationMinutes = 60)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.FirstName),
                new Claim(ClaimTypes.Surname,user.LastName),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim("isActive",user.IsActiveUser.ToString()),
                new Claim("hasActiveSubscription",user.HasActiveSubscription.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSecureConstants.JwtSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: AppSecureConstants.JwtIssuer,
                audience: AppSecureConstants.JwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptedToken = tokenHandler.WriteToken(token);

            return encryptedToken;
        }
    }
}
