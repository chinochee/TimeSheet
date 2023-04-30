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

        public Task<ScopeEntryDto[]> GetEntries()
        {
            return _context.TimeSheets.GroupBy(s => new
                {
                    s.Scope.Id,
                    s.Scope.Name,
                    s.Scope.Rate,
                    s.Scope.Currency.DollarExchangeRate,
                    s.Scope.Currency.ShortName
                })
                .Select(s => new ScopeEntryDto
                {
                    Id = s.Key.Id,
                    Name = s.Key.Name,
                    TotalPrice = Math.Round(s.Sum(timeSheet => timeSheet.WorkHours ?? 0) * s.Key.Rate, 2),
                    TotalPriceUSD = Math.Round(s.Sum(timeSheet => timeSheet.WorkHours ?? 0) * s.Key.Rate * s.Key.DollarExchangeRate, 2),
                    NameCurrency = s.Key.ShortName
                })
                .OrderByDescending(scope => scope.TotalPriceUSD)
                .Take(_tableSettings.TopScope)
                .ToArrayAsync();
        }
    }
}