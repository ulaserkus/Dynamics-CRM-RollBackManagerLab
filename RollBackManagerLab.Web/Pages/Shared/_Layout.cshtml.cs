using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RollBackManagerLab.Web.Pages.Shared
{
    public class _LayoutModel : PageModel
    {
        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            await Task.CompletedTask;
        }
    }
}
