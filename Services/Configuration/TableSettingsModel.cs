using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Services.Configuration
{
    public class TableSettingsModel : PageModel
    {
        private readonly TableSettings _settings;
        public TableSettingsModel(IOptions<TableSettings> settings)
        {
            _settings = settings.Value;
        }
        public ContentResult OnGet()
        {
            return Content($"PageSize: {_settings.PageSize}");
        }
    }
}
