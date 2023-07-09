using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Dtos;

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

        public async Task<List<RoleEntryDto>> GetRolesByUserId(int userId)
        {
            var roles = await _context.Users
                .Where(e => e.Id == userId)
                .SelectMany(e => e.RoleList)
                .Select(r => new RoleEntryDto
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync();
            
            return roles;
        }

        public async Task<List<RoleEntryDto>> GetRoles()
        {
            var roles = await _context.Roles.Select(r => new RoleEntryDto
            {
                Id = r.Id,
                Name = r.Name,
            }).ToListAsync();

            return roles;
        }
    }
}