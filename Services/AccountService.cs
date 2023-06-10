﻿using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Services.Dtos;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly UserManager<Employee> _userManager;

        public AccountService(ILogger<AccountService> logger, UserManager<Employee> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task ChangePassword(LoginEditDto userEdit)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userEdit.UserName);

                if (user is null) { return; }

                if (string.IsNullOrEmpty(user.UserName))
                    await _userManager.SetUserNameAsync(user, userEdit.UserName);

                var passToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                var result = await _userManager.ResetPasswordAsync(user, passToken, userEdit.NewPassword);

                if (result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        _logger.LogWarning($"{error.Description} ({error.Code})");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("LogError {0}", ex.Message);
            }
        }
    }
}