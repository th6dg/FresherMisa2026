using Dapper;
using FresherMisa2026.Application.Extensions;
using FresherMisa2026.Application.Interfaces.Repositories;
using FresherMisa2026.Entities.Department;
using FresherMisa2026.Entities.Employee;
using FresherMisa2026.Infrastructure.Repositories.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for Department entity
    /// </summary>
    /// Created By: dvhai (09/04/2026)
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(IConfiguration configuration) : base(configuration)
        {

        }

        /// <summary>
        /// Lấy department theo code
        /// </summary>
        /// <param name="code">Mã department</param>
        /// <returns>Department tìm thấy hoặc null</returns>
        /// CREATED BY: dvhai (09/04/2026)
        public async Task<Department> GetDepartmentByCode(string code)
        {
            string query = SQLExtension.GetQuery("Department.GetByCode");
            var @param = new Dictionary<string, object>
            {
                {"@DepartmentCode", code }
            };
            return await _dbConnection.QueryFirstOrDefaultAsync<Department>(query, @param, commandType: System.Data.CommandType.Text);
        }

        public async Task<IEnumerable<Employee>> GetEmployeeByDepartmentCode(string DepartmentCode)
        {
            var SelectClause = new StringBuilder("select ").Append(QueryConfig.EMPLOYEE_FIELD_SEARCH + " from " + QueryConfig.EMPLOYEE_TABLE_NAME + " e ");
            var JoinClause = new StringBuilder(" join " + QueryConfig.DEPARTMENT_TABLE_NAME + " d on e.DepartmentID = d.DepartmentID ");
            var WhereClause = new StringBuilder(" where d.DepartmentCode=@DepartmentCode");

            string sql = SelectClause.Append(JoinClause).Append(WhereClause).ToString();
            var param = new DynamicParameters();
            param.Add("DepartmentCode", DepartmentCode);
            return await _dbConnection.QueryAsync<Employee>(sql, param);

        }

        public async Task<int> GetNumberOfEmployeeInDepartment(Guid DepartmentID)
        {
            var SelectClause = new StringBuilder("select ").Append(QueryConfig.COUNT_EMPLOYEE + ",d." +QueryConfig.DEPARTMENT_ID +" from " + QueryConfig.EMPLOYEE_TABLE_NAME + " e ");
            var JoinClause = new StringBuilder(" join " + QueryConfig.DEPARTMENT_TABLE_NAME + " d on e.DepartmentID = d.DepartmentID ");
            var WhereClause = new StringBuilder(" where d." + QueryConfig.DEPARTMENT_ID + "=@" + QueryConfig.DEPARTMENT_ID);
            var GroupBy = new StringBuilder(" group by d."+QueryConfig.DEPARTMENT_ID);
            string sql = SelectClause.Append(JoinClause).Append(WhereClause).Append(GroupBy).ToString();
            var param = new DynamicParameters();
            param.Add(QueryConfig.DEPARTMENT_ID, DepartmentID);
            return await _dbConnection.QuerySingleAsync<int>(sql, param);
        }
    }
}
