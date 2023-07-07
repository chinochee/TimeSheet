using Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Services;
using Services.Attributes;
using Services.Configuration;
using Services.Dtos;
using System.Security.Claims;
using static Services.Extensions.ListExtensions;

namespace TimeSheet.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;
        private readonly UserManager<Employee> _userManager;
        private readonly IEmployeeService _employeeService;
        private readonly IRoleService _roleService;
        private readonly Dictionary<int, List<string>> _rolesPermissions;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService, UserManager<Employee> userManager, IEmployeeService employeeService, IRoleService roleService, IOptions<Permissions> config)
        {
            _logger = logger;
            _accountService = accountService;
            _employeeService = employeeService;
            _roleService = roleService;
            _userManager = userManager;
            _rolesPermissions = config.Value.RolesPermissions;
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

            var roleList = await _roleService.GetRolesByUserId(user.Id);

            var allPermissions = new List<string>();
            foreach (var role in roleList)
            {
                var rolePermissions = _rolesPermissions.Where(r => r.Key == role.Id).SelectMany(r => r.Value).ToList();
                allPermissions.AddRange(rolePermissions);
            }

            allPermissions = RemoveDuplicates(allPermissions);

            claims.AddRange(allPermissions.Select(permissions => new Claim(PermissionsConstant.ClaimType, permissions)));

            await _accountService.SignIn(user, claims);

            _logger.LogInformation(1, "User logged in.");

            return RedirectToAction("TimeSheets", "TimeSheet");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Registration()
        {
            var roles = await _roleService.GetRoles();

            ViewBag.Roles = roles.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.Id.ToString()
            });
            
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Registration(RegisterDataDto user)
        {
            if (user.UserName is null || user.Name is null || user.Password is null || user.RoleIdList.Count == 0) { return View(); }

            var result = await _employeeService.Create(user);

            if (!result.Succeeded) { return View(); }

            return await Login(new LoginEntryDto
            {
                UserName = user.UserName,
                Password = user.Password
            });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieSettingsConstant.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [Access("EditUser")]
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

        [Access("EditUser")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(LoginEditDto userEdit)
        {
            if (userEdit.EmployeeId == 0 || userEdit.Password == null) { return RedirectToAction(nameof(ChangePassword)); }

            await _accountService.ChangePassword(userEdit);

            return RedirectToAction(nameof(ChangePassword));
        }
    }
}