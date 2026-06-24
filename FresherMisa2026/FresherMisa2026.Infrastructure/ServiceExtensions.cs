using Dapper;
using FresherMisa2026.Application.Interfaces;
using FresherMisa2026.Application.Interfaces.Repositories;
using FresherMisa2026.Application.Interfaces.Repositories.Write;
using FresherMisa2026.Entities.Employee;
using FresherMisa2026.Infrastructure.Data;
using FresherMisa2026.Infrastructure.Repositories;
using FresherMisa2026.Infrastructure.utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Infrastructure
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            SqlMapper.AddTypeHandler(typeof(Guid), new GuidTypeHandler());
            SqlMapper.AddTypeHandler(typeof(Guid?), new GuidTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
            //base
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddMemoryCache();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();

            services.AddScoped<EmployeeRepository>();
            services.AddScoped<IEmployeeRepository>(sp => sp.GetRequiredService<EmployeeRepository>());
            services.AddScoped<IEmployeeRepo2>(sp => sp.GetRequiredService<EmployeeRepository>());
            services.AddScoped<IBaseRepository<Employee>>(sp => sp.GetRequiredService<EmployeeRepository>());

            //additional
            services.AddScoped(typeof(IBaseWriteRepository<>), typeof(BaseWriteRepository<>));
            services.AddScoped<IEmployeeWriteRepository, EmployeeWriteRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // AppDbContext is scoped
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
            return services;
        }
    }
}
