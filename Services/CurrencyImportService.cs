using ClosedXML.Excel;
using Data.Entities;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class CurrencyImportService : ICurrencyImportService
    {
        private readonly ILogger<CurrencyImportService> _logger;
        private readonly ICurrencyService _currencyService;
        public CurrencyImportService(ILogger<CurrencyImportService> logger, ICurrencyService currencyService)
        {
            _logger = logger;
            _currencyService = currencyService;
        }

        public async Task Import(Stream stream)
        {
            var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.Worksheet(1);

            var currencies = new List<Currency>();
            try
            {
                for (var i = 0; i < worksheet.Rows().Count() - 1; i++)
                {
                    var shortNameCell = worksheet.Cell(i + 2, 2).Value.ToString();
                    if (string.IsNullOrEmpty(shortNameCell)) break;

                    var currency = new Currency
                    {
                        ShortName = shortNameCell,
                        FullName = worksheet.Cell(i + 2, 1).Value.ToString(),
                        DollarExchangeRate = Convert.ToDouble(worksheet.Cell(i + 2, 3).Value.ToString())
                    };

                    currencies.Add(currency);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("LogError {0}", ex.Message);
            }

            await _currencyService.Save(currencies);
        }
    }
}