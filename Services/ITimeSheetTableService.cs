using Services.Dtos;

namespace Services
{
    public interface ITimeSheetTableService
    {
        Task<TimeSheetEntryDto[]> GetEntries(TimeSheetFiltersDto filter, int page);
        Task<int> GetEntriesCount(TimeSheetFiltersDto filter);
    }
}
