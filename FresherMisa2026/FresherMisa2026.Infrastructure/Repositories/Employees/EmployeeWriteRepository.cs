using FresherMisa2026.Application.Interfaces.Repositories.Write;
using FresherMisa2026.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using FresherMisa2026.Entities.Employee;

namespace FresherMisa2026.Infrastructure.Repositories
{
    public class EmployeeWriteRepository :BaseWriteRepository<Employee>, IEmployeeWriteRepository
    {
       public EmployeeWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
