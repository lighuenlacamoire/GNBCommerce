using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace GNBCommerce.Domain.Models
{
    /// <summary>
    /// Rate
    /// </summary>
    [BsonIgnoreExtraElements]
    public class RateEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        #region Properties
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("rate")]
        public string Rate { get; set; }
        #endregion

        #region Methods
        public static RateEntity Create(string _from, string _to, string _rate)
        {
            RateEntity rate = new RateEntity()
            {
                From = _from,
                To = _to,
                Rate = _rate,
            };

            return rate;
        }

        #endregion
    }
}
