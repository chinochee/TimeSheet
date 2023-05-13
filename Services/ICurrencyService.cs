using Data.Entities;
using Services.Dtos;

namespace Services
{
    public interface ICurrencyService
    {
        Task<CurrencyEntryDto[]> Get();
        Task Save(IEnumerable<Currency> currency);
    }
}