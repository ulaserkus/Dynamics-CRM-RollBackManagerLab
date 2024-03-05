using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RollBackManagerLab.Web.Constants;
using RollBackManagerLab.Web.Model;
using RollBackManagerLab.Web.Utilities.Email;
using RollBackManagerLab.Web.Utilities.General;
using Shared.Api.Abstract;
using Shared.Api.DTOs;
using Shared.Config.Abstract;
using Shared.Model.Concrete;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace RollBackManagerLab.Web.Pages.Admin
{
    public class SignInModel : PageModel
    {
        private readonly IIdentityApiHandler _identityApiHandler;
        private readonly ISmtpConfig _smtpConfig;
        private readonly Regex _emailRegex = new Regex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$");

        [BindProperty]
        public CaptchaModel Captcha { get; set; } = default!;

        [BindProperty]
        public LoginApiDto LoginApiDto { get; set; } = default!;

        [BindProperty]
        public string EnteredCaptcha { get; set; } = string.Empty;

        public SignInModel(IIdentityApiHandler identityApiHandler, ISmtpConfig smtpConfig)
        {
            _identityApiHandler = identityApiHandler;
            _smtpConfig = smtpConfig;
        }
        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;
            SetPageDefaults();
            await Task.CompletedTask;
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return RedirectToPage();

            var validationResult = await ValidateSignInFormAsync(cancellationToken);

            if (!validationResult)
            {
                SetPageDefaults();
                return Page();
            }

            var otpMail = new FakeOtpEmail(_smtpConfig);
            await otpMail.SendAsync(LoginApiDto.EmailAddress, cancellationToken);
            TempData[SessionContants.SignInOtpCode] = otpMail.Code;

            return RedirectToPage(PageRedirectConstants.AdminVerifySignInPage);
        }
        public void SetPageDefaults()
        {
            Captcha = CaptchaUtil.GenerateCaptcha();
            LoginApiDto = new();
            EnteredCaptcha = string.Empty;
        }
        public async Task<bool> ValidateSignInFormAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return false;

            if (string.IsNullOrEmpty(LoginApiDto.EmailAddress) || string.IsNullOrEmpty(LoginApiDto.Password))
            {
                ModelState.AddModelError(ErrorMessageContants.Key, ErrorMessageContants.EmailAndPasswordEmptyErrorMessage);
                return false;
            }

            if (string.IsNullOrEmpty(EnteredCaptcha))
            {
                ModelState.AddModelError(ErrorMessageContants.Key, ErrorMessageContants.EmptyCaptchaErrorMessage);
                return false;
            }

            if (!_emailRegex.IsMatch(LoginApiDto.EmailAddress))
            {
                ModelState.AddModelError(ErrorMessageContants.Key, ErrorMessageContants.EmailRegexNotValideErrorMessage);
                return false;
            }

            if (!Captcha.ValidateCaptcha(EnteredCaptcha))
            {
                ModelState.AddModelError(ErrorMessageContants.Key, ErrorMessageContants.CaptchaIsNotExpectedValueErrorMessage);
                return false;
            }

            ApiResult? apiResult = await _identityApiHandler.LoginAsync(LoginApiDto) as ApiResult;

            if (apiResult == null || apiResult.StatusCode != StatusCodes.Status200OK)
            {
                ModelState.AddModelError(ErrorMessageContants.Key, apiResult?.ErrorMessage ?? ErrorMessageContants.InvalidAuthErrorMessage);
                return false;
            }

            TempData[SessionContants.UserSignInResponse] = JsonSerializer.Serialize(apiResult);
            return true;
        }
    }
}
