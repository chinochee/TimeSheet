﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Services
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddServicesLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITimeSheetTableService, TimeSheetTableService>();
            services.AddScoped<IScopeTableService, ScopeTableService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ICurrencyExportService, CurrencyExportService>();

            return services;
        }
    }
}