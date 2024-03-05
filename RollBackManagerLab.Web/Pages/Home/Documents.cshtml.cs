using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RollBackManagerLab.Web.Pages.Home
{
    public class DocumentsModel : PageModel
    {
        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            await Task.CompletedTask;
        }
    }
}
