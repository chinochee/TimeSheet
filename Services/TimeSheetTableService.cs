using Data.Entities;
using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Dtos;
using Services.Extensions;

namespace Services
{
    public class TimeSheetTableService : ITimeSheetTableService
    {
        private readonly IConfiguration _configuration;
        private TimeSheetContext _context;
        public TimeSheetTableService(IConfiguration configuration, TimeSheetContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public Task<TimeSheetEntryDto[]> GetEntries(TimeSheetFiltersDto filter, int page)
        {
            var pageSize = Convert.ToInt32(_configuration.GetSection("PageSize").Value);
            var timeSheets = GetQuerybleFiltredEntries(filter).Skip((page - 1) * pageSize).Take(pageSize);

            var timeSheetsDto = timeSheets.Select(timeSheets => new TimeSheetEntryDto
            {
                Id = timeSheets.Id,
                NameEmployee = timeSheets.NameEmployee,
                Scope = timeSheets.Scope.Name,
                ScopeRate = timeSheets.Scope.Rate.ToString() + " " + timeSheets.Scope.Currency.ShortName,
                WorkHours = timeSheets.WorkHours,
                DateOfWorks = timeSheets.DateOfWorks.ToShortDateString(),
                Comment = timeSheets.Comment
            });

            return timeSheetsDto.ToArrayAsync();
        }

        public Task<int> GetEntriesCount(TimeSheetFiltersDto filter) => GetQuerybleFiltredEntries(filter).CountAsync();

        private IQueryable<TimeSheet> GetQuerybleFiltredEntries(TimeSheetFiltersDto filter)
        {
            return _context.TimeSheets
                .WhereIf(filter.DateOfWorksFrom.HasValue, s => s.DateOfWorks >= filter.DateOfWorksFrom)
                .WhereIf(filter.DateOfWorksTo.HasValue, s => s.DateOfWorks <= filter.DateOfWorksTo)
                .WhereIf(!String.IsNullOrEmpty(filter.Scope), s => s.Scope.Name == filter.Scope)
                .WhereIf(!String.IsNullOrEmpty(filter.NameEmployee), s => s.NameEmployee == filter.NameEmployee);
        }
    }
}