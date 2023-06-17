using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Services
{
    public class LogTimeWorkMiddleware : IMiddleware
    {
        private readonly ILogger<LogTimeWorkMiddleware> _logger;
        public LogTimeWorkMiddleware(ILogger<LogTimeWorkMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var startTime = Stopwatch.GetTimestamp();

            await next(context);

            var elapsedTime = Stopwatch.GetElapsedTime(startTime);

            _logger.LogInformation("Request completed in {0} milliseconds", elapsedTime.Milliseconds);
        }
    }
}