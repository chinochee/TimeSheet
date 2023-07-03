using Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly UserManager<Employee> _userManager;
        private readonly IEmployeeService _employeeService;
        private readonly IRoleService _roleService;
        
        public AccountController(ILogger<AccountController> logger, IAccountService accountService, UserManager<Employee> userManager, IEmployeeService employeeService, IRoleService roleService)
        {
            _logger = logger;
            _accountService = accountService;
            _employeeService = employeeService;
            _roleService = roleService;
            _userManager = userManager;
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
            var user = await _userManager.FindByNameAsync(login.UserName);

            if (user == null) return View();

            var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, login.Password);

            if (!passwordIsCorrect) return View();

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            var roleList = await _roleService.GetRolesNameByUserId(user.Id);
            claims.AddRange(roleList.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

            await _accountService.SignIn(user, claims);

            _logger.LogInformation(1, "User logged in.");

            return RedirectToAction("TimeSheets", "TimeSheet");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieSettingsConstant.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var employees = await _employeeService.Get();

            ViewBag.Employees = employees.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.Id.ToString()
            });

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(LoginEditDto userEdit)
        {
            if (userEdit.EmployeeId == 0 || userEdit.Password == null) { return RedirectToAction(nameof(ChangePassword)); }

            await _accountService.ChangePassword(userEdit);

            return RedirectToAction(nameof(ChangePassword));
        }
    }
}