using AutoMapper;
using GNBCommerce.Infrastructure.Logger;
using GNBCommerce.Application.Models;
using GNBCommerce.Application.Services.Implementation;
using GNBCommerce.Domain.Models;
using GNBCommerce.Infrastructure.Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GNBCommerce.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IMongoUnitOfWork _mongoUnitOfWork;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(
            ILoggerManager loggerManager,
            IMongoUnitOfWork mongoUnitOfWork,
            ITransactionRepository transactionRepository)
        {
            _logger = loggerManager;
            _mongoUnitOfWork = mongoUnitOfWork;
            _transactionRepository = transactionRepository;

            #region Mapping
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                #region Rates
                cfg.CreateMap<TransactionEntity, TransactionDto>()
                .ForMember(
                    dest => dest.SKU,
                    src => src.MapFrom(x => x.SKU))
                .ForMember(
                    dest => dest.Amount,
                    src => src.MapFrom(x => x.Amount))
                .ForMember(
                    dest => dest.Currency,
                    src => src.MapFrom(x => x.Currency))
                .ReverseMap();
                #endregion
            });
            _mapper = config.CreateMapper();
            #endregion

        }

        public IEnumerable<TransactionDto> GetAll()
        {
            try
            {
                var transactions = _transactionRepository.All();

                if (transactions == null && !transactions.Any())
                {
                    _logger.LogWarn("Was not possible find any transaction");
                }
                return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error recovering every transactions");
                throw;
            }
        }
    }
}
