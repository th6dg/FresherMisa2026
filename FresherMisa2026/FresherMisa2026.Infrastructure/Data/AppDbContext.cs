using FresherMisa2026.Entities.Department;
using FresherMisa2026.Entities.Employee;
using FresherMisa2026.Entities.Position;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Infrastructure.Data
{
    // Quản connection, entity 
    // Query, Save data 
    public class AppDbContext : DbContext
    {
        // Override configuration thông qua DI
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> employee { get; set; }
        public DbSet<Department> department { get; set; }
        public DbSet<Position> position { get; set; }
    }
}
