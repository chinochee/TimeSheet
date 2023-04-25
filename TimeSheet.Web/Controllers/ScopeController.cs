using Microsoft.AspNetCore.Mvc;
using Services;
using TimeSheet.Web.Models;

namespace TimeSheet.Web.Controllers
{
    public class ScopeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ScopeController> _logger;
        private readonly IScopeTableService _scopeTableService;

        public ScopeController(IConfiguration configuration, ILogger<ScopeController> logger, IScopeTableService scopeTableService)
        {
            _configuration = configuration;
            _logger = logger;
            _scopeTableService = scopeTableService;
        }

        [HttpGet]
        public async Task<IActionResult> Scopes([FromQuery] ScopesFiltersModel filters)
        {
            var tableDto = await _scopeTableService.GetEntries(filters);
            return View(new ScopeTableModel(tableDto, filters));
        }
    }
}
