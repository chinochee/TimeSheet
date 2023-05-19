using Services.Dtos;

namespace Services
{
    public interface IBitcoinHttpClient
    {
        public Task<RatesDto> GetRates();
    }

    public interface INamedBitcoinHttpClient : IBitcoinHttpClient
    {
        string? ApiHostName { get; }
    }
}