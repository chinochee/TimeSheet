using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Services.Configuration;
using Services.Dtos;

namespace Services
{
    public class ScopeTableService : IScopeTableService
    {
        private readonly TableSettings _tableSettings;
        private readonly TimeSheetContext _context;
        public ScopeTableService(IOptions<TableSettings> config, TimeSheetContext context)
        {
            _tableSettings = config.Value;
            _context = context;
        }

        public async Task<ScopeTableDto> GetEntries(ScopeFiltersDto filter)
        {
            var pageSize = _tableSettings.PageSize;

            var scopes = _context.TimeSheets
                .Select(timeSheets => new ScopeEntryDto
                {
                    Id = timeSheets.Scope.Id,
                    TotalPrice = Math.Round((double)(timeSheets.Scope.Rate * timeSheets.WorkHours), 2),
                    TotalPriceUSD = Math.Round((double)(Math.Round(timeSheets.Scope.Rate * timeSheets.Scope.Currency.DollarExchangeRate, 2) * timeSheets.WorkHours), 2)
                }).GroupBy(scope => scope.Id).Select(scope =>
                    new ScopeEntryDto
                    {
                        Id = scope.Key,
                        Name = _context.TimeSheets.First(s => s.Scope.Id == scope.Key).Scope.Name,
                        TotalPrice = scope.Sum(scope => scope.TotalPrice),
                        TotalPriceUSD = scope.Sum(scope => scope.TotalPriceUSD),
                        NameCurrency = _context.TimeSheets.First(s => s.Scope.Id == scope.Key).Scope.Currency.ShortName
                    })
                .OrderByDescending(scope => scope.TotalPriceUSD);

            var count = await scopes.CountAsync();

            var entries = await scopes.Skip((filter.PageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();
            
            return new ScopeTableDto
            {
                Entries = entries,
                Total = count,
                PageSize = pageSize
            };
        }
    }
}