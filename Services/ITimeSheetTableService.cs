using Services.Dtos;

namespace Services
{
    public interface ITimeSheetTableService
    {
        Task<TimeSheetEntryDto[]> GetEntries(TimeSheetFiltersDto filter);
    }
}
