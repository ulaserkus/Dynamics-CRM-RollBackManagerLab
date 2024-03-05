using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RollBackManagerLab.Web.Constants;
using Shared.Model.Concrete;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace RollBackManagerLab.Web.Pages.Admin
{
    public class VerifySignInModel : PageModel
    {
        [BindProperty]
        public string EnteredText { get; set; } = string.Empty;

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return RedirectToPage();

            var code = TempData[SessionContants.SignInOtpCode]?.ToString() ?? "";

            var validationResult = await ValidateVerifySignInFormAsync(code);

            if (!validationResult)
            {
                return Page();
            }

            return RedirectToPage(PageRedirectConstants.AdminIndexPage);
        }

        public async Task<bool> ValidateVerifySignInFormAsync(string otpCode)
        {
            if (string.IsNullOrEmpty(otpCode))
            {
                ModelState.AddModelError(ErrorMessageContants.Key, ErrorMessageContants.NotFoundOtpCode);
                return false;
            }

            if (!EnteredText.Equals(otpCode, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(ErrorMessageContants.Key, ErrorMessageContants.InvalidOtpCode);
                return false;
            }

            var signInResponse = TempData[SessionContants.UserSignInResponse]?.ToString() ?? "";

            if (string.IsNullOrEmpty(signInResponse))
            {
                ModelState.AddModelError(ErrorMessageContants.Key, ErrorMessageContants.NotFoundOtpCode);
                return false;
            }

            ApiResult? apiResult = JsonSerializer.Deserialize<ApiResult>(signInResponse);

            if (apiResult == null || apiResult.Result == null)
            {
                ModelState.AddModelError(ErrorMessageContants.Key, ErrorMessageContants.NotFoundOtpCode);
                return false;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(apiResult.Result.ToString()) as JwtSecurityToken;
            var claimsIdentity = new ClaimsIdentity(token?.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return true;
        }
    }
}
