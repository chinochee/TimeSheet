using ClosedXML.Excel;
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

        [HttpGet]
        public async Task<IActionResult> OnGetCurrencies()
        {
            var currencies = await _currencyService.Get();

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Exchange rates");
            worksheet.Cell("A1").Value = "Full name";
            worksheet.Cell("B1").Value = "Short name";
            worksheet.Cell("C1").Value = "Dollar exchange rate";

            using (MemoryStream stream = new MemoryStream())
            {
                for (var i = 0; i < currencies.Length; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = currencies[i].FullName;
                    worksheet.Cell(i + 2, 2).Value = currencies[i].ShortName;
                    worksheet.Cell(i + 2, 3).Value = currencies[i].DollarExchangeRate;
                }
                workbook.SaveAs(stream);

                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExchangeRates.xlsx");
            }
        }
    }
}