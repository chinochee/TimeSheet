using Services.Dtos;

namespace Services
{
    public interface ICurrencyService
    {
        Task<CurrencyEntryDto[]> Get();
    }
}