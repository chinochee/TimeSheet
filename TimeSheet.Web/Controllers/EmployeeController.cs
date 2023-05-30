using Microsoft.AspNetCore.Mvc;
using Services;

namespace TimeSheet.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeService _employeeTableService;
        private readonly IBitcoinHttpClient _client;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeTableService, IBitcoinHttpClient client)
        {
            _logger = logger;
            _employeeTableService = employeeTableService;
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Employees()
        {
            var scopes = await _employeeTableService.GetTopLastYearTimeSheet();
            return View(scopes);
        }
    }
}