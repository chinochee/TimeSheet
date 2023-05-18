using Services.Dtos;

namespace Services.BitcoinHttpClientService.Clients
{
    public abstract class BitcoinHttpClient : IBitcoinHttpClient
    {
        public abstract string? ApiHostName { get; }
        protected abstract Task<RatesDto> GetIfExists();
        public Task<RatesDto> GetRates()
        {
            return GetIfExists();
        }
    }
}