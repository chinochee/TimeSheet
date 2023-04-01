using Data.Entities;
using Data.Persistence;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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

        public IActionResult TimeSheets(string searchDateOfWorksFrom, string searchDateOfWorksTo)
        {
            var timeSheets = _timeSheetContext.TimeSheets.ToList();

            //Filter by DateOfWorks
            if (!String.IsNullOrEmpty(searchDateOfWorksFrom) && !String.IsNullOrEmpty(searchDateOfWorksTo))
            {
                DateTime searchDateOfWorksFromDateTime = DateTime.Parse(searchDateOfWorksFrom);
                DateTime searchDatOfWorksToDateTime = DateTime.Parse(searchDateOfWorksTo);

                timeSheets = timeSheets.Where(s => s.DateOfWorks >= searchDateOfWorksFromDateTime && s.DateOfWorks <= searchDatOfWorksToDateTime).ToList();
            }

            return View(timeSheets);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}