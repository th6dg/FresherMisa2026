using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Application.Interfaces.Repositories.Write
{
    // Using EF core for insert, update, delete
    public interface IBaseWriteRepository<TEntity>
    {
        Task InsertEntityAsync(TEntity entity);
    }
}
