using Dapper;
using FresherMisa2026.Application.CustomException;
using FresherMisa2026.Application.Extensions;
using FresherMisa2026.Application.Interfaces.Repositories;
using FresherMisa2026.Entities.Employee;
using FresherMisa2026.Entities.Employee.DTO;
using FresherMisa2026.Entities.Position;
using FresherMisa2026.Entities.RequestDTO;
using FresherMisa2026.Infrastructure.Repositories.Config;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FresherMisa2026.Infrastructure.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository, IEmployeeRepo2
    {
        public EmployeeRepository(IConfiguration configuration, IMemoryCache cache) : base(configuration,cache)
        {
        }

        public async Task<Employee> GetEmployeeByCode(string code)
        {
            await OpenConnectionAsync();
            // RAM và Database cấp phát tài nguyên, cần giải phóng ngay khi có thể 
            
            try
            {
                string query = SQLExtension.GetQuery("Employee.GetByCode");
                var param = new Dictionary<string, object>
                {
                    {"@EmployeeCode", code }
                };
                
                var result =  await _dbConnection.QueryFirstOrDefaultAsync<Employee>(query, param, commandType: System.Data.CommandType.Text);
                    
                return result;
            }
            catch
            {
                    
                throw;
            }
            
            
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentId(Guid departmentId)
        {
            string query = SQLExtension.GetQuery("Employee.GetByDepartmentId");
            var param = new Dictionary<string, object>
            {
                {"@DepartmentID", departmentId }
            };
            return await _dbConnection.QueryAsync<Employee>(query, param, commandType: System.Data.CommandType.Text);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByPositionId(Guid positionId)
        {
            string query = SQLExtension.GetQuery("Employee.GetByPositionId");
            var param = new Dictionary<string, object>
            {
                {"@PositionID", positionId }
            };
            return await _dbConnection.QueryAsync<Employee>(query, param, commandType: System.Data.CommandType.Text);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByFilter(Dictionary<string, object> listField)
        {
            var SqlBuilder = new StringBuilder();
            var JoinClause = new StringBuilder();
            var WhereClase = new StringBuilder(" WHERE e.IsDeleted = 0 ");
            var param = new DynamicParameters();
            SqlBuilder.Append("select ").Append(QueryConfig.EMPLOYEE_FIELD_SEARCH).Append(" from " + QueryConfig.EMPLOYEE_TABLE_NAME + " e ");
            foreach (var (key, value) in listField)
            {
                param.Add(key, value);
                // Join Table
                if (key == "DepartmentID")
                {
                    JoinClause.Append(" join " + QueryConfig.DEPARTMENT_TABLE_NAME + " d on e.DepartmentID = d.DepartmentID ");
                    WhereClase.Append(" AND e." + key + "=@" + key);
                    continue;
                }

                if (key == "PositionID")
                {
                    JoinClause.Append(" join " + QueryConfig.POSITION_TABLE_NAME + " p on e.PositionID = p.PositionID ");
                    WhereClase.Append(" AND e." + key + "=@" + key);
                    continue;
                }

                if (key == "SalaryFrom")
                {
                    WhereClase.Append(" AND Salary between @" + key);
                    continue;
                }

                if (key == "SalaryTo")
                {
                    WhereClase.Append(" AND @" + key);
                    continue;
                }

                if (key == "HireDateFrom")
                {
                    WhereClase.Append(" AND HireDate between @" + key);
                    continue;
                }

                if (key == "HireDateTo")
                {
                    WhereClase.Append(" AND @" + key);
                    continue;
                }

                if (key == "Gender")
                {
                    WhereClase.Append(" AND Gender = @" + key);
                    continue;
                }
            }
            SqlBuilder.Append(JoinClause).Append(WhereClase).Append(";");
            string sql = SqlBuilder.ToString();
            return await _dbConnection.QueryAsync<Employee>(SqlBuilder.ToString(), param);
        }

        /// <summary>
        /// Get total salary of employee, manage transaction manually
        /// </summary>
        /// <returns></returns>
        public async Task<decimal?> GetTotalSalaryEmployee() 
        {
            await OpenConnectionAsync();
            using (var tran = _dbConnection.BeginTransaction(IsolationLevel.ReadCommitted)) 
            {
                try
                {
                    string query = $"select sum(salary) from employee;";
                    decimal? totalSalary = await _dbConnection.QuerySingleAsync<decimal?>(query, transaction: tran,commandType: System.Data.CommandType.Text);
                    tran.Commit();
                    return totalSalary;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        public async Task<int> UpdateEmployeeSalary(string code, decimal newSalary, IDbTransaction trans)
        {
            try
            {
                string query = SQLExtension.GetQuery("Employee.UpdateSalaryByCode");
                var param = new Dictionary<string, object>();
                param.Add("@newSalary", newSalary);
                param.Add("@EmployeeCode", code);
                int updateRow = await _dbConnection.ExecuteAsync(query, param, transaction: trans);
                return updateRow;
            }
            catch
            {  
                throw;
            }
               
            
           
        }

        public async Task<int> GetTotalEmployeeInPosition(string PositionId)
        {
            if (string.IsNullOrWhiteSpace(PositionId))
            {
                throw new Exception("Position Id không hợp lệ");
            }
            string query = SQLExtension.GetQuery("Employee.GetTotalNumberEmployeeInPosition");
            var param = new { PositionId = PositionId };    
            int totalEmployee = await _dbConnection.QuerySingleAsync<int>(query, param, commandType: System.Data.CommandType.Text);
            return totalEmployee;
        }

        private async Task<int> TakeMaxSlotOfPosition(string PositionId)
        {
            string query = "select MaxSlot from position where PositionID = @PositionId";
            var param = new { PositionId = PositionId };
            int maxSlot = await _dbConnection.QuerySingleOrDefaultAsync<int>(query, param, commandType: System.Data.CommandType.Text);
            return maxSlot;
        }

        public async Task<int> UpdateEmployeePosition(string EmployeeCode, string PositionId)
        {
            int NumberEmployeeInPosition = await GetTotalEmployeeInPosition(PositionId);
            int maxSlot = await TakeMaxSlotOfPosition(PositionId);
            if (maxSlot == 0)
            {
                throw new Exception("Tên vị trí không hợp lệ");
            }
            if (NumberEmployeeInPosition >= maxSlot)
            {
                throw new Exception("Số lượng vị trí này đã đầy");
            }
            else
            {
                string query = "update employee set PositionID = @PositionId where EmployeeCode = @EmployeeCode";
                var param = new
                { EmployeeCode = EmployeeCode, PositionId = PositionId }; 
                int rowEffect = await _dbConnection.ExecuteAsync(query, param);
                if (rowEffect == 0)
                {
                    throw new Exception("Không cập nhật data Employee thành công");
                }
                return rowEffect;
            }

        }

        // please using ef core for maintain code
        public Task<int> InsertNewEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<dynamic>> GetEmployeeDynamically(Employee0RequestDTO listParam)
        {
            string sql = SQLExtension.GetQuery("Employee.GetByPagingV2");
            var param = new
            {
                Limit = listParam.PageSize,
                Offset = (listParam.PageNum - 1) * (listParam.PageSize)
                
            };
            return await _dbConnection.QueryAsync<dynamic>(sql, param);
        }
    }
}