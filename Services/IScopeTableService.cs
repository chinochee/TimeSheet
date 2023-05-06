using Services.Dtos;

namespace Services
{
    public interface IScopeTableService
    {
        Task<ScopeEntryDto[]> Get();
        Task<Dictionary<int, string>> GetDictionary();
    }
}