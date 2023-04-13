using Microsoft.AspNetCore.Mvc;
using Services;
using System.Diagnostics;
using TimeSheet.Web.Models;

namespace TimeSheet.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        private readonly ITimeSheetTableService _tableService;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger, ITimeSheetTableService tableService)
        {
            _configuration = configuration;
            _logger = logger;
            _tableService = tableService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> TimeSheets([FromQuery]TimeSheetsFiltersModel filters)
        {
            var tableDto = await _tableService.GetEntries(filters);
            return View(new TimeSheetTableModel(tableDto, filters));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}