using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Attributes;

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

        [Access("ViewScopes")]
        [HttpGet]
        public async Task<IActionResult> Scopes()
        {
            var scopes = await _scopeTableService.Get();
            return View(scopes);
        }
    }
}