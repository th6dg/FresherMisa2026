using FresherMisa2026.Entities.Employee;
using FresherMisa2026.Entities.Employee.DTO;
using FresherMisa2026.Entities.RequestDTO;
using System;
using System.Collections.Generic;
using System.Data;

namespace FresherMisa2026.Application.Interfaces.Repositories
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        
        Task<Employee> GetEmployeeByCode(string code);
        Task<IEnumerable<Employee>> GetEmployeesByDepartmentId(Guid departmentId);
        Task<IEnumerable<Employee>> GetEmployeesByPositionId(Guid positionId);
        Task<IEnumerable<Employee>> GetEmployeesByFilter(Dictionary<string, object> listField);

        Task<IEnumerable<dynamic>> GetEmployeeDynamically(Employee0RequestDTO listParam);
    }
}
