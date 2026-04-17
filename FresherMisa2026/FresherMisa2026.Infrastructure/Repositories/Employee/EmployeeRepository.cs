using Dapper;
using FresherMisa2026.Application.Extensions;
using FresherMisa2026.Application.Interfaces.Repositories;
using FresherMisa2026.Entities.Employee;
using FresherMisa2026.Entities.Employee.DTO;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Infrastructure.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        private string FIELD_SEARCH = "e.*";
        private string EMPLOYEE_TABLE_NAME = "employee";
        private string DEPARTMENT_TABLE_NAME = "department";
        private string POSITION_TABLE_NAME = "position";
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Employee> GetEmployeeByCode(string code)
        {
            string query = SQLExtension.GetQuery("Employee.GetByCode");
            var param = new Dictionary<string, object>
            {
                {"@EmployeeCode", code }
            };
            return await _dbConnection.QueryFirstOrDefaultAsync<Employee>(query, param, commandType: System.Data.CommandType.Text);
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
            SqlBuilder.Append("select ").Append(FIELD_SEARCH).Append(" from " + EMPLOYEE_TABLE_NAME + " e ");
            foreach (var (key, value) in listField)
            {
                param.Add(key, value);
                // Join Table
                if (key == "DepartmentID")
                {
                    JoinClause.Append(" join " + DEPARTMENT_TABLE_NAME + " d on e.DepartmentID = d.DepartmentID ");
                    WhereClase.Append(" AND e." + key + "=@" + key);
                    continue;
                }

                if (key == "PositionID")
                {
                    JoinClause.Append(" join " + POSITION_TABLE_NAME + " p on e.PositionID = p.PositionID ");
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
    }
}