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
            try
            {
                var employee = await _context.Users.Include(u => u.RoleList).FirstOrDefaultAsync(u => u.UserName == userName);

                return employee.RoleList.Select(r => r.Name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("LogError {0}", ex.Message);
            }

            return null;
        }
    }
}