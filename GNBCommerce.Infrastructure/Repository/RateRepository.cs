using GNBCommerce.Domain.Models;
using GNBCommerce.Infrastructure.Repository.Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;
using System.Linq;
using GNBCommerce.Domain.External;
using System.Threading.Tasks;
using GNBCommerce.Domain.Context;
using MongoDB.Driver;
using GNBCommerce.Domain.Extensions;
using GNBCommerce.Infrastructure.Logger;

namespace GNBCommerce.Infrastructure.Repository
{

    public class RateRepository : BaseRepository<RateEntity>, IRateRepository
    {
        public RateRepository(ILoggerManager loggerManager,IMongoContext context, string endpoint = null) : base(loggerManager,context, endpoint)
        {
        }

        #region Methods
        public override IEnumerable<RateEntity> JsonToClass(string content)
        {
            var Data = base.JsonToClass(content);

            return FillFaults(Data);
        }
        
        #endregion

        #region Functions
        public IEnumerable<RateEntity> FillFaults(IEnumerable<RateEntity> rates)
        {
            List<string> currencies = rates.Select(x => x.From).Distinct().ToList();
            currencies.AddRange(rates.Where(x => !currencies.Contains(x.To)).Select(x => x.To));

            return CreateFaults(rates, currencies);
        }

        private IEnumerable<RateEntity> CreateFaults(IEnumerable<RateEntity> rateDtos, List<string> currencies)
        {
            var list = rateDtos.ToList();

            foreach (var currencySelected in currencies)
            {
                if (rateDtos.Any(x => Functions.StringCompare(x.From, currencySelected)))
                {
                    List<string> currencyFilter = currencies
                        .Where(x => !x.Equals(currencySelected))
                        .Select(y => y).ToList();

                    foreach (var currencyTo in currencyFilter)
                    {
                        if (!rateDtos.Any(x =>
                            Functions.StringCompare(x.From, currencySelected)
                          && Functions.StringCompare(x.To, currencyTo)))
                        {
                            var foundTo = list.FirstOrDefault(x => Functions.StringCompare(x.To, currencyTo));
                            var passTo = list.FirstOrDefault(x => 
                            Functions.StringCompare(x.From, currencySelected) && 
                            Functions.StringCompare(x.To, foundTo.From));

                            var newRate =
                                RateEntity.Create(
                                    currencySelected,
                                    currencyTo,
                                    Functions.ExchangeConversion(foundTo.Rate, passTo.Rate)
                                    );

                            list.Add(newRate);
                        }
                    }

                }
            }

            return list;
        }
        #endregion
    }
}
