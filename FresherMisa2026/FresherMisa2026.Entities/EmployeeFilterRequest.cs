using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Entities
{
    public class EmployeeFilterRequest
    {
        public string? DepartmentID { get; set; }

        public string? PositionID { get; set; }

        public decimal? SalaryFrom { get; set; }

        public decimal? SalaryTo { get; set; }

        public int? Gender { get; set; }

        public DateTime? HireDateFrom { get; set; }

        public DateTime? HireDateTo { get; set; }

        public static void TrimAllStringField(EmployeeFilterRequest filterRequest)
        {
            if (filterRequest == null) return;

            var properties = filterRequest.GetType().GetProperties();
            foreach (var p in properties)
            {
                // 1. Kiểm tra xem thuộc tính đó có phải là kiểu string không
                if (p.PropertyType == typeof(string))
                {
                    // 2. Lấy giá trị hiện tại
                    var value = (string)p.GetValue(filterRequest);

                    if (value != null)
                    {
                        // 3. Trim và gán ngược lại vào object
                        p.SetValue(filterRequest, value.Trim());
                    }
                }
            }
        }

        public static bool IsValidSalary(EmployeeFilterRequest filterRequest)
        {
            if (filterRequest.SalaryFrom < 0 && filterRequest.SalaryTo <= 0)
            {
                return false;
            }
            if (filterRequest.SalaryFrom > filterRequest.SalaryTo)
            {
                return false;
            }
            return true;
        }

        public static bool IsValidHireDate(EmployeeFilterRequest filterRequest)
        {
            if (filterRequest.HireDateFrom > filterRequest.HireDateTo)
            {
                return false;
            }
            return true;
        }
    }
}
