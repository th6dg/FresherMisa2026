using FresherMisa2026.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Application
{
    public class ValidateException : Exception
    {
        // Danh sách chứa các đối tượng lỗi (ValidationError)
        public List<ValidationError> Errors { get; set; }

        // Constructor nhận vào một List
        public ValidateException(List<ValidationError> errors) : base("Dữ liệu không hợp lệ.")
        {
            Errors = errors;
        }

        // Constructor nhận vào một lỗi lẻ (tiện ích)
        public ValidateException(ValidationError error) : base(error.Message)
        {
            Errors = new List<ValidationError> { error };
        }
    }
}
