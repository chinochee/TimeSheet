using Services.Dtos;

namespace Services
{
    public interface IBitcoinHttpClient
    {
        string? ApiHostName { get; }
        public Task<RatesDto> GetRates();
    }
}