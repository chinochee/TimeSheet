﻿using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Services.Configuration;
using Services.Dtos;

namespace Services
{
    public class ScopeTableService : IScopeTableService
    {
        private readonly TableSettings _tableSettings;
        private readonly TimeSheetContext _context;
        public ScopeTableService(IOptions<TableSettings> config, TimeSheetContext context)
        {
            _tableSettings = config.Value;
            _context = context;
        }
        public async Task<ScopeTableDto> GetEntries(ScopeFiltersDto filter)
        {
            var pageSize = _tableSettings.PageSize;
            var scopes = _context.TimeSheets
                .Select(timeSheets => new ScopeEntryDto
                {
                    Id = timeSheets.Scope.Id,
                    Name = timeSheets.Scope.Name,
                    Rate = $"{timeSheets.Scope.Rate} {timeSheets.Scope.Currency.ShortName}",
                    RateUSD = $"{timeSheets.Scope.Rate * timeSheets.Scope.Currency.DollarExchangeRate} USD"
                });

            var count = await scopes.CountAsync();

            var entries = await scopes.Skip((filter.PageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();
            
            return new ScopeTableDto
            {
                Entries = entries,
                Total = count,
                PageSize = pageSize
            };
        }
    }
}