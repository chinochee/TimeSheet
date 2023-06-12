using Services.Dtos;

namespace Services
{
    public interface IScopeTableService
    {
        Task<ScopeEntryBTCDto[]> Get();
        Task<Dictionary<int, string?>> GetDictionary();
    }
}