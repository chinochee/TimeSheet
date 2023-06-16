using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Services
{
    public class RequestLogger : IRequestLogger
    {
        private readonly ILogger<RequestLogger> _logger;

        public RequestLogger(ILogger<RequestLogger> logger)
        {
            _logger = logger;
        }

        public async Task TimeWork(HttpContext context, Func<Task> next)
        {
            var startTime = Stopwatch.GetTimestamp();

            await next.Invoke();

            var elapsedTime = Stopwatch.GetElapsedTime(startTime);

            _logger.LogInformation("Request completed in {0} milliseconds", elapsedTime.Milliseconds);
        }
    }
}