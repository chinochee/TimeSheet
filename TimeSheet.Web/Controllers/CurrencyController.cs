using Microsoft.AspNetCore.Mvc;
using Services;

namespace TimeSheet.Web.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly ILogger<CurrencyController> _logger;
        private readonly ICurrencyService _currencyService;
        private readonly ITableSheetExportService _exportService;

        public CurrencyController(ILogger<CurrencyController> logger, ICurrencyService currencyService, ITableSheetExportService exportService)
        {
            _logger = logger;
            _currencyService = currencyService;
            _exportService = exportService;
        }

        [HttpGet]
        public async Task<IActionResult> Currencies()
        {
            var currencies = await _currencyService.Get();
            return View(currencies);
        }

        [HttpGet]
        public async Task<IActionResult> OnGetCurrencies()
        {
            var currencies = await _exportService.GetCurrencyStreamXlsx();

            return File(currencies.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExchangeRates.xlsx");
        }
    }
}