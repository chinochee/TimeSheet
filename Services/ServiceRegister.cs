﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.HttpClientService;

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
            services.AddScoped<ICurrencyImportService, CurrencyImportService>();
            services.AddScoped<IBitcoinClientFactory, BitcoinClientFactory>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IReInitialUsersService, ReInitialUsersService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddSingleton<IMiddleware, LogTimeWorkMiddleware>();
            services.AddMemoryCache();

            return services;
        }
    }
}