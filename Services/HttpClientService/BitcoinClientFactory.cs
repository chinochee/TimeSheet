using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Services.Configuration;

namespace Services.HttpClientService
{
    public class BitcoinClientFactory : IBitcoinClientFactory
    {
        private readonly Dictionary<string, INamedBitcoinHttpClient> _clients;
        private readonly CacheSettings _cacheSettings;
        private readonly IMemoryCache _memoryCache;

        public BitcoinClientFactory(IEnumerable<INamedBitcoinHttpClient> clients, IOptionsMonitor<CacheSettings> config, IMemoryCache memoryCache)
        {
            _clients = clients.ToDictionary(p => p.ApiHostName);
            _cacheSettings = config.CurrentValue;
            _memoryCache = memoryCache;
        }

        public IBitcoinHttpClient GetClient()
        {
            var result = _clients[_cacheSettings.ApiHostName];
            return new CachedBtcHttpClient(result, _cacheSettings, _memoryCache);
        }
    }
}