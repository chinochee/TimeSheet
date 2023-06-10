using Data.Entities;
using Data.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class ReInitialUsersService : IReInitialUsersService
    {
        private readonly ILogger<ReInitialUsersService> _logger;
        private readonly UserManager<Employee> _userManager;
        private readonly TimeSheetContext _context;

        public ReInitialUsersService(ILogger<ReInitialUsersService> logger, UserManager<Employee> userManager, TimeSheetContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task ReInitializeUsers()
        {
            try
            {
                var timeSheets = await _context.TimeSheets.ToArrayAsync();

                var users = await _userManager.Users.Where(u => String.IsNullOrEmpty(u.UserName) && String.IsNullOrEmpty(u.PasswordHash)).ToListAsync();

                foreach (var user in users)
                {
                    await _userManager.DeleteAsync(user);

                    user.UserName = $"{user.Name}Login";
                    
                    var result = await _userManager.CreateAsync(user, $"{user.Name}Pass1!");

                    if (result.Succeeded) continue;

                    foreach (var error in result.Errors)
                        _logger.LogWarning($"{error.Description} ({error.Code})");
                }

                await _context.TimeSheets.AddRangeAsync(timeSheets);
            }
            catch (Exception ex)
            {
                _logger.LogError("LogError {0}", ex.Message);
            }
        }
    }
}