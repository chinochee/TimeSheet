﻿using Services.Dtos;

namespace Services
{
    public interface ITimeSheetTableService
    {
        Task<TimeSheetTableDto> GetEntries(TimeSheetFiltersDto filter);
        Task Add(TimeSheetCreateDto timeSheet);
    }
}