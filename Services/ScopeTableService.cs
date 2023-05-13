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

        public async Task<Dictionary<int, string>> GetDictionary() => await _context.Scopes.ToDictionaryAsync(s => s.Id, s => s.Name);

        public async Task<ScopeEntryDto[]> Get()
        {
            var rateBTC = await ApiCalls.GetRateBTC();

            return await _context.Scopes.Select(s => new ScopeEntryDto
            {
                Id = s.Id,
                Name = s.Name,
                TotalPrice = Math.Round(s.Rate * s.TimeSheetList.Sum(timeSheet => timeSheet.WorkHours ?? 0), 2),
                NameCurrency = s.Currency.ShortName,
                TotalPriceUSD = Math.Round(s.Rate * s.TimeSheetList.Sum(timeSheet => timeSheet.WorkHours ?? 0) * s.Currency.DollarExchangeRate, 2),
                TotalPriceInBTC = Math.Round(s.Rate * s.TimeSheetList.Sum(timeSheet => timeSheet.WorkHours ?? 0) * s.Currency.DollarExchangeRate / rateBTC, 2)
            }).OrderByDescending(scope => scope.TotalPriceUSD)
            .Take(_tableSettings.TopScopes)
            .ToArrayAsync();
        }
    }
}