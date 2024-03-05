using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Repository.Abstract;
using Shared.Utils;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;

namespace Shared.Authentication.Basic
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IBasicApiUserRepository _userRepository;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, IBasicApiUserRepository basicApiUserRepository, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _userRepository = basicApiUserRepository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Authorization header not found");
            }

            try
            {
                var authenticationHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authenticationHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);

                var username = credentials[0];
                var password = credentials[1];

                var apiUser = await _userRepository.FindOneAsync(x => x.UserName == username);

                if (apiUser == null)
                {
                    return AuthenticateResult.Fail("Invalid username or password");
                }

                if (HashUtil.VerifyHash(password, apiUser.HashedPassword, apiUser.Salt))
                {
                    var claims = new[] { new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, username) };
                    var identity = new System.Security.Claims.ClaimsIdentity(claims, Scheme.Name);
                    var principal = new System.Security.Claims.ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
                else

                {
                    return AuthenticateResult.Fail("Invalid username or password");

                }
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid authorization header");
            }
        }
    }
}
