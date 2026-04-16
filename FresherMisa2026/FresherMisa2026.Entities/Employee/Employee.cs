using FresherMisa2026.Entities.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FresherMisa2026.Entities.Employee
{
    [ConfigTable("Employee", true, "EmployeeCode")]
    public class Employee : BaseModel
    {
        [Key]
        public Guid EmployeeID { get; set; }

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
        public Guid DepartmentID { get; set; }

        [IRequired]
        public Guid PositionID { get; set; }

        public decimal? Salary { get; set; }

        public DateTime? CreatedDate { get; set; }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Regex cho email
            string regexPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, regexPattern);
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            // Pattern: Bắt đầu bằng số 0, theo sau là 9 chữ số nữa (tổng 10 số)
            string pattern = @"^0\d{9}$";

            return Regex.IsMatch(phoneNumber, pattern);
        }

        public static bool IsValidDateOfBirth(DateTime? date)
        {
            if (date != null && date > DateTime.Now)
            {
                  return false;
            }
            return true;
        }
    }
}