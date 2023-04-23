using Services.Dtos;

namespace Services
{
    public interface IScopeTableService
    {
        Task<ScopeTableDto> GetEntries(ScopeFiltersDto filter);
    }
}