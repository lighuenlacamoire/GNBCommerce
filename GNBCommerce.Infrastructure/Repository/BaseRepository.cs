using GNBCommerce.Domain.Context;
using GNBCommerce.Domain.External;
using GNBCommerce.Infrastructure.Logger;
using GNBCommerce.Infrastructure.Repository.Implementation;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GNBCommerce.Infrastructure.Repository
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ILoggerManager Logger;
        private IEnumerable<TEntity> Data = null;
        private RestService ExternalService;
        protected readonly IMongoContext Context;
        protected IMongoCollection<TEntity> DbSet;

        protected BaseRepository(
            ILoggerManager loggerManager,
            IMongoContext context,
            string endpoint = null)
        {
            Logger = loggerManager;
            Context = context;
            DbSet = Context.GetCollection<TEntity>(typeof(TEntity).Name);

            if (Data == null || !Data.Any())
            {
                try
                {
                    ExternalService = new RestService(endpoint);

                    Logger.LogInfo("Invoking external service for request " + typeof(TEntity).Name);
                    Logger.LogInfo("Requesting Endpoint: " + endpoint);
                    var response = ExternalService.Invoke();

                    if (!string.IsNullOrEmpty(response.Content) && response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Logger.LogInfo("Invoke Success, formatting data");
                        Data = JsonToClass(response.Content);

                        if (Data != null && Data.Any())
                        {
                            Logger.LogInfo("Deleting old data from DB");
                            DbSet.DeleteMany(u => true);
                            Logger.LogInfo("Inserting new data from external source");
                            DbSet.InsertMany(Data);
                        }
                    }
                    else
                    {
                        Logger.LogInfo("Invoke Failed, recovering data from Db");
                        Data = DbSet.AsQueryable().ToList();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error into constructo: " + ex.Message);
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public virtual IEnumerable<TEntity> JsonToClass(string content)
        {
            return JsonConvert.DeserializeObject<List<TEntity>>(content);
        }

        public virtual IEnumerable<TEntity> All()
        {
            return Data;
        }

        public virtual IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> predicate)
        {
            return Data.AsQueryable().Where(predicate).ToList();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
