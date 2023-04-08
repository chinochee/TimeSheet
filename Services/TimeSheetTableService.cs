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

        public Task<TimeSheetEntryDto[]> GetEntries(TimeSheetFiltersDto filter, int page)
        {
            var timeSheets = _context.TimeSheets
                .WhereIf(filter.DateOfWorksFrom.HasValue, s => s.DateOfWorks >= filter.DateOfWorksFrom)
                .WhereIf(filter.DateOfWorksTo.HasValue, s => s.DateOfWorks <= filter.DateOfWorksTo)
                .WhereIf(!String.IsNullOrEmpty(filter.Scope), s => s.Scope == filter.Scope)
                .WhereIf(!String.IsNullOrEmpty(filter.NameEmployee), s => s.NameEmployee == filter.NameEmployee);

            var timeSheetsDto = timeSheets.Select(timeSheets => new TimeSheetEntryDto
            {
                Id = timeSheets.Id,
                NameEmployee = timeSheets.NameEmployee,
                Scope = timeSheets.Scope,
                WorkHours = timeSheets.WorkHours,
                DateOfWorks = timeSheets.DateOfWorks.ToShortDateString(),
                Comment = timeSheets.Comment
            });

            int pageSize = Configuration.GetConfiguration().PageSize;
            var count = timeSheetsDto.CountAsync();

            return timeSheetsDto.Skip((page - 1) * pageSize).Take(pageSize).ToArrayAsync();
        }

        public Task<int> GetEntriesCount(TimeSheetFiltersDto filter)
        {
            var timeSheets = _context.TimeSheets
                .WhereIf(filter.DateOfWorksFrom.HasValue, s => s.DateOfWorks >= filter.DateOfWorksFrom)
                .WhereIf(filter.DateOfWorksTo.HasValue, s => s.DateOfWorks <= filter.DateOfWorksTo)
                .WhereIf(!String.IsNullOrEmpty(filter.Scope), s => s.Scope == filter.Scope)
                .WhereIf(!String.IsNullOrEmpty(filter.NameEmployee), s => s.NameEmployee == filter.NameEmployee);

            var timeSheetsDto = timeSheets.Select(timeSheets => new TimeSheetEntryDto
            {
                Id = timeSheets.Id,
                NameEmployee = timeSheets.NameEmployee,
                Scope = timeSheets.Scope,
                WorkHours = timeSheets.WorkHours,
                DateOfWorks = timeSheets.DateOfWorks.ToShortDateString(),
                Comment = timeSheets.Comment
            });

            return timeSheetsDto.CountAsync();
        }
    }
}