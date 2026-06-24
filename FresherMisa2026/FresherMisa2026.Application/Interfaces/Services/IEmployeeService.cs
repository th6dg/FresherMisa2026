using FresherMisa2026.Application.Interfaces.Repositories;
using FresherMisa2026.Entities;
using FresherMisa2026.Entities.Employee;
using FresherMisa2026.Entities.Employee.DTO;
using FresherMisa2026.Entities.RequestDTO;
using System;
using System.Collections.Generic;
using System.Data;

namespace FresherMisa2026.Application.Interfaces.Services
{
    public interface IEmployeeService : IBaseService<Employee>
    {
        IEmployeeRepo2 EmployeeRepo2 { get;  }
        Task<Employee> GetEmployeeByCodeAsync(string code);
        Task<IEnumerable<Employee>> GetEmployeesByDepartmentIdAsync(Guid departmentId);
        Task<IEnumerable<Employee>> GetEmployeesByPositionIdAsync(Guid positionId);
        Task<IEnumerable<Employee>> GetBySomeCondition(EmployeeFilterRequest employeeFilterRequest);

        Task<IEnumerable<dynamic>> GetDynamicsEmployee(Employee0RequestDTO employee0RequestDTO);

    }
}