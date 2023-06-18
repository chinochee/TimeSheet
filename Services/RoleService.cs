using Data.Entities;
using Data.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class RoleService : IRoleService
    {
        private readonly ILogger<RoleService> _logger;
        private readonly UserManager<Employee> _userManager;
        private readonly TimeSheetContext _context;

        public RoleService(ILogger<RoleService> logger, UserManager<Employee> userManager, TimeSheetContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task<string> GetRoleNameByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return user is null? string.Empty : await GetRoleNameById(user.RoleId);
        }

        public async Task<string> GetRoleNameById(int id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);

            return role is null ? string.Empty : role.Name;
        }
    }
}