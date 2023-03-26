using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Data.Persistence
{
    public static class DataBaseExtensions
    {
        public static void Migrate(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<TimeSheetContext>();
                db.Database.Migrate();
            }
        }
    }
}
