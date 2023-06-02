using Data.Persistence;
using Microsoft.Extensions.Logging;
using Services.Dtos;

namespace Services
{
    internal class AccountService : IAccountService
    {
        private readonly TimeSheetContext _context;
        private readonly ILogger<AccountService> _logger;

        public AccountService(ILogger<AccountService> logger, TimeSheetContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task EditEmployee(LoginEditDto userEdit)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == userEdit.Name);
            user.UserName = userEdit.UserName;
            user.PasswordHash = userEdit.Password;

            _context.Users.Update(user);

            await _context.SaveChangesAsync();
        }
    }
}