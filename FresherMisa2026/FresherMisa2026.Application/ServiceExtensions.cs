using FresherMisa2026.Application.Interfaces.Repositories;
using FresherMisa2026.Application.Interfaces.Services;
using FresherMisa2026.Application.Interfaces.Services.Write;
using FresherMisa2026.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Application
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationDI(
            this IServiceCollection services)
        {
            // Base
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddScoped<IDepartmentSerice, DepartmentService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            // Additional
            services.AddScoped(typeof(IBaseWriteService<>), typeof(BaseWriteService<>));
            services.AddScoped<IEmployeeWriteService, EmployeeWriteService>();

            return services;
        }
    }
}
