using FresherMisa2026.Entities.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FresherMisa2026.Entities.Employee
{
    [ConfigTable("EmployeeFinance", false, "TaxCode")]
    public class EmployeeFinance
    {
        [Key] // Khóa chính kiêm khóa ngoại nối với EmployeeID của bảng Employee
        public Guid EmployeeID { get; set; }

        public decimal? Salary { get; set; }

        public string? TaxCode { get; set; }       // Mã số thuế

        public string? BankAccount { get; set; }   // Số tài khoản

        public string? BankName { get; set; }      // Tên ngân hàng

        public bool IsCustomer { get; set; }       // Là khách hàng (Dạng tích chọn)

        public bool IsSupplier { get; set; }
    }
}
