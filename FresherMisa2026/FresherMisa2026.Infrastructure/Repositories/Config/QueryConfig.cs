using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Infrastructure.Repositories.Config
{
    public class QueryConfig
    {
        public static string COUNT_EMPLOYEE = "count(*)";
        public static string DEPARTMENT_ID = "DepartmentID";
        public static string EMPLOYEE_FIELD_SEARCH = "e.*";
        public static string EMPLOYEE_TABLE_NAME = "employee";
        public static string DEPARTMENT_TABLE_NAME = "department";
        public static string POSITION_TABLE_NAME = "position";
    }
}
