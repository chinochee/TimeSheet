using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Services
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddServicesLayer(this IServiceCollection services, IConfiguration configuration)
        {
            //TODO регистрируем здесь сервисы

            services.AddScoped<ITimeSheetTableService, TimeSheetTableService>();

            return services;
        }
    }
}
