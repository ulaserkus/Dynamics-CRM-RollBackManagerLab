using Shared.Config.Abstract;

namespace Shared.Config.Concrete
{
    public class AuthenticationConfig : IAuthenticationConfig
    {
        public string ApiName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
