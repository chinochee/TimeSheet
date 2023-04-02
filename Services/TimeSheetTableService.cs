using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Services.Dtos;
using Services.Extensions;

namespace Services
{
    public class TimeSheetTableService : ITimeSheetTableService
    {
        private TimeSheetContext _context;
        public TimeSheetTableService(TimeSheetContext context)
        {
            _context = context;
        }

        public Task<TimeSheetEntryDto[]> GetEntries(TimeSheetFiltersDto filter)
        {
            var timeSheets = _context.TimeSheets
                .WhereIf(filter.DateOfWorksFrom.HasValue, s => s.DateOfWorks >= filter.DateOfWorksFrom)
                .WhereIf(filter.DateOfWorksTo.HasValue, s => s.DateOfWorks <= filter.DateOfWorksTo);

            var timeSheetsDto = timeSheets.Select(timeSheets => new TimeSheetEntryDto
            {
                Id = timeSheets.Id,
                Name = timeSheets.Name,
                Scope = timeSheets.Scope,
                WorkHours = timeSheets.WorkHours,
                DateOfWorks = timeSheets.DateOfWorks.ToShortDateString(),
                Comment = timeSheets.Comment
            });

            return timeSheetsDto.ToArrayAsync();
        }
    }
}