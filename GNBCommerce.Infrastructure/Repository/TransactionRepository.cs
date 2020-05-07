using GNBCommerce.Domain.Context;
using GNBCommerce.Domain.Extensions;
using GNBCommerce.Domain.External;
using GNBCommerce.Domain.Models;
using GNBCommerce.Infrastructure.Logger;
using GNBCommerce.Infrastructure.Repository.Implementation;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GNBCommerce.Infrastructure.Repository
{
    public class TransactionRepository : BaseRepository<TransactionEntity>, ITransactionRepository
    {
        public TransactionRepository(ILoggerManager loggerManager,IMongoContext context, string endpoint = null) : base(loggerManager,context, endpoint)
        {
        }

        #region Methods
        public IEnumerable<TransactionEntity> GetBySKU(string sku)
        {
            var list = base.All();

            return list.Where(x => Functions.StringCompare(x.SKU, sku));
        }
        #endregion
    }
}
