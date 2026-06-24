using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Application.CustomException
{
    public class TestException: Exception
    {
        public string msg { get; set; }

        public TestException() : base($"Don't worry, this is Test Exception in Development Environment")
        {
           
        }

        public TestException(string msg) : base(msg)
        {
            this.msg = msg;
        }
    }
}
