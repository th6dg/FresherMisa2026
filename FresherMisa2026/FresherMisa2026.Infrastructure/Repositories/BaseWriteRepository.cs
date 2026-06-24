using FresherMisa2026.Application.Interfaces.Repositories.Write;
using FresherMisa2026.Entities;
using FresherMisa2026.Entities.Employee;
using FresherMisa2026.Entities.Extensions;
using FresherMisa2026.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Infrastructure.Repositories
{
    public class BaseWriteRepository<TEntity> : IBaseWriteRepository<TEntity> where TEntity : BaseModel
    {
        private readonly AppDbContext _dbContext;
        public BaseWriteRepository(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public async Task InsertEntityAsync(TEntity entity)
        {
            // Add entity vào change tracker, no commit 
            await _dbContext.Set<TEntity>().AddAsync(entity);
        }
    }
}
