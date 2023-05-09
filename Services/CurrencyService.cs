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

        public void Post(Currency[] currencyEntryDto)
        {
            var addCurrencyList = new List<Currency>();
            foreach (var currency in currencyEntryDto)
            {
                if (_context.Currencies.Any(c => c.FullName == currency.FullName))
                {
                    var editCurrency = _context.Currencies.FirstOrDefault(c => c.FullName == currency.FullName);
                    _context.Currencies.Remove(editCurrency);

                    editCurrency.ShortName = currency.ShortName;
                    editCurrency.DollarExchangeRate = currency.DollarExchangeRate;
                    addCurrencyList.Add(editCurrency);
                }
                else
                {
                    addCurrencyList.Add(currency);
                }
            }

            _context.SaveChanges();
            _context.Currencies.AddRange(addCurrencyList.ToArray());
            _context.SaveChanges();
        }
    }
}