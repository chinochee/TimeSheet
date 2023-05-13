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

        public async Task<CurrencyEntryDto[]> Get() => await _context.Currencies.Select(c => new CurrencyEntryDto
            {
                Id = c.Id,
                ShortName = c.ShortName,
                FullName = c.FullName,
                DollarExchangeRate = c.DollarExchangeRate
            })
            .ToArrayAsync();

        public async Task Save(IEnumerable<Currency> currencyEntryDto)
        {
            var currencies = await _context.Currencies.ToDictionaryAsync(currency => currency.ShortName);

            foreach (var currency in currencyEntryDto)
            {
                if (currencies.TryGetValue(currency.ShortName, out var currencyD))
                {
                    currencyD.FullName = currency.FullName;
                    currencyD.DollarExchangeRate = currency.DollarExchangeRate;
                    _context.Currencies.Update(currencyD);
                }
                else
                {
                    await _context.Currencies.AddAsync(currency);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}