using Newtonsoft.Json.Linq;

namespace Services
{
    public static class ApiCalls
    {
        public static async Task<double> GetRateBTC()
        {

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.coindesk.com/v1/bpi/currentprice.json");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseJObject = JObject.Parse(await response.Content.ReadAsStringAsync());

            return Convert.ToDouble(responseJObject["bpi"]["USD"]["rate_float"]);
        }
    }
}