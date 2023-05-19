using Microsoft.Extensions.Logging;
using Services.Dtos;
using System.Net.Http.Json;

namespace Services.HttpClientService
{
    public class BlockchainInfoHttpClient : INamedBitcoinHttpClient
    {
        public string? ApiHostName => "Blockchain";

        private readonly ILogger<BlockchainInfoHttpClient> _logger;
        private readonly HttpClient _httpClient;

        public BlockchainInfoHttpClient(ILogger<BlockchainInfoHttpClient> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri("https://blockchain.info/ticker");
        }

        public async Task<RatesDto> GetRates()
        {
            _logger.LogInformation("Request rates from Blockchain API");
            var result = await _httpClient.GetFromJsonAsync<BlockchainInfoDto>("");
            _logger.LogInformation("Request rates from Blockchain API Finished");

            return new RatesDto
            {
                Rate = result.USD.sell
            };
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