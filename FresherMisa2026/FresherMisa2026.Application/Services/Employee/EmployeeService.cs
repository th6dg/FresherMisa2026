using FresherMisa2026.Application.CustomException;
using FresherMisa2026.Application.Interfaces;
using FresherMisa2026.Application.Interfaces.Repositories;
using FresherMisa2026.Application.Interfaces.Repositories.Write;
using FresherMisa2026.Application.Interfaces.Services;
using FresherMisa2026.Entities;
using FresherMisa2026.Entities.Employee;
using FresherMisa2026.Entities.Employee.DTO;
using FresherMisa2026.Entities.Enums;
using FresherMisa2026.Entities.RequestDTO;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.Metrics;


namespace FresherMisa2026.Application.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        private readonly IEmployeeRepo2 _employeeRepo2;
        public IEmployeeRepo2 EmployeeRepo2 => _employeeRepo2;


        public EmployeeService(
            IBaseRepository<Employee> baseRepository,
            IEmployeeRepository employeeRepository,
            IEmployeeRepo2 employeeRepo2,
            IEmployeeWriteRepository employeeWriteRepository
            ) : base(baseRepository)
        {
            _employeeRepository = employeeRepository;
            _employeeRepo2 = employeeRepo2;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        protected override List<ValidationError> ValidateCustom(Employee employee)
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

        public async Task<IEnumerable<Employee>> GetBySomeCondition(EmployeeFilterRequest employeeFilterRequest)
        {
            var validField = GetValidField(employeeFilterRequest);
            return await _employeeRepository.GetEmployeesByFilter(validField);
        }

        /// <summary>
        /// Lọc ra các trường khác null và thoả mãn filter 
        /// </summary>
        /// <param name="filterRequest"></param>
        /// <returns></returns>
        protected Dictionary<string, object> GetValidField(EmployeeFilterRequest filterRequest)
        {
            var errors = new List<ValidationError>();
            var validField = new Dictionary<string, object>();
            EmployeeFilterRequest.TrimAllStringField(filterRequest);

            // Department
            if (!string.IsNullOrEmpty(filterRequest.DepartmentID)) 
            {
                if (!Employee.IsValidGuid((filterRequest.DepartmentID)))
                {
                    errors.Add(new ValidationError("Department", "Mã Department lỗi"));
                }
                else
                {
                    validField.Add("DepartmentID", filterRequest.DepartmentID);
                }
                
            }

            // Position
            if (!string.IsNullOrEmpty(filterRequest.PositionID))
            {
                if (!Employee.IsValidGuid((filterRequest.PositionID)))
                {
                    errors.Add(new ValidationError("Position", "Mã Position lỗi"));
                }
                else
                {
                    validField.Add("PositionID", filterRequest.PositionID);
                }
            }

            // Gender
            if (filterRequest.Gender != null) 
            {
                var validGenders = new List<int?> { 0, 1, 2 };
                if (!validGenders.Contains(filterRequest.Gender))
                {
                    errors.Add(new ValidationError("Gender", "Gender lỗi"));
                }
                else
                {
                    validField.Add("Gender", filterRequest.Gender);
                }
            }

            // Salary
            if ((filterRequest.SalaryFrom == null) == (filterRequest.SalaryTo == null))
            {
                if (filterRequest.SalaryFrom != null)
                {
                    if (EmployeeFilterRequest.IsValidSalary(filterRequest))
                    {
                        validField.Add("SalaryFrom", filterRequest.SalaryFrom);
                        validField.Add("SalaryTo", filterRequest.SalaryTo);
                    }
                }
            }
            if (!((filterRequest.SalaryFrom == null) == (filterRequest.SalaryTo == null)))
            {
                errors.Add(new ValidationError("Salary", "Salary lỗi"));
            }

            // Hire Date
            if ((filterRequest.HireDateFrom == null) == (filterRequest.HireDateTo == null))
            {
                if (filterRequest.HireDateFrom != null)
                {
                    if (EmployeeFilterRequest.IsValidHireDate(filterRequest))
                    {
                        validField.Add("HireDateFrom", filterRequest.HireDateFrom);
                        validField.Add("HireDateTo", filterRequest.HireDateTo);
                    }
                }
            }
            if (!((filterRequest.HireDateFrom == null) == (filterRequest.HireDateTo == null)))
            {
                errors.Add(new ValidationError("Hire Date", "Hire Date lỗi"));
            }
            
            if (errors.Any())
            {
                throw new ValidateException(errors);
            }
            return validField;
        }

        /// <summary>
        /// Ghi đè insert method, xử lý Race Condition 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override async Task<ServiceResponse> InsertAsync(Employee entity)
        {
            // Check required, custom validate
            var errors = Validate(entity);
            if (errors.Count > 0)
            {
                return CreateErrorResponse(
                ResponseCode.BadRequest,
                "Validate thất bại",
                string.Join("; ", errors.Select(static e => e.Message)));
            }

            else
            {
                try
                {
                    //Thread.Sleep(3000);
                    int rowEffect = await _employeeRepository.InsertAsync(entity);
                    var response = new ServiceResponse();
                    response.IsSuccess = true;
                    response.Data = 1;
                    return response;
                }
                catch
                {
                    throw new CanNotAddEmployeeException(entity.EmployeeCode);
                }
                finally { }
            }
        }

        public async Task<IEnumerable<dynamic>> GetDynamicsEmployee(Employee0RequestDTO employee0RequestDTO)
        {
            var rawData = await _employeeRepository.GetEmployeeDynamically(employee0RequestDTO);
            try
            {
                Func<dynamic, dynamic> transform = (OldItem) =>
                {
                    var NewItem = new Dictionary<string, object>();
                    var source = (IDictionary<string, object>)OldItem;

                    foreach (var item in employee0RequestDTO.VisibleColumns)
                    {
                        if(!source.ContainsKey(item))
                        {
                            NewItem[item] = null;
                        }
                        else
                        {
                            NewItem[item] = source[item];
                        }
                    }

                    return NewItem;
                };
                var result = rawData.Select(item => transform(item));
                return result;

            }
            catch
            {
                throw;
            }
        }
    }

        
}