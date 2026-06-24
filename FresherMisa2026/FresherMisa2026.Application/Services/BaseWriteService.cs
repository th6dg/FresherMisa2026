using FresherMisa2026.Application.Interfaces.Repositories.Write;
using FresherMisa2026.Application.Interfaces.Services.Write;
using FresherMisa2026.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Application.Services
{
    public class BaseWriteService<TEntity> : IBaseWriteService<TEntity> where TEntity : BaseModel
    {
        private readonly IBaseWriteRepository<TEntity> _baseWriteRepository;
        public BaseWriteService(IBaseWriteRepository<TEntity> baseWriteRepository)
        {
            _baseWriteRepository = baseWriteRepository;
        }
        public virtual Task<TEntity> AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
