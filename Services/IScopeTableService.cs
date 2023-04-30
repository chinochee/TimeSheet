using Services.Dtos;

namespace Services
{
    public interface IScopeTableService
    {
        Task<ScopeEntryDto[]> GetEntries();
    }
}