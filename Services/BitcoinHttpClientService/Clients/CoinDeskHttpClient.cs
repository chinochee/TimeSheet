using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Configuration;
using Services.Dtos;

namespace Services.BitcoinHttpClientService.Clients
{
    public class CoinDeskHttpClient : BitcoinHttpClient
    {
        public override string? ApiHostName => "CoinDesk";

        private readonly ILogger<CoinDeskHttpClient> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly HttpClient _httpClient;
        private readonly CacheSettings _cacheSettings;

        public CoinDeskHttpClient(ILogger<CoinDeskHttpClient> logger, IOptionsMonitor<CacheSettings> config, IMemoryCache memoryCache, HttpClient httpClient)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _httpClient = httpClient;
            _cacheSettings = config.CurrentValue;

            _httpClient.BaseAddress = new Uri("https://api.coindesk.com/v1/bpi/currentprice.json");
        }

        protected override async Task<RatesDto> GetIfExists()
        {
            var ratesDto = new RatesDto();
            
            if (!_memoryCache.TryGetValue("rate", out RatesDto cacheValue))
            {
                _logger.LogInformation("Request rates from CoinDesk API");

                var result = await _httpClient.GetFromJsonAsync<CoinDesk>("");
                ratesDto.Rate = result.bpi.USD.rate_float;
                ratesDto.update_at = DateTime.UtcNow;

                _memoryCache.Set("rate", ratesDto, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheSettings.SecondsHoldCache)));

                _logger.LogInformation("Request rates from CoinDesk API Finished");
            }
            else
            {
                ratesDto = cacheValue;
            }

            return ratesDto;
        }

        private class CoinDesk
        {
            public Bpi bpi { get; set; }
        }

        private class Bpi
        {
            public BpiCurrency USD { get; set; }
        }

        private class BpiCurrency
        {
            public double rate_float { get; set; }
        }
    }
}