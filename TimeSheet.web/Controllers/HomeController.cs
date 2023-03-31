﻿using Data.Persistence;
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

        public IActionResult TimeSheets(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var timeSheets = _timeSheetContext.TimeSheets.ToList();
            switch (sortOrder)
            {
                case "name_desc":
                    timeSheets = timeSheets.OrderByDescending(s => s.Id).ToList();
                    break;
                case "Date":
                    timeSheets = timeSheets.OrderBy(s => s.DateOfWorks).ToList();
                    break;
                case "date_desc":
                    timeSheets = timeSheets.OrderByDescending(s => s.DateOfWorks).ToList();
                    break;
                default:
                    timeSheets = timeSheets.OrderBy(s => s.Id).ToList();
                    break;
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