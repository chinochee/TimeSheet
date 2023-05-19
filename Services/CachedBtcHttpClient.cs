using Microsoft.Extensions.Caching.Memory;
using Services.Configuration;
using Services.Dtos;

namespace Services
{
    public class CachedBtcHttpClient : IBitcoinHttpClient
    {
        private readonly IBitcoinHttpClient _component;
        private readonly IMemoryCache _memoryCache;
        private readonly CacheSettings _cacheSettings;

        public CachedBtcHttpClient(IBitcoinHttpClient component, CacheSettings config, IMemoryCache memoryCache)
        {
            _component = component;
            _memoryCache = memoryCache;
            _cacheSettings = config;
        }

        public async Task<RatesDto> GetRates()
        {
            if (!_memoryCache.TryGetValue("rate", out RatesDto cacheValue))
            {
                var rates = await _component.GetRates();
                _memoryCache.Set("rate", rates, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheSettings.SecondsHoldCache)));
                return rates;
            }

            return cacheValue;
        }
    }
}