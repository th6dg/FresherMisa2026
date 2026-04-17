using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FresherMisa2026.Infrastructure.utils
{
    public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid value)
        {
            parameter.Value = value.ToString();
        }

        public override Guid Parse(object value)
        {
            if (value == null || value == DBNull.Value)
                return Guid.Empty;
            string guidString = value.ToString().Trim();
            if (Guid.TryParse(guidString, out Guid result))
            {
                return result;
            }

            return Guid.Empty;
        }
    }
}
