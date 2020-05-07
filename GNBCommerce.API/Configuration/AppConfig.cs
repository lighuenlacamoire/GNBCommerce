using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GNBCommerce.API.Configuration
{
    public sealed class AppConfig
    {
        public class MongoConnection
        {
            public string ConnectionString { get; set; }
            public string Database { get; set; }
        }
        public class ExternalServices
        {
            public class GNBRates
            {
                public string Endpoint { get; set; }
            }
            public class GNBTransactions
            {
                public string Endpoint { get; set; }
            }
        }

        public class CacheConfiguration
        {
            public class Rates
            {
                public int Time { get; set; }
                public string Key { get; set; }
            }
            public class Transactions
            {
                public int Time { get; set; }
                public string Key { get; set; }
            }
        }
    }
}
