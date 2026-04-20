using FresherMisa2026.Application.Interfaces.Services;
using FresherMisa2026.Entities;
using FresherMisa2026.Entities.Employee;
using FresherMisa2026.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace FresherMisa2026.WebAPI.Controllers
{
    [ApiController]
    public class EmployeesController : BaseController<Employee>
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(
            IEmployeeService employeeService) : base(employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("Code/{code}")]
        public async Task<ActionResult<ServiceResponse>> GetByCode(string code)
        {
           
            var response = new ServiceResponse();
            try
            {
                response.Code = 200;
                response.Data = await _employeeService.GetEmployeeByCodeAsync(code);
                response.IsSuccess = true;
            }
            catch
            {
                response.Code = 404;
                response.IsSuccess = false;
                response.DevMessage = "Not found Employee";
            }
            return response;
        }

        [HttpGet("Department/{departmentId}")]
        public async Task<ActionResult<ServiceResponse>> GetByDepartmentId(Guid departmentId)
        {
            var response = new ServiceResponse();
            response.Data = await _employeeService.GetEmployeesByDepartmentIdAsync(departmentId);
            response.IsSuccess = true;

            return response;
        }

        [HttpGet("Position/{positionId}")]
        public async Task<ActionResult<ServiceResponse>> GetByPositionId(Guid positionId)
        {
            var response = new ServiceResponse();
            response.Data = await _employeeService.GetEmployeesByPositionIdAsync(positionId);
            response.IsSuccess = true;

            return response;
        }

        [HttpGet("filter")]
        public async Task<ActionResult<ServiceResponse>> GetBySomeCondition([FromQuery] EmployeeFilterRequest filterRequest, SimplePagingRequest pagingRequest)
        {
            int PageSize = pagingRequest.PageSize;
            int PageNum = pagingRequest.PageNum;
            var response = new ServiceResponse();
            // Handle edge case
            if (filterRequest == null || filterRequest.GetType().GetProperties().All(p => p.GetValue(filterRequest) == null))
            {
                response.IsSuccess = false;
                response.Code = (int)ResponseCode.NotFound;
                return NotFound(response);
            }
            response.IsSuccess = true;
            var data = (await _employeeService.GetBySomeCondition(filterRequest)).ToList();
            if (pagingRequest.PageNum < 1 || pagingRequest.PageSize < 0)
            {
                response.IsSuccess = false;
                response.UserMessage = "Tham số không hợp lệ";
                return NotFound(response);
            }
            else if ((PageNum - 1) * PageSize > data.Count())
            {
                response.IsSuccess = false;
                response.UserMessage = "Tham số quá lớn";
                return NotFound(response);
            }
            else
            {
                //response.IsSuccess = true;
                //response.Data = data[((PageNum - 1) * PageSize)..(PageNum * PageSize)];
                //return Ok(response);
                var response1 = new PagingResponse<Employee>
                {
                    Total = PageSize,
                    Data = data.GetRange((PageNum - 1) * PageSize, PageSize)
                };
                response.IsSuccess = true;
                response.Code = (int)ResponseCode.Success;
                response.Data = response1;
                return response;
            }

        }
    }
}