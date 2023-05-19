﻿using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Configuration;
using Services.Dtos;

namespace Services.HttpClientService
{
    public class BlockchainInfoHttpClient : IBitcoinHttpClient
    {
        public string? ApiHostName => "Blockchain";

        private readonly ILogger<BlockchainInfoHttpClient> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly HttpClient _httpClient;
        private readonly CacheSettings _cacheSettings;

        public BlockchainInfoHttpClient(ILogger<BlockchainInfoHttpClient> logger, IOptionsMonitor<CacheSettings> config, IMemoryCache memoryCache, HttpClient httpClient)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _httpClient = httpClient;
            _cacheSettings = config.CurrentValue;

            _httpClient.BaseAddress = new Uri("https://blockchain.info/ticker");
        }

        public async Task<RatesDto> GetRates()
        {
            var ratesDto = new RatesDto();

            if (!_memoryCache.TryGetValue("rate", out RatesDto cacheValue))
            {
                _logger.LogInformation("Request rates from Blockchain API");

                var result = await _httpClient.GetFromJsonAsync<BlockchainInfoDto>("");
                ratesDto.Rate = result.USD.sell;

                _memoryCache.Set("rate", ratesDto, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheSettings.SecondsHoldCache)));

                _logger.LogInformation("Request rates from Blockchain API Finished");
            }
            else
            {
                ratesDto = cacheValue;
            }

            return ratesDto;
        }

        private class BlockchainInfoDto
        {
            public BlockchainInfoRate USD { get; set; }
        }

        private class BlockchainInfoRate
        {
            public double sell { get; set; }
        }
    }
}