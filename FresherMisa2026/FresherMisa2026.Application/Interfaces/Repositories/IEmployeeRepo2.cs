using FresherMisa2026.Entities.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;

namespace FresherMisa2026.Application.Interfaces.Repositories
{
    public interface IEmployeeRepo2
    {
        Task<decimal?> GetTotalSalaryEmployee();

        Task<int> UpdateEmployeeSalary(string code, decimal newSalary, IDbTransaction tran);

        //===============================================================

        Task<int> GetTotalEmployeeInPosition(string PositionId);

        Task<int> UpdateEmployeePosition(string EmployeeCode, string PositionId);

        //================================================================

        Task<int> InsertNewEmployee(Employee employee);
    }
}
