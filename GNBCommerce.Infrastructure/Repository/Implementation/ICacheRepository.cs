using System;
using System.Collections.Generic;
using System.Text;

namespace GNBCommerce.Infrastructure.Repository.Implementation
{
    public interface ICacheRepository<TEntity> where TEntity : class
    {
        //string CacheKey { get; }

        //void InvalidateCache();
        //void InsertIntoCache(TEntity item);
    }
}
