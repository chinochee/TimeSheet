using Data.Entities;
using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Dtos;

namespace Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ILogger<CurrencyService> _logger;
        private readonly TimeSheetContext _context;
        public CurrencyService(ILogger<CurrencyService> logger, TimeSheetContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CurrencyEntryDto[]> Get()
        {
            try
            {
                var result = await _context.Currencies.Select(c => new CurrencyEntryDto
                    {
                        Id = c.Id,
                        ShortName = c.ShortName,
                        FullName = c.FullName,
                        DollarExchangeRate = c.DollarExchangeRate
                    })
                    .ToArrayAsync();
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("LogError {0}", ex.Message);
            }

            return null;
        }

        public async Task Save(IEnumerable<Currency> currencyEntryDto)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError("LogError {0}", ex.Message);
            }
        }
    }
}