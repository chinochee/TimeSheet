using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Attributes;

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

        [Access("ViewEmployees")]
        [HttpGet]
        public async Task<IActionResult> Employees()
        {
            var employes = await _employeeTableService.GetTopLastYearTimeSheet();
            return View(employes);
        }
    }
}