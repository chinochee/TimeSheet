using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace TimeSheet.Web.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
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
        public async Task<IActionResult> Scopes()
        {
            var scopes = await _scopeTableService.Get();
            return View(scopes);
        }
    }
}