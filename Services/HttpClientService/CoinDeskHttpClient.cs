using Microsoft.Extensions.Logging;
using Services.Dtos;
using System.Net.Http.Json;

namespace Services.HttpClientService
{
    public class CoinDeskHttpClient : INamedBitcoinHttpClient
    {
        public string ApiHostName => "CoinDesk";

        private readonly ILogger<CoinDeskHttpClient> _logger;
        private readonly HttpClient _httpClient;

        public CoinDeskHttpClient(ILogger<CoinDeskHttpClient> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri("https://api.coindesk.com/v1/bpi/currentprice.json");
        }

        public async Task<RatesDto> GetRates()
        {
            _logger.LogInformation("Request rates from CoinDesk API");

            var result = await _httpClient.GetFromJsonAsync<CoinDesk>("");

            _logger.LogInformation("Request rates from CoinDesk API Finished");

            return new RatesDto
            {
                Rate = result.bpi.USD.rate_float
            };
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