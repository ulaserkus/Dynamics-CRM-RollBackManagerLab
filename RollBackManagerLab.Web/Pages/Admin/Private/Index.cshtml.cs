using Identity.Framework.Data.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RollBackManagerLab.Web.Constants;
using Shared.Api.Abstract;
using System.Security.Claims;
using System.Text.Json;

namespace RollBackManagerLab.Web.Pages.Admin.Private
{
    public class IndexModel : PageModel
    {
        private readonly IIdentityApiHandler _identityApiHandler;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public IReadOnlyList<ApiKey> ApiKeys { get; set; }
        public IndexModel(IIdentityApiHandler identityApiHandler)
        {
            _identityApiHandler = identityApiHandler;
        }
        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            ApiKeys = await GetUserKeys();
        }
        public async Task<IReadOnlyList<ApiKey>> GetUserKeys()
        {
            var user = HttpContext.User;

            if (user == null)
                return new List<ApiKey>().AsReadOnly();

            string userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            var apiResult = await _identityApiHandler.GetUserKeysByIdAsync(userId);


            if (apiResult == null || apiResult.StatusCode != StatusCodes.Status200OK)
            {
                return new List<ApiKey>().AsReadOnly();
            }

            var keysJson = JsonSerializer.Serialize(apiResult.Result, _jsonSerializerOptions);
            var keys = JsonSerializer.Deserialize<List<ApiKey>>(keysJson, _jsonSerializerOptions);

            if (keys == null)
                return new List<ApiKey>().AsReadOnly();

            return keys.AsReadOnly();
        }

        public async Task<IActionResult> OnPostSignOutAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return RedirectToPage();

            await HttpContext.SignOutAsync();
            return RedirectToPage(PageRedirectConstants.HomePage);
        }

        public IActionResult OnDeleteKey()
        {
            // Delete key process
            return Page();
        }
    }
}
