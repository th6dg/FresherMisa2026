using FresherMisa2026.Entities.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FresherMisa2026.Entities.Employee.DTO
{
    [ConfigTable("Employee", true, "EmployeeCode")]
    public class EmployeeDTO
    {
        [Key]
        public Guid? EmployeeID { get; set; }

        [IRequired]
        public string EmployeeCode { get; set; }

        [IRequired]
        public string EmployeeName { get; set; }

        public int? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        [IRequired]
        public Guid? DepartmentID { get; set; }

        [IRequired]
        public Guid? PositionID { get; set; }

        public decimal? Salary { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
