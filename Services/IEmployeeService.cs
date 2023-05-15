﻿using Services.Dtos;

namespace Services
{
    public interface IEmployeeService
    {
        Task<EmployeeEntryDto[]> Get();
        Task<EmployeeEntryBTCDto[]> GetTopLastYearTimeSheet();
    }
}