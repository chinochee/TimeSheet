﻿using Data.Entities;
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
            var timeSheets = GetQuerybleFiltredEntries(filter);

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

            int pageSize = Configuration.GetConfiguration().PageSize;
            var count = timeSheetsDto.CountAsync();

            return timeSheetsDto.Skip((page - 1) * pageSize).Take(pageSize).ToArrayAsync();
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