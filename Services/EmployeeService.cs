using Data.Entities;
using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Services.Configuration;

namespace Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly TimeSheetContext _context;
        public EmployeeService(IOptions<TableSettings> config, TimeSheetContext context)
        {
            _context = context;
        }
        public Task<Employee[]> GetEntries() => _context.Employees.OrderByDescending(s => s.Name).ToArrayAsync();
    }
}