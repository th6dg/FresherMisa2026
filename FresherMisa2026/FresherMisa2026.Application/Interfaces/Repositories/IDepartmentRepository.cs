using FresherMisa2026.Entities.Department;
using FresherMisa2026.Entities.Employee;
using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Application.Interfaces.Repositories
{
    public interface IDepartmentRepository : IBaseRepository<Department>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<Department> GetDepartmentByCode(string code);

        // 2.3
        Task<IEnumerable<Employee>> GetEmployeeByDepartmentCode(string DepartmentCode);
        Task<int> GetNumberOfEmployeeInDepartment(Guid DepartmentID);
    }
}
