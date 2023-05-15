using System.Text.Json;

namespace Services
{
    public class BitcoinHttpClient : IBitcoinHttpClient
    {
        public async Task<CoinDesk> GetRates()
        {

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.coindesk.com/v1/bpi/currentprice.json");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CoinDesk>(await response.Content.ReadAsStringAsync());
        }
    }

    public class CoinDesk
    {
        public Bpi bpi { get; set; }
    }

    public class Bpi
    {
        public BpiCurrency USD { get; set; }
        public BpiCurrency GBP { get; set; }
        public BpiCurrency EUR { get; set; }
    }

    public class BpiCurrency
    {
        public double rate_float { get; set; }
    }
}