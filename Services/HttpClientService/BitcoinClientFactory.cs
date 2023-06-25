using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Configuration;

namespace Services.HttpClientService
{
    public class BitcoinClientFactory : IBitcoinClientFactory
    {
        private readonly ILogger<BitcoinClientFactory> _logger;
        private readonly Dictionary<string, INamedBitcoinHttpClient> _clients;
        private readonly CacheSettings _cacheSettings;
        private readonly IMemoryCache _memoryCache;

        public BitcoinClientFactory(ILogger<BitcoinClientFactory> logger, IEnumerable<INamedBitcoinHttpClient> clients, IOptionsMonitor<CacheSettings> config, IMemoryCache memoryCache)
        {
            _clients = clients.ToDictionary(p => p.ApiHostName);
            _cacheSettings = config.CurrentValue;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public IBitcoinHttpClient GetClient()
        {
            try
            {
                var result = _clients[_cacheSettings.ApiHostName];
                return new CachedBtcHttpClient(result, _cacheSettings, _memoryCache);
            }
            catch (Exception ex)
            {
                _logger.LogError("LogError {0}", ex.Message);
            }

            return null;
        }
    }
}