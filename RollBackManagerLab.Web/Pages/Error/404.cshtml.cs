using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RollBackManagerLab.Web.Pages.Error
{
    public class _404Model : PageModel
    {
        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            await Task.CompletedTask;
        }
    }
}
