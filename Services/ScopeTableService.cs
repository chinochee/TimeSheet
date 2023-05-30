using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.BitcoinHttpClientService;
using Services.Configuration;
using Services.Dtos;

namespace Services
{
    public class ScopeTableService : IScopeTableService
    {
        private readonly TableSettings _tableSettings;
        private readonly TimeSheetContext _context;
        private readonly IBitcoinClientFactory _clientFactory;
        private readonly ILogger<ScopeTableService> _logger;
        public ScopeTableService(ILogger<ScopeTableService> logger, IOptions<TableSettings> config, TimeSheetContext context, IBitcoinClientFactory bitcoinClientFactory)
        {
            _tableSettings = config.Value;
            _context = context;
            _clientFactory = bitcoinClientFactory;
            _logger = logger;
        }

        public async Task<Dictionary<int, string>> GetDictionary() => await _context.Scopes.ToDictionaryAsync(s => s.Id, s => s.Name);

        public async Task<ScopeEntryBTCDto[]> Get()
        {
            var coinDeskTask = _clientFactory.GetClient().GetRates();
            var topScopesTask = GetAnnualTopUSD();

            await Task.WhenAll(coinDeskTask, topScopesTask);

            var coinDesk = await coinDeskTask;
            var topScopes = await topScopesTask;

            return topScopes.Select(s => new ScopeEntryBTCDto(s, coinDesk.Rate)).ToArray();
        }

        private async Task<ScopeEntryDto[]> GetAnnualTopUSD()
        {
            _logger.LogInformation("Get top scopes from db");

            var result = await _context.Scopes.Select(s => new ScopeEntryDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    TotalPrice = s.Rate * s.TimeSheetList.Sum(timeSheet => timeSheet.WorkHours ?? 0),
                    NameCurrency = s.Currency.ShortName,
                    TotalPriceUSD = s.Rate * s.TimeSheetList.Sum(timeSheet => timeSheet.WorkHours ?? 0) * s.Currency.DollarExchangeRate
                }).OrderByDescending(scope => scope.TotalPriceUSD)
                .Take(_tableSettings.TopScopes)
                .ToArrayAsync();

            _logger.LogInformation("Get top scopes from db Finished");

            return result;
        }
    }
}