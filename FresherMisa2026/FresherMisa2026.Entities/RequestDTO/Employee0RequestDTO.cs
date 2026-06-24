using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Entities.RequestDTO
{
    public class Employee0RequestDTO
    {
        public int PageSize { get; set; }

        public int PageNum { get; set; }

        public List<string> VisibleColumns { get; set; }
    }
}
