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
        private readonly ILogger<ScopeTableService> _logger;
        private readonly TableSettings _tableSettings;
        private readonly TimeSheetContext _context;
        private readonly IBitcoinClientFactory _clientFactory;
        public ScopeTableService(ILogger<ScopeTableService> logger, IOptions<TableSettings> config, TimeSheetContext context, IBitcoinClientFactory bitcoinClientFactory)
        {
            _tableSettings = config.Value;
            _context = context;
            _clientFactory = bitcoinClientFactory;
            _logger = logger;
        }

        public async Task<Dictionary<int, string?>> GetDictionary()
        {
            try
            {
                var result =
                    await _context.Scopes.ToDictionaryAsync(s => s.Id, s => s.Name);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("LogError {0}", ex.Message);
            }

            return null;
        }

        public async Task<ScopeEntryBTCDto[]> Get()
        {
            try
            {
                var coinDeskTask = _clientFactory.GetClient().GetRates();
                var topScopesTask = GetAnnualTopUSD();

                await Task.WhenAll(coinDeskTask, topScopesTask);

                var coinDesk = await coinDeskTask;
                var topScopes = await topScopesTask;

                return topScopes.Select(s => new ScopeEntryBTCDto(s, coinDesk.Rate)).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError("LogError {0}", ex.Message);
            }

            return null;
        }

        private async Task<ScopeEntryDto[]> GetAnnualTopUSD()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError("LogError {0}", ex.Message);
            }

            return null;
        }
    }
}