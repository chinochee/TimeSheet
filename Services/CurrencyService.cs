using Data.Entities;
using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Services.Dtos;

namespace Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly TimeSheetContext _context;
        public CurrencyService(TimeSheetContext context)
        {
            _context = context;
        }

        public Task<CurrencyEntryDto[]> Get() => _context.Currencies.Select(c => new CurrencyEntryDto
            {
                Id = c.Id,
                ShortName = c.ShortName,
                FullName = c.FullName,
                DollarExchangeRate = c.DollarExchangeRate
            })
            .ToArrayAsync();

        public void Add(Currency[] currencyEntryDto)
        {
            var currencies = _context.Currencies;

            foreach (var currency in currencyEntryDto)
            {
                if (currencies.Any(c => c.FullName == currency.FullName))
                {
                    var editCurrency = currencies.FirstOrDefault(c => c.FullName == currency.FullName);

                    editCurrency.ShortName = currency.ShortName;
                    editCurrency.DollarExchangeRate = currency.DollarExchangeRate;
                    _context.Currencies.Update(editCurrency);
                }
                else
                {
                    _context.Currencies.Add(currency);
                }
            }

            _context.SaveChanges();
        }
    }
}