using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FresherMisa2026.Infrastructure.utils
{
    public class GuidTypeHandler : SqlMapper.ITypeHandler
    {
        public void SetValue(IDbDataParameter parameter, object value)
        {
            parameter.Value = value;
        }

        public object Parse(Type destinationType, object value)
        {
            if (value is null)
            {
                return null;
            }

            if  (value is Guid guid)
            {
                return guid;
            }
            if (value is string str)
            {
                return new Guid(str);
            }
            return Guid.Parse(value?.ToString() ?? Guid.Empty.ToString());        }
    }
}
