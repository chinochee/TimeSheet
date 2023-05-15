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
        private readonly IBitcoinHttpClient _client;
        public ScopeTableService(IOptions<TableSettings> config, TimeSheetContext context, IBitcoinHttpClient client)
        {
            _tableSettings = config.Value;
            _context = context;
            _client = client;
        }

        public async Task<Dictionary<int, string>> GetDictionary() => await _context.Scopes.ToDictionaryAsync(s => s.Id, s => s.Name);

        public async Task<ScopeEntryDto[]> Get()
        {
            var coinDesk = await _client.GetRates();

            return await _context.Scopes.Select(s => new ScopeEntryDto
            {
                Id = s.Id,
                Name = s.Name,
                TotalPrice = Math.Round(s.Rate * s.TimeSheetList.Sum(timeSheet => timeSheet.WorkHours ?? 0), 2),
                NameCurrency = s.Currency.ShortName,
                TotalPriceUSD = Math.Round(s.Rate * s.TimeSheetList.Sum(timeSheet => timeSheet.WorkHours ?? 0) * s.Currency.DollarExchangeRate, 2),
                TotalPriceInBTC = Math.Round(s.Rate * s.TimeSheetList.Sum(timeSheet => timeSheet.WorkHours ?? 0) * s.Currency.DollarExchangeRate / coinDesk.bpi.USD.rate_float, 2)
            }).OrderByDescending(scope => scope.TotalPriceUSD)
            .Take(_tableSettings.TopScopes)
            .ToArrayAsync();
        }
    }
}