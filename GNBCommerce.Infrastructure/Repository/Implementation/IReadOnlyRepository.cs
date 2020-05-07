using System;
using System.Collections.Generic;
using System.Text;

namespace GNBCommerce.Infrastructure.Repository.Implementation
{
    public interface IReadOnlyRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
    }
}
