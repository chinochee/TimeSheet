using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Helpers;

namespace TimeSheet.Web.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly ILogger<CurrencyController> _logger;
        private readonly ICurrencyService _currencyService;
        private readonly ICurrencyExportService _currencyExportService;
        private readonly ICurrencyImportService _currencyImportService;

        public CurrencyController(ILogger<CurrencyController> logger, ICurrencyService currencyService, ICurrencyExportService currencyExportService, ICurrencyImportService currencyImportService)
        {
            _logger = logger;
            _currencyService = currencyService;
            _currencyExportService = currencyExportService;
            _currencyImportService = currencyImportService;
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> Currencies()
        {
            var currencies = await _currencyService.Get();
            return View(currencies);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> OnGetCurrencies()
        {
            var currencies = await _currencyExportService.Export();
            return File(currencies.AsMemoryStream().ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExchangeRates.xlsx");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> OnPostCurrencies(IFormFile uploadedFile)
        {
            if (uploadedFile == null)
                return RedirectToAction(nameof(Currencies));

            using (var stream = new MemoryStream())
            {
                await uploadedFile.CopyToAsync(stream);
                await _currencyImportService.Import(stream);
            }

            return RedirectToAction(nameof(Currencies));
        }
    }
}