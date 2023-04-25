using Microsoft.AspNetCore.Mvc;
using Services;
using System.Diagnostics;
using TimeSheet.Web.Models;

namespace TimeSheet.Web.Controllers
{
    public class TimeSheetController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        private readonly ITimeSheetTableService _timeSheetTableService;
        public TimeSheetController(IConfiguration configuration, ILogger<HomeController> logger, ITimeSheetTableService timeSheetTableService)
        {
            _configuration = configuration;
            _logger = logger;
            _timeSheetTableService = timeSheetTableService;
        }

        [HttpGet]
        public async Task<IActionResult> TimeSheets([FromQuery] TimeSheetsFiltersModel filters)
        {
            var tableDto = await _timeSheetTableService.GetEntries(filters);
            return View(new TimeSheetTableModel(tableDto, filters));
        }
    }
}
