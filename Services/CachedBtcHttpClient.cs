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
        private static SemaphoreSlim _semaphore = new(1, 1);
        private const string _key = "rate";

        public CachedBtcHttpClient(IBitcoinHttpClient component, CacheSettings config, IMemoryCache memoryCache)
        {
            _component = component;
            _memoryCache = memoryCache;
            _cacheSettings = config;
        }

        public async Task<RatesDto> GetRates()
        {
            if (_memoryCache.TryGetValue(_key, out RatesDto cacheValue)) return cacheValue;

            await _semaphore.WaitAsync();

            try
            {
                if (_memoryCache.TryGetValue(_key, out cacheValue)) return cacheValue;

                cacheValue = await _component.GetRates();
                _memoryCache.Set(_key, cacheValue, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheSettings.SecondsHoldCache)));
                return cacheValue;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}