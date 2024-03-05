using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RollBackManagerLab.Web.Constants;
using Shared.Api.Abstract;
using Shared.Api.DTOs;
using Shared.Model.Concrete;
using System.Text.Json;

namespace RollBackManagerLab.Web.Pages.Admin
{
    public class VerifyRegisterModel : PageModel
    {
        [BindProperty]
        public string EnteredText { get; set; } = string.Empty;

        private readonly IIdentityApiHandler _identityApiHandler;
        public VerifyRegisterModel(IIdentityApiHandler identityApiHandler)
        {
            _identityApiHandler = identityApiHandler;
        }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;
            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return RedirectToPage();

            var registerOtpCode = TempData[SessionContants.RegisterOtpCode]?.ToString() ?? "";
            var userRequestJson = TempData[SessionContants.UserRegisterRequest]?.ToString() ?? "";

            var validationResult = await ValidateVerifyRegisterFormAsync(registerOtpCode, userRequestJson, cancellationToken);

            if (!validationResult)
            {
                return Page();
            }

            return RedirectToPage(PageRedirectConstants.SignInPage);
        }

        public async Task<bool> ValidateVerifyRegisterFormAsync(string otpCode, string userRequestJson, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return false;

            if (string.IsNullOrEmpty(otpCode))
            {
                ModelState.AddModelError(ErrorMessageContants.Key, ErrorMessageContants.NotFoundOtpCode);
                return false;
            }

            if (string.IsNullOrEmpty(userRequestJson))
            {
                ModelState.AddModelError(ErrorMessageContants.Key, ErrorMessageContants.NotFoundRegistration);
                return false;
            }

            if (!EnteredText.Equals(otpCode, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(ErrorMessageContants.Key, ErrorMessageContants.InvalidOtpCode);
                return false;
            }

            var userRequest = JsonSerializer.Deserialize<AddUserRequestApiDto>(userRequestJson);
            ApiResult? apiResult = await _identityApiHandler.AddUserWithAdminServiceAsync(userRequest) as ApiResult;

            if (apiResult == null || apiResult.StatusCode != StatusCodes.Status201Created)
            {
                ModelState.AddModelError(ErrorMessageContants.Key, apiResult?.ErrorMessage ?? ErrorMessageContants.RegistrationFailed);
                return false;
            }

            return true;
        }
    }
}
