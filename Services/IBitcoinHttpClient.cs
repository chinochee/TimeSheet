using Services.Dtos;

namespace Services
{
    public interface IBitcoinHttpClient
    {
        Task<RatesDto> GetRates();
    }
}