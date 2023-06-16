using Microsoft.AspNetCore.Http;

namespace Services
{
    public interface IRequestLogger
    {
        Task TimeWork(HttpContext context, Func<Task> next);
    }
}