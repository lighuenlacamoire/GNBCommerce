using AutoMapper;
using GNBCommerce.Application.Models;
using GNBCommerce.Application.Services.Implementation;
using GNBCommerce.Domain.Extensions;
using GNBCommerce.Domain.Models;
using GNBCommerce.Infrastructure.Logger;
using GNBCommerce.Infrastructure.Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GNBCommerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IMongoUnitOfWork _mongoUnitOfWork;
        private readonly IRateRepository _rateRepository;
        private readonly ITransactionRepository _transactionRepository;

        public ProductService(
            ILoggerManager loggerManager,
            IMongoUnitOfWork mongoUnitOfWork,
            IRateRepository rateRepository,
            ITransactionRepository transactionRepository)
        {
            _logger = loggerManager;
            _mongoUnitOfWork = mongoUnitOfWork;
            _rateRepository = rateRepository;
            _transactionRepository = transactionRepository;

            #region Mapping
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                #region Rates
                #endregion
            });
            _mapper = config.CreateMapper();
            #endregion

        }

        #region Methods
        public ProductDto GetBySKU(string sku, string currency = "EUR")
        {
            _logger.LogInfo("Requesting Product "+sku+" sales at currency "+currency);

            ProductDto product = new ProductDto()
            {
                SKU = sku,
                Currency = currency,
                TotalAmount = "0"
            };

            try
            {
                var transactions =
                _transactionRepository.GetFiltered(x => Functions.StringCompare(x.SKU, sku));

                var rates =
                    _rateRepository.GetFiltered(x => Functions.StringCompare(x.To, currency));


                if (transactions != null && transactions.Any())
                {
                    _logger.LogInfo("Product "+sku+": "+transactions.Count()+ " sales found");
                    GetSalesToCurrency(transactions, rates, ref product);
                }
                else
                {
                    _logger.LogWarn("Was not possible find any sales for product "+sku);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error invoking method: GetBySKU:"+ex.Message);

            }
            return product;
        }

        IEnumerable<ProductDto> IService<ProductDto>.GetAll()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Functions
        private void GetSalesToCurrency(IEnumerable<TransactionEntity> transactionEntities, IEnumerable<RateEntity> rateEntities, ref ProductDto product)
        {
            string currency = product.Currency;
            float totalAmount = 0;

            foreach(var transaction in transactionEntities)
            {
                TransactionDto transactionDto = new TransactionDto();
                transactionDto.SKU = transaction.SKU;
                transactionDto.Currency = transaction.Currency;

                if (Functions.StringCompare(transaction.Currency, currency))
                {
                    transactionDto.Amount = transaction.Amount;
                }
                else
                {

                    var exchange = rateEntities
                        .FirstOrDefault(x =>
                        Functions.StringCompare(x.From, transaction.Currency) 
                      && Functions.StringCompare(x.To, currency));

                    transactionDto.Amount = Functions.ExchangeConversion(transaction.Amount, exchange.Rate);
                    
                }
                totalAmount = totalAmount + float.Parse(transactionDto.Amount);
                product.Transactions.Add(transactionDto);

            }

            product.TotalAmount = string.Format("{0:f2}", totalAmount);
        }
        #endregion
    }
}
