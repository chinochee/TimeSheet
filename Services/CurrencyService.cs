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

        public Task<CurrencyEntryDto[]> Get() => _context.Currencies.Select(c => new CurrencyEntryDto{ Id = c.Id, ShortName = c.ShortName, FullName = c.FullName, DollarExchangeRate = c.DollarExchangeRate}).ToArrayAsync();
    }
}