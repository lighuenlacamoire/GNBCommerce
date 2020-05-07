using GNBCommerce.Domain.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GNBCommerce.Domain.Context
{
    public class MongoContext : IMongoContext
    {
        private readonly IMongoDatabase _database = null;

        public MongoContext(string ConnectionString, string Database)
        {
            var client = new MongoClient(ConnectionString);
            if (client != null)
                _database = client.GetDatabase(Database);
        }
        
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
