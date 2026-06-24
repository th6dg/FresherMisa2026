using FresherMisa2026.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Application.Interfaces.Services.Write
{
    public interface IBaseWriteService<TEntity> where TEntity:BaseModel
    {
        public Task<TEntity> AddAsync(TEntity entity);
    }
}
