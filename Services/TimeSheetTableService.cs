using Services.Dtos;

namespace Services
{
    public class TimeSheetTableService : ITimeSheetTableService
    {
        public Task<TimeSheetEntryDto[]> GetEntries(TimeSheetFiltersDto filter)
        {
            throw new NotImplementedException();
        }
    }
}
