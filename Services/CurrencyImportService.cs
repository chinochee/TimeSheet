using ClosedXML.Excel;
using Data.Entities;

namespace Services
{
    public class CurrencyImportService : ICurrencyImportService
    {
        private readonly ICurrencyService _currencyService;
        public CurrencyImportService(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async void Import(Stream stream)
        {
            var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.Worksheet(1);

            var currencies = new List<Currency>();
            for (var i = 0; i < worksheet.Rows().Count() - 1; i++)
            {
                var currency = new Currency
                {
                    ShortName = worksheet.Cell(i + 2, 2).Value.ToString(),
                    FullName = worksheet.Cell(i + 2, 1).Value.ToString(),
                    DollarExchangeRate = Convert.ToDouble(worksheet.Cell(i + 2, 3).Value.ToString()) 
                };

                currencies.Add(currency);
            }

            _currencyService.Add(currencies.ToArray());
        }
    }
}