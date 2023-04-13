﻿using Data.Entities;
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
        private readonly TimeSheetContext _context;
        public TimeSheetTableService(IConfiguration configuration, TimeSheetContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<TimeSheetTableDto> GetEntries(TimeSheetFiltersDto filter)
        {
            var pageSize = Convert.ToInt32(_configuration.GetSection("PageSize").Value);
            var timeSheets = GetQueryableFilteredEntries(filter)
                .Select(timeSheets => new TimeSheetEntryDto
                {
                    Id = timeSheets.Id,
                    NameEmployee = timeSheets.NameEmployee,
                    Scope = timeSheets.Scope.Name,
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

        private IQueryable<TimeSheet> GetQueryableFilteredEntries(TimeSheetFiltersDto filter)
        {
            return _context.TimeSheets
                .WhereIf(filter.DateOfWorksFrom.HasValue, s => s.DateOfWorks >= filter.DateOfWorksFrom)
                .WhereIf(filter.DateOfWorksTo.HasValue, s => s.DateOfWorks <= filter.DateOfWorksTo)
                .WhereIf(!String.IsNullOrEmpty(filter.Scope), s => s.Scope.Name == filter.Scope)
                .WhereIf(!String.IsNullOrEmpty(filter.NameEmployee), s => s.NameEmployee == filter.NameEmployee);
        }
    }
}