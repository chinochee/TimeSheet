using Data.Entities;
using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultContext");
            services.AddDbContext<TimeSheetContext>(options => options.UseSqlite(connectionString)); 
            services.AddIdentityCore<Employee>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<TimeSheetContext>();
            return services;
        }
    }
}