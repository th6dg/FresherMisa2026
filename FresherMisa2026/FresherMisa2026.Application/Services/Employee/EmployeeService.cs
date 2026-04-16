using FresherMisa2026.Application.Interfaces;
using FresherMisa2026.Application.Interfaces.Repositories;
using FresherMisa2026.Application.Interfaces.Services;
using FresherMisa2026.Entities;
using FresherMisa2026.Entities.Employee;
using System;
using System.Collections.Generic;

namespace FresherMisa2026.Application.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(
            IBaseRepository<Employee> baseRepository,
            IEmployeeRepository employeeRepository
            ) : base(baseRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Employee> GetEmployeeByCodeAsync(string code)
        {
            var employee = await _employeeRepository.GetEmployeeByCode(code);
            if (employee == null)
                throw new Exception("Employee not found");

            return employee;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentIdAsync(Guid departmentId)
        {
            return await _employeeRepository.GetEmployeesByDepartmentId(departmentId);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByPositionIdAsync(Guid positionId)
        {
            return await _employeeRepository.GetEmployeesByPositionId(positionId);
        }

        protected override List<ValidationError> ValidateCustom(Employee employee)
        {
            var errors = new List<ValidationError>();

            if (!string.IsNullOrEmpty(employee.EmployeeCode) && employee.EmployeeCode.Length > 20)
            {
                errors.Add(new ValidationError("EmployeeCode", "Mã nhân viên không được vượt quá 20 ký tự"));
            }

            //if (string.IsNullOrEmpty(employee.EmployeeName))
            //{
            //    errors.Add(new ValidationError("EmployeeName", "Tên nhân viên không được để trống"));
            //}

            // Mã nhân viên không được trùng lặp
            var existEmployee = _employeeRepository.GetEmployeeByCode(employee.EmployeeCode).GetAwaiter().GetResult();
            if (existEmployee != null)
            {
                errors.Add(new ValidationError("EmployeeCode", "Mã nhân viên đã tồn tại"));
            }

            // Email phải đúng định dạng (nếu có)
            if (employee.Email != null)
            {
                bool IsValidEmail = Employee.IsValidEmail(employee.Email);
                if (!IsValidEmail)
                {
                    errors.Add(new ValidationError("EmployeeEmail", "Email không đúng định dạng"));
                }
            }

            // Số điện thoại phải đúng định dạng (nếu có)
            if (employee.PhoneNumber != null)
            {
                bool IsValidPhoneNumber = Employee.IsValidPhoneNumber(employee.PhoneNumber);
                if (!IsValidPhoneNumber)
                {
                    errors.Add(new ValidationError("EmployeePhoneNumber", "Số điện thoại không hợp lệ"));
                }
            }

            // Ngày sinh phải nhỏ hơn ngày hiện tại
            if (employee.DateOfBirth != null)
            {
                bool IsValidDate = Employee.IsValidDateOfBirth(employee.DateOfBirth);
                if (!IsValidDate)
                {
                    errors.Add(new ValidationError("EmployeeDate", "Ngày sinh không hợp lệ"));
                }
            }

            return errors;
        }
    }
}