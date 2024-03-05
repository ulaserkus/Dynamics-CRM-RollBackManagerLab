using Identity.Framework.Data.Model;

namespace Identity.Framework.Core.Services.JWT
{
    public interface IJwtService
    {
        string GenerateToken(User user, int expirationMinutes = 60);
    }
}
