using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services;
using Services.Dtos;
using TimeSheet.Web.Models;

namespace TimeSheet.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeService _employeeTableService;
        private readonly IMemoryCache _memoryCache;

        public EmployeeController(ILogger<EmployeeController> logger, IMemoryCache memoryCache, IEmployeeService employeeTableService)
        {
            _logger = logger;
            _employeeTableService = employeeTableService;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> Employees()
        {
            var employes = await _employeeTableService.GetTopLastYearTimeSheet();
            
            var cacheDateTime = new CacheDateTimeDto
            {
                CacheCurrentDateTime = _memoryCache.Get<RatesDto>("rate")?.update_at,
                CurrentDateTime = DateTime.UtcNow
            };

            return View(new EmployeeModel(cacheDateTime, employes));
        }
    }
}