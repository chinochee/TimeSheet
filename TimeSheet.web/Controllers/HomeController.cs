using Data.Persistence;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Data;
using TimeSheet.Web.Models;

namespace TimeSheet.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TimeSheetContext _timeSheetContext;

        public HomeController(ILogger<HomeController> logger, TimeSheetContext timeSheetContext)
        {
            _logger = logger;
            _timeSheetContext = timeSheetContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TimeSheets()
        {
            var timeSheetList = _timeSheetContext.TimeSheets.ToList();
            return View(timeSheetList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}