using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class RoleService : IRoleService
    {
        private readonly ILogger<RoleService> _logger;
        private readonly TimeSheetContext _context;

        public RoleService(ILogger<RoleService> logger, TimeSheetContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<string>> GetRolesNameByUserName(string userName)
        {
            var roles = await _context.Roles.Include(r => r.EmployeeList)
                .Where(r => r.EmployeeList.Any(e => e.UserName == userName))
                .Select(r => r.Name)
                .ToListAsync();
            
            return roles;
        }
    }
}