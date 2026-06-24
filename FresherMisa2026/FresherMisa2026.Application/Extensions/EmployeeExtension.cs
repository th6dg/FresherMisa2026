using FresherMisa2026.Application.CustomException;
using FresherMisa2026.Application.Interfaces.Repositories;
using FresherMisa2026.Application.Interfaces.Services;
using FresherMisa2026.Application.Services;
using FresherMisa2026.Entities.Employee;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace FresherMisa2026.Application.Extensions
{
    public static class EmployeeExtension
    {
        public static async Task<decimal?> GetTotalSalaryOfEmployee(this IEmployeeService service)
        {
            return await service.EmployeeRepo2.GetTotalSalaryEmployee();
        }

        public static async Task<int?> UpdateSalaryEmployee(this IEmployeeService service, string EmployeeCode, decimal newSalary)
        {
                try
                {
                    Employee employee = await service.GetEmployeeByCodeAsync(EmployeeCode);
                    if (newSalary <= 0)
                    {
                        throw new Exception("Mức lương không hợp lệ");
                    }
                    var tran = service.CreateTransaction();
                    using (tran)
                    {
                        try
                        {
                            int? rowEffect = await service.EmployeeRepo2.UpdateEmployeeSalary(EmployeeCode, newSalary, tran);
                            Thread.Sleep(10000);
                            if (newSalary == 1234567890)
                            {
                                throw new TestException();
                            }
                            tran.Commit();
                            return rowEffect;
                        }
                        catch
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }

        //==================================================================Case 2
        public static async Task<int> GetTotalEmployeeInPosition(this IEmployeeService service, string PositionId)
        {
            return await service.EmployeeRepo2.GetTotalEmployeeInPosition(PositionId);
        }

        public static async Task<int> UpdateEmployeePosition(this IEmployeeService service, string EmployeeCode, string PositionId)
        {
            return await service.EmployeeRepo2.UpdateEmployeePosition(EmployeeCode, PositionId);
        }

        //==================================================================================Case3

    }
}
