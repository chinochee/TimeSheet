using Microsoft.AspNetCore.Mvc;
using Services;

namespace TimeSheet.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeService _employeeTableService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeTableService)
        {
            _logger = logger;
            _employeeTableService = employeeTableService;
        }

        [HttpGet]
        public async Task<IActionResult> Employees()
        {
            var scopes = await _employeeTableService.GetTopLastYearTimeSheet();
            return View(scopes);
        }
    }
}