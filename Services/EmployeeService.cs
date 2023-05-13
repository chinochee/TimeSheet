using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Services.Configuration;
using Services.Dtos;

namespace Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly TimeSheetContext _context;
        private readonly TableSettings _tableSettings;

        public EmployeeService(IOptions<TableSettings> config, TimeSheetContext context)
        {
            _tableSettings = config.Value;
            _context = context;
        }

        public async Task<EmployeeEntryDto[]> Get() => await _context.Employees
            .Select(e => new EmployeeEntryDto { Id = e.Id, Name = e.Name }).ToArrayAsync();

        public async Task<EmployeeEntryDto[]> GetTopLastYearTimeSheet()
        {
            var rateBTC = await ApiCalls.GetRateBTC();

            return await _context.Employees.Select(e => new EmployeeEntryDto
            {
                Id = e.Id,
                Name = e.Name,
                TotalPriceUSD =
                    Math.Round(
                        e.TimeSheetList.Where(t => t.DateOfWorks >= DateTime.UtcNow.AddYears(-1)).Sum(timeSheet =>
                            timeSheet.WorkHours * timeSheet.Scope.Rate * timeSheet.Scope.Currency.DollarExchangeRate ??
                            0), 2),
                TotalPriceInBTC =
                    Math.Round(
                        e.TimeSheetList.Where(t => t.DateOfWorks >= DateTime.UtcNow.AddYears(-1)).Sum(timeSheet =>
                            timeSheet.WorkHours * timeSheet.Scope.Rate * timeSheet.Scope.Currency.DollarExchangeRate ??
                            0) / rateBTC, 2)
            }).OrderByDescending(s => s.TotalPriceUSD).Take(_tableSettings.TopEmployees).ToArrayAsync();
        }
    }
}