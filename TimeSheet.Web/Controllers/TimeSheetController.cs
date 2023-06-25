using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using Services.Dtos;
using TimeSheet.Web.Models;

namespace TimeSheet.Web.Controllers
{
    public class TimeSheetController : Controller
    {
        private readonly ILogger<TimeSheetController> _logger;
        private readonly ITimeSheetTableService _timeSheetTableService;
        private readonly IEmployeeService _employeeService;
        private readonly IScopeTableService _scopeTableService;

        public TimeSheetController(ILogger<TimeSheetController> logger, ITimeSheetTableService timeSheetTableService, IEmployeeService employeeService, IScopeTableService scopeTableService)
        {
            _logger = logger;
            _timeSheetTableService = timeSheetTableService;
            _employeeService = employeeService;
            _scopeTableService = scopeTableService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> TimeSheets([FromQuery] TimeSheetsFiltersModel filters)
        {
            var tableDto = await _timeSheetTableService.GetEntries(filters);
            var employees = await _employeeService.Get();

            return View(new TimeSheetTableModel(tableDto, filters, employees));
        }

        [Authorize(Roles = "Manager, Employee")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var employees = await _employeeService.Get();
            var scopes = await _scopeTableService.GetDictionary();

            ViewBag.Employees = employees.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.Id.ToString()
            });

            ViewBag.Scopes = scopes.Select(t => new SelectListItem
            {
                Text = t.Value,
                Value = t.Key.ToString()
            });

            return View();
        }

        [Authorize(Roles = "Manager, Employee")]
        [HttpPost]
        public async Task<IActionResult> Create(TimeSheetCreateDto timeSheet)
        {
            await _timeSheetTableService.Add(timeSheet);
            return RedirectToAction(nameof(TimeSheets));
        }
    }
}