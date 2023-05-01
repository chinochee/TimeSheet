using Microsoft.AspNetCore.Mvc;
using Services;
using TimeSheet.Web.Models;

namespace TimeSheet.Web.Controllers
{
    public class TimeSheetController : Controller
    {
        private readonly ILogger<TimeSheetController> _logger;
        private readonly ITimeSheetTableService _timeSheetTableService;
        private readonly IEmployeeService _employeeService;

        public TimeSheetController(ILogger<TimeSheetController> logger, ITimeSheetTableService timeSheetTableService, IEmployeeService employeeService)
        {
            _logger = logger;
            _timeSheetTableService = timeSheetTableService;
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> TimeSheets([FromQuery] TimeSheetsFiltersModel filters)
        {
            var tableDto = await _timeSheetTableService.GetEntries(filters);
            var employees = await _employeeService.Get();

            return View(new TimeSheetTableModel(tableDto, filters, employees));
        }
    }
}