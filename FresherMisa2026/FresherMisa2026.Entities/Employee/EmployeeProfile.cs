using FresherMisa2026.Entities.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FresherMisa2026.Entities.Employee
{
    [ConfigTable("Employee", true, "IdentityNumber")]
    public class EmployeeProfile
    {
        [Key] // Vừa là Khóa chính, vừa là Khóa ngoại nối với EmployeeID của bảng Employee
        public Guid EmployeeID { get; set; }

        public int? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? IdentityNumber { get; set; } // Số CCCD / CMND

        public DateTime? IdentityDate { get; set; }  // Ngày cấp

        public string? IdentityPlace { get; set; } // Nơi cấp

        public string? PassportNumber { get; set; }
    }
}
