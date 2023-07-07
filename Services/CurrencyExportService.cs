using ClosedXML.Excel;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class CurrencyExportService : ICurrencyExportService
    {
        private readonly ILogger<CurrencyExportService> _logger;
        private readonly ICurrencyService _currencyService;
        public CurrencyExportService(ILogger<CurrencyExportService> logger, ICurrencyService currencyService)
        {
            _currencyService = currencyService;
            _logger = logger;
        }

        public async Task<XLWorkbook> Export()
        {
            try
            {
                var currencies = await _currencyService.Get();

                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Exchange rates");
                worksheet.Cell("A1").Value = "Full name";
                worksheet.Cell("B1").Value = "Short name";
                worksheet.Cell("C1").Value = "Dollar exchange rate";

                for (var i = 0; i < currencies.Length; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = currencies[i].FullName;
                    worksheet.Cell(i + 2, 2).Value = currencies[i].ShortName;
                    worksheet.Cell(i + 2, 3).Value = currencies[i].DollarExchangeRate;
                }

                return workbook;
            }
            catch (Exception ex)
            {
                _logger.LogError("LogError {0}", ex.Message);
            }

            return null;
        }
    }
}