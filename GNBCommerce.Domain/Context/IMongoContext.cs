using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GNBCommerce.Domain.Context
{
    public interface IMongoContext : IDisposable
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
