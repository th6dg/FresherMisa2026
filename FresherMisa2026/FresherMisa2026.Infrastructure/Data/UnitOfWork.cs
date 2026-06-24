using FresherMisa2026.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;  
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        { 
            await _transaction!.CommitAsync(); 
        }

        public async Task RollbackAsync()
        { 
            await _transaction!.RollbackAsync(); 
        }

        public async Task<int> SaveChangesAsync()
        { 
            return await _dbContext.SaveChangesAsync(); 
        }
    }
}
