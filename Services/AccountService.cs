using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Services.Configuration;
using Services.Dtos;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;

        public AccountService(ILogger<AccountService> logger, SignInManager<Employee> signInManager, UserManager<Employee> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task ChangePassword(LoginEditDto userEdit)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userEdit.EmployeeId.ToString());

                if (user is null) { return; }

                var passToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                var result = await _userManager.ResetPasswordAsync(user, passToken, userEdit.Password);

                if (result.Succeeded) { return; }

                foreach (var error in result.Errors)
                    _logger.LogWarning($"{error.Description} ({error.Code})");
            }
            catch (Exception ex)
            {
                _logger.LogError("LogError {0}", ex.Message);
            }
        }

        public async Task SignIn(Employee user, IEnumerable<Claim> customClaims, bool isPersistent = true)
        {
            var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);

            if (customClaims != null && claimsPrincipal?.Identity is ClaimsIdentity claimsIdentity)
            {
                claimsIdentity.AddClaims(customClaims);
            }

            await _signInManager.Context.SignInAsync(CookieSettingsConstant.AuthenticationScheme,
                claimsPrincipal,
                new AuthenticationProperties { IsPersistent = isPersistent });
        }
    }
}