using Microsoft.AspNetCore.Identity;
using Services.Dtos;

namespace Services
{
    public interface IEmployeeService
    {
        Task<EmployeeEntryDto[]> Get();
        Task<EmployeeEntryBTCDto[]> GetTopLastYearTimeSheet();
        Task<IdentityResult> Create(RegisterDataDto user);
    }
}