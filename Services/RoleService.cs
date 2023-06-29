﻿using Data.Persistence;
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
            var roles = await _context.Users.Include(e => e.RoleList)
                .Where(e => e.UserName == userName)
                .SelectMany(e => e.RoleList)
                .ToListAsync();
            
            return roles.Select(r => r.Name).ToList();
        }
    }
}