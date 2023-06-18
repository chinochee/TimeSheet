﻿using Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Services;
using Services.Configuration;
using Services.Dtos;
using System.Security.Claims;
using TimeSheet.Web.Models;

namespace TimeSheet.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;
        private readonly SignInManager<Employee> _signInManager;
        private readonly CookieSettings _tableSettings;
        private readonly IEmployeeService _employeeService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService, SignInManager<Employee> signInManager, IOptions<CookieSettings> config, IEmployeeService employeeService)
        {
            _logger = logger;
            _accountService = accountService;
            _signInManager = signInManager;
            _tableSettings = config.Value;
            _employeeService = employeeService;
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
            var employees = await _employeeService.Get();

            ViewBag.Employees = employees.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.Id.ToString()
            });

            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> ChangePassword(LoginEditDto userEdit)
        {
            if (userEdit.EmployeeId == 0 || userEdit.Password == null) { return RedirectToAction(nameof(ChangePassword)); }

            await _accountService.ChangePassword(userEdit);

            return RedirectToAction(nameof(ChangePassword));
        }
    }
}