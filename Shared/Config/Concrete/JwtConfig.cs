using Shared.Config.Abstract;

namespace Shared.Config.Concrete
{
    public class JwtConfig : IJwtConfig
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
