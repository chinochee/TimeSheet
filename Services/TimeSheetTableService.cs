using Data.Entities;
using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Configuration;
using Services.Dtos;
using Services.Extensions;

namespace Services
{
    public class TimeSheetTableService : ITimeSheetTableService
    {
        private readonly ILogger<TimeSheetTableService> _logger;
        private readonly TableSettings _tableSettings;
        private readonly TimeSheetContext _context;
        public TimeSheetTableService(ILogger<TimeSheetTableService> logger, IOptions<TableSettings> config, TimeSheetContext context)
        {
            _tableSettings = config.Value;
            _context = context;
            _logger = logger;
        }

        public async Task<TimeSheetTableDto> GetEntries(TimeSheetFiltersDto filter)
        {
            try
            {
                var pageSize = _tableSettings.PageSize;
                var timeSheets = GetQueryableFilteredEntries(filter)
                    .Select(timeSheets => new TimeSheetEntryDto
                    {
                        Id = timeSheets.Id,
                        NameEmployee = timeSheets.Employee.Name,
                        NameScope = timeSheets.Scope.Name,
                        WorkHours = timeSheets.WorkHours,
                        DateOfWorks = timeSheets.DateOfWorks.ToShortDateString(),
                        Comment = timeSheets.Comment
                    });

                var count = await timeSheets.CountAsync();

                var entries = await timeSheets.Skip((filter.PageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToArrayAsync();

                return new TimeSheetTableDto
                {
                    Entries = entries,
                    Total = count,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("LogError {0}", ex.Message);
            }

            return null;
        }

        public async Task Add(TimeSheetCreateDto timeSheet)
        {
            try
            {
                await _context.TimeSheets.AddAsync(new TimeSheet
                {
                    EmployeeId = timeSheet.EmployeeId,
                    ScopeId = timeSheet.ScopeId,
                    WorkHours = timeSheet.WorkHours,
                    DateOfWorks = timeSheet.DateOfWorks,
                    Comment = timeSheet.Comment,
                    DateLastEdit = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("LogError {0}", ex.Message);
            }
        }

        private IQueryable<TimeSheet> GetQueryableFilteredEntries(TimeSheetFiltersDto filter)
        {
            try
            {
                var result = _context.TimeSheets
                    .WhereIf(filter.DateOfWorksFrom.HasValue, s => s.DateOfWorks >= filter.DateOfWorksFrom)
                    .WhereIf(filter.DateOfWorksTo.HasValue, s => s.DateOfWorks <= filter.DateOfWorksTo)
                    .WhereIf(!String.IsNullOrEmpty(filter.Scope), s => s.Scope.Name == filter.Scope)
                    .WhereIf(filter.EmployeeId.HasValue, s => s.Employee.Id == filter.EmployeeId);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("LogError {0}", ex.Message);
            }

            return null;
        }
    }
}