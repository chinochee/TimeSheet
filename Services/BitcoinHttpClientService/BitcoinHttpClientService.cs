using Microsoft.Extensions.Options;
using Services.Configuration;

namespace Services.BitcoinHttpClientService
{
    public class BitcoinHttpClientService : IBitcoinHttpClientService
    {
        private readonly Dictionary<string, IBitcoinHttpClient> _clients;
        private readonly CacheSettings _cacheSettings;

        public BitcoinHttpClientService(IEnumerable<IBitcoinHttpClient> clients, IOptionsMonitor<CacheSettings> config)
        {
            _clients = clients.ToDictionary(p => p.ApiHostName);
            _cacheSettings = config.CurrentValue;
        }

        public IBitcoinHttpClient GetClient()
        {
            return _clients[_cacheSettings.ApiHostName];
        }
    }
}