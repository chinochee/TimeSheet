using Data.Entities;
using Data.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Configuration;

namespace Services
{
    public class ReInitialUsersService : IReInitialUsersService
    {
        private readonly ILogger<ReInitialUsersService> _logger;
        private readonly UserManager<Employee> _userManager;
        private readonly TimeSheetContext _context;
        private readonly BaseUserCredis _baseUserCredis;

        public ReInitialUsersService(ILogger<ReInitialUsersService> logger, UserManager<Employee> userManager, TimeSheetContext context, IOptions<BaseUserCredis> config)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _baseUserCredis = config.Value;
        }

        public async Task ReInitializeUsers()
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var timeSheets = await _context.TimeSheets.ToListAsync();

                var users = await _userManager.Users
                    .Where(u => String.IsNullOrEmpty(u.UserName) && String.IsNullOrEmpty(u.PasswordHash))
                    .ToListAsync();

                foreach (var user in users)
                {
                    await _userManager.DeleteAsync(user);

                    user.UserName = $"{user.Name}{_baseUserCredis.Login}";

                    var result = await _userManager.CreateAsync(user, $"{user.Name}{_baseUserCredis.Password}");

                    if (result.Succeeded) continue;

                    foreach (var error in result.Errors)
                        _logger.LogWarning($"{error.Description} ({error.Code})");
                }

                await _context.TimeSheets.AddRangeAsync(timeSheets);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError("LogError {0}", ex.Message);
            }
        }
    }
}