using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Application.CustomException
{
    public class CanNotAddEmployeeException : Exception
    {
        public string EmployeeCode;

        public CanNotAddEmployeeException(string employeeCode) : base($"Mã nhân viên {employeeCode} đã tồn tại")
        {
            this.EmployeeCode = employeeCode;
        }
    }
}
