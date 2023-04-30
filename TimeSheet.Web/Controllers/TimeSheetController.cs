using Microsoft.AspNetCore.Mvc;
using Services;
using TimeSheet.Web.Models;

namespace TimeSheet.Web.Controllers
{
    public class TimeSheetController : Controller
    {
        private readonly ILogger<TimeSheetController> _logger;
        private readonly ITimeSheetTableService _timeSheetTableService;
        public TimeSheetController(ILogger<TimeSheetController> logger, ITimeSheetTableService timeSheetTableService)
        {
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