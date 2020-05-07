using GNBCommerce.Domain.Context;
using GNBCommerce.Infrastructure.Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GNBCommerce.Infrastructure.Repository
{
    public class MongoUnitOfWork : IMongoUnitOfWork
    {
        private readonly IMongoContext _context;

        public MongoUnitOfWork(IMongoContext context)
        {
            _context = context;
        }
        
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
