using Microsoft.AspNetCore.Mvc;
using Services;
using TimeSheet.Web.Models;

namespace TimeSheet.Web.Controllers
{
    public class ScopeController : Controller
    {
        private readonly ILogger<ScopeController> _logger;
        private readonly IScopeTableService _scopeTableService;

        public ScopeController(ILogger<ScopeController> logger, IScopeTableService scopeTableService)
        {
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
