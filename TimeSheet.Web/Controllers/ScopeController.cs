using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services;
using Services.Dtos;
using TimeSheet.Web.Models;

namespace TimeSheet.Web.Controllers
{
    public class ScopeController : Controller
    {
        private readonly ILogger<ScopeController> _logger;
        private readonly IScopeTableService _scopeTableService;
        private readonly IMemoryCache _memoryCache;

        public ScopeController(ILogger<ScopeController> logger, IMemoryCache memoryCache, IScopeTableService scopeTableService)
        {
            _logger = logger;
            _scopeTableService = scopeTableService;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> Scopes()
        {
            var scopes = await _scopeTableService.Get();

            var cacheDateTime = new CacheDateTimeDto
            {
                CacheCurrentDateTime = _memoryCache.Get<RatesDto>("rate")?.update_at,
                CurrentDateTime = DateTime.UtcNow
            };

            return View(new ScopeModel(cacheDateTime, scopes));
        }
    }
}