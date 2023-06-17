using Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services;
using Services.Configuration;
using Services.Dtos;
using System.Security.Claims;

namespace TimeSheet.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;
        private readonly SignInManager<Employee> _signInManager;
        private readonly CookieSettings _tableSettings;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService, SignInManager<Employee> signInManager, IOptions<CookieSettings> config)
        {
            _logger = logger;
            _accountService = accountService;
            _signInManager = signInManager;
            _tableSettings = config.Value;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginEntryDto login)
        {
            if (login.UserName is null || login.Password is null) { return View(); }

            var result = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, true, false);

            if (!result.Succeeded) { return View(); }

            var claims = new List<Claim> { new(ClaimTypes.Name, login.UserName) };
            var identity = new ClaimsIdentity(claims, _tableSettings.AuthenticationScheme);

            await HttpContext.SignInAsync(_tableSettings.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("TimeSheets","TimeSheet");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(_tableSettings.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
        
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> ChangePassword(LoginEntryDto user)
        {
            await _accountService.ChangePassword(user);
            return View();
        }
    }
}