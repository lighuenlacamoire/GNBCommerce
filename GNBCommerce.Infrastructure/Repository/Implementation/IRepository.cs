using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GNBCommerce.Infrastructure.Repository.Implementation
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> JsonToClass(string content);
        IEnumerable<TEntity> All();
        IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> predicate);
    }
}
