using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RollBackManagerLab.Web.Constants;
using RollBackManagerLab.Web.Model;
using RollBackManagerLab.Web.Utilities.Email;
using RollBackManagerLab.Web.Utilities.General;
using Shared.Api.DTOs;
using Shared.Config.Abstract;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace RollBackManagerLab.Web.Pages.Admin
{
    public class RegisterModel : PageModel
    {
        private readonly ISmtpConfig _smtpConfig;
        private readonly Regex _emailRegex = new Regex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$");
        private readonly Regex _passwordRegex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*\\W).{8,}$");

        [BindProperty]
        public CaptchaModel Captcha { get; set; } = default!;

        [BindProperty]
        public string EnteredCaptcha { get; set; } = string.Empty;

        [BindProperty]
        public AddUserRequestApiDto UserRequestApiDto { get; set; } = default!;

        public RegisterModel(ISmtpConfig smtpConfig)
        {
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

            var validationResult = ValidateSignInForm(cancellationToken);

            if (!validationResult)
            {
                SetPageDefaults();
                return Page();
            }

            var otpMail = new FakeOtpEmail(_smtpConfig);
            await otpMail.SendAsync(UserRequestApiDto.EmailAddress, cancellationToken);
            TempData[SessionContants.RegisterOtpCode] = otpMail.Code;
            TempData[SessionContants.UserRegisterRequest] = JsonSerializer.Serialize(UserRequestApiDto);

            return RedirectToPage(PageRedirectConstants.AdminVerifyRegisterPage);
        }
        public void SetPageDefaults()
        {
            Captcha = CaptchaUtil.GenerateCaptcha();
            UserRequestApiDto = new();
            EnteredCaptcha = string.Empty;
        }
        public bool ValidateSignInForm(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return false;

            if (string.IsNullOrEmpty(UserRequestApiDto.EmailAddress) || string.IsNullOrEmpty(UserRequestApiDto.Password))
            {
                ModelState.AddModelError(ErrorMessageContants.Key, ErrorMessageContants.EmailAndPasswordEmptyErrorMessage);
                return false;
            }

            if (string.IsNullOrEmpty(EnteredCaptcha))
            {
                ModelState.AddModelError(ErrorMessageContants.Key, ErrorMessageContants.EmptyCaptchaErrorMessage);
                return false;
            }

            if (!_emailRegex.IsMatch(UserRequestApiDto.EmailAddress))
            {
                ModelState.AddModelError(ErrorMessageContants.Key, ErrorMessageContants.EmailRegexNotValideErrorMessage);
                return false;
            }

            if (!Captcha.ValidateCaptcha(EnteredCaptcha))
            {
                ModelState.AddModelError(ErrorMessageContants.Key, ErrorMessageContants.CaptchaIsNotExpectedValueErrorMessage);
                return false;
            }

            if (!_passwordRegex.IsMatch(UserRequestApiDto.Password))
            {
                ModelState.AddModelError(ErrorMessageContants.Key, ErrorMessageContants.PasswordRuleErrorMessage);
                return false;
            }

            return true;
        }

    }
}
