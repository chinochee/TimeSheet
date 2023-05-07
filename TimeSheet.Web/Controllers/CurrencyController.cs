using Microsoft.AspNetCore.Mvc;
using Services;

namespace TimeSheet.Web.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly ILogger<CurrencyController> _logger;
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ILogger<CurrencyController> logger, ICurrencyService currencyService)
        {
            _logger = logger;
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<IActionResult> Currencies()
        {
            var currencies = await _currencyService.Get();
            return View(currencies);
        }
    }
}