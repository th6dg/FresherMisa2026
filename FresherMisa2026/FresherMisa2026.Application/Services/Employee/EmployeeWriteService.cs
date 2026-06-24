using FresherMisa2026.Application.CustomException;
using FresherMisa2026.Application.Interfaces;
using FresherMisa2026.Application.Interfaces.Repositories;
using FresherMisa2026.Application.Interfaces.Repositories.Write;
using FresherMisa2026.Application.Interfaces.Services;
using FresherMisa2026.Application.Interfaces.Services.Write;
using FresherMisa2026.Entities;
using FresherMisa2026.Entities.Employee;
using FresherMisa2026.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Application.Services
{
    public class EmployeeWriteService : BaseWriteService<Employee>, IEmployeeWriteService
    {
        private readonly IEmployeeWriteRepository _employeeWriteRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IBaseService<Employee> _baseService;
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeWriteService(
            IEmployeeWriteRepository employeeWriteRepository,
            IEmployeeRepository employeeRepository,
            IBaseWriteRepository<Employee> baseRepository,
            IUnitOfWork unitOfWork
            ) :base(baseRepository)
            
        {
            _employeeWriteRepository = employeeWriteRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }
        

        /// <summary>
        /// Insert new recore employee to database, hope it work correctly
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override async Task<Employee> AddAsync(Employee employee)
        {
            // Check required, custom validate
            bool MockValidatorError = false;
            if (MockValidatorError)
            {
                throw new TestException("Mock Validate Employee Failed");
            }

            else
            {
                await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _employeeWriteRepository.InsertEntityAsync(employee);
                    int rowEffect = await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitAsync();
                    if (rowEffect == 0)
                    {
                        throw new Exception("Insert Employee Failed");
                    }
                    return employee;
                }
                catch
                {
                    await _unitOfWork.RollbackAsync();
                    throw;
                }
                }
                
            
            }
        protected List<ValidationError> ValidateCustom(Employee employee)
        {
            var errors = new List<ValidationError>();

            if (!string.IsNullOrEmpty(employee.EmployeeCode) && employee.EmployeeCode.Length > 20)
            {
                errors.Add(new ValidationError("EmployeeCode", "Mã nhân viên không được vượt quá 20 ký tự"));
            }

            if (string.IsNullOrEmpty(employee.EmployeeName))
            {
                errors.Add(new ValidationError("EmployeeName", "Tên nhân viên không được để trống"));
            }

            // Mã nhân viên không được trùng lặp
            var existEmployee = _employeeRepository.GetEmployeeByCode(employee.EmployeeCode).GetAwaiter().GetResult();
            if (existEmployee != null)
            {
                errors.Add(new ValidationError("EmployeeCode", "Mã nhân viên đã tồn tại"));
            }

            //// Email phải đúng định dạng (nếu có)
            //if (employee.Email != null)
            //{
            //    bool IsValidEmail = Employee.IsValidEmail(employee.Email);
            //    if (!IsValidEmail)
            //    {
            //        errors.Add(new ValidationError("EmployeeEmail", "Email không đúng định dạng"));
            //    }
            //}

            //// Số điện thoại phải đúng định dạng (nếu có)
            //if (employee.PhoneNumber != null)
            //{
            //    bool IsValidPhoneNumber = Employee.IsValidPhoneNumber(employee.PhoneNumber);
            //    if (!IsValidPhoneNumber)
            //    {
            //        errors.Add(new ValidationError("EmployeePhoneNumber", "Số điện thoại không hợp lệ"));
            //    }
            //}

            //// Ngày sinh phải nhỏ hơn ngày hiện tại
            //if (employee.DateOfBirth != null)
            //{
            //    bool IsValidDate = Employee.IsValidDateOfBirth(employee.DateOfBirth);
            //    if (!IsValidDate)
            //    {
            //        errors.Add(new ValidationError("EmployeeDate", "Ngày sinh không hợp lệ"));
            //    }
            //}

            return errors;
        }

        
    }
}
