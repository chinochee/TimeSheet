﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> TimeSheets([FromQuery]TimeSheetFiltersDto filters, int page = 1)
        {
            int pageSize = Configuration.GetConfiguration().PageSize;
            var count = await _tableService.GetEntriesCount(filters);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            var items = await _tableService.GetEntries(filters, page);

            var result = new TimeSheetTableModel
            {
                Filters = filters,
                Entries = items,
                Page = pageViewModel
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