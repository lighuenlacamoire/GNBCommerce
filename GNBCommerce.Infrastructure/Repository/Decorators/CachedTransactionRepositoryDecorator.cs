using GNBCommerce.Domain.Extensions;
using GNBCommerce.Domain.Models;
using GNBCommerce.Infrastructure.Repository.Implementation;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GNBCommerce.Infrastructure.Repository.Decorators
{
    public class CachedTransactionRepositoryDecorator : ITransactionRepository
    {
        private readonly ITransactionRepository _repository;
        private readonly IMemoryCache _cache;
        private string ModelCacheKey;
        private MemoryCacheEntryOptions cacheOptions;

        public CachedTransactionRepositoryDecorator(
            ITransactionRepository repository,
            IMemoryCache cache,
            int time,
            string key)
        {
            _repository = repository;
            _cache = cache;
            ModelCacheKey = key;

            cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(relative: TimeSpan.FromHours(time));
        }

        #region Methods
        public IEnumerable<TransactionEntity> All()
        {
            return _cache.GetOrCreate(ModelCacheKey, entry =>
            {
                entry.SetOptions(cacheOptions);
                return _repository.All();
            });
        }
        
        public IEnumerable<TransactionEntity> GetFiltered(Expression<Func<TransactionEntity, bool>> predicate)
        {
            return this.All().AsQueryable().Where(predicate).ToList();
        }

        public IEnumerable<TransactionEntity> JsonToClass(string content)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
