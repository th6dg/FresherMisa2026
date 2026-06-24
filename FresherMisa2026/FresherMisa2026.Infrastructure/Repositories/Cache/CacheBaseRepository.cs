using FresherMisa2026.Application.Interfaces;
using FresherMisa2026.Entities;
using FresherMisa2026.Infrastructure.Repositories.Config;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FresherMisa2026.Infrastructure.Repositories.Cache
{
    // Base repository add caching behavior (query + cache)
    public class CacheBaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseModel
    {
        // Wrap a class
        protected readonly IBaseRepository<TEntity> _innerRepository;
        // Caching behavior
        protected readonly IMemoryCache _cache;

        public CacheBaseRepository(IBaseRepository<TEntity> innerRepository, IMemoryCache cache)
        {
            this._innerRepository = innerRepository;
            this._cache = cache;
        }

        public IDbTransaction CreateTransaction()
        {
            Console.WriteLine("CREATE TRANSACTION");
            throw new NotImplementedException();
        }

        /// <summary>
        ///  Check cache before query DB
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<int> DeleteAsync(Guid entityId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Entites with cache aside
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<BaseModel>> GetEntitiesAsync()
        {
            // Check cache
            string key = $"GetAll{typeof(TEntity)}";
            if (!_cache.TryGetValue(key, out IEnumerable<TEntity> entities))
            {
                // Query DB
                IEnumerable<BaseModel> result = await _innerRepository.GetEntitiesAsync();
                // Write Cache 5 minutes
                var cacheOption = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                _cache.Set(key, result, cacheOption);
            }
            return entities;
        }

        public Task<TEntity> GetEntityByIDAsync(Guid entityId)
        {
            throw new NotImplementedException();
        }

        public Task<(long Total, IEnumerable<TEntity> Data)> GetFilterPagingAsync(int pageSize, int pageIndex, string search, List<string> searchFields, string sort)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Guid entityId, TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
