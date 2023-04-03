using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Dtos;
using System.Diagnostics;
using TimeSheet.Web.Models;

namespace TimeSheet.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITimeSheetTableService _tableService;

        public HomeController(ILogger<HomeController> logger, ITimeSheetTableService tableService)
        {
            _logger = logger;
            _tableService = tableService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> TimeSheets([FromQuery]TimeSheetFiltersDto filters)
        {
            var result = new TimeSheetTableModel
            {
                Filters = filters,
                Entries = await _tableService.GetEntries(filters)
            };

            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}