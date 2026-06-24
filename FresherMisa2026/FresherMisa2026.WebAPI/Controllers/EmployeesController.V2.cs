using FresherMisa2026.Application.Extensions;
using FresherMisa2026.Application.Interfaces.Services.Write;
using FresherMisa2026.Entities.Employee;
using Microsoft.AspNetCore.Mvc;

namespace FresherMisa2026.WebAPI.Controllers
{
    public partial class EmployeesController
    {
        
        [HttpGet("TotalSalary")]
        public async Task<decimal?> GetTotalSalaryOfEmployee()
        {
            return await _employeeService.GetTotalSalaryOfEmployee();
        }

        [HttpPost("UpdateSalary")]
        public async Task<int?> UpdateSalaryByCode(string code, decimal newSalary)
        {
            return await _employeeService.UpdateSalaryEmployee(code, newSalary);
        }

        //==============================================================================Case 2

        [HttpGet("GetNumberEmployeeInPosition")]
        public async Task<int?> GetNumberEmployeeInPosition(string PositionId)
        {
            return await _employeeService.GetTotalEmployeeInPosition(PositionId);
        }

        /// <summary>
        /// Test api for write skew problem (case 2)
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <param name="PositionId"></param>
        /// <returns></returns>
        [HttpPost("UpdateEmployeePosition")]
        public async Task<int> UpdateEmployeePosition(string EmployeeCode, string PositionId)
        {
            return await _employeeService.UpdateEmployeePosition(EmployeeCode, PositionId);
        }

        //===========================================================================Casse3
        [HttpPost("NewEmployee")]
        public async Task<Employee> AddNewEmployee([FromBody] Employee employee)
        {
            Random rand = new Random();
            List<string> DepartmentIdList = new List<string>
            {
                "550e8400-e29b-41d4-a716-446655440010",
                "550e8400-e29b-41d4-a716-446655440011",
                "550e8400-e29b-41d4-a716-446655440012",
                "550e8400-e29b-41d4-a716-446655440013",
                "550e8400-e29b-41d4-a716-446655440014",
                "550e8400-e29b-41d4-a716-446655440015",
                "550e8400-e29b-41d4-a716-446655440016",
                "550e8400-e29b-41d4-a716-446655440017"
            };
            List<string> PositionIdList = new List<string>
            {
                "11111111-1111-1111-1111-111111111111",
                "22222222-2222-2222-2222-222222222222",
                "33333333-3333-3333-3333-333333333333",
                "44444444-4444-4444-4444-444444444444",
                "55555555-5555-5555-5555-555555555555",
                "66666666-6666-6666-6666-666666666666",
                "77777777-7777-7777-7777-777777777777",
                "88888888-8888-8888-8888-888888888888"
            };
            int randomIndex = rand.Next(DepartmentIdList.Count);
            var DefaultEmployee = new Employee
            {
                EmployeeID = Guid.NewGuid(),
                EmployeeCode = Guid.NewGuid().ToString()[0..19],
                EmployeeName = "Pham The Duong test",          
                DepartmentID = Guid.Parse("550e8400-e29b-41d4-a716-446655440004"),
                PositionID = Guid.Parse(PositionIdList[randomIndex]),
                CreatedDate = DateTime.Now
            };
            //Console.WriteLine(DefaultEmployee);
            return await _employeeWriteService.AddAsync(DefaultEmployee);
        }
        
    }
}
