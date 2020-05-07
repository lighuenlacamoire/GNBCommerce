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
    public class RateService : IRateService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IMongoUnitOfWork _mongoUnitOfWork;
        private readonly IRateRepository _rateRepository;

        public RateService(
            ILoggerManager loggerManager,
            IMongoUnitOfWork mongoUnitOfWork,
            IRateRepository rateRepository)
        {
            _logger = loggerManager;
            _mongoUnitOfWork = mongoUnitOfWork;
            _rateRepository = rateRepository;

            #region Mapping
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                #region Rates
                cfg.CreateMap<RateEntity, RateDto>()
                .ForMember(
                    dest => dest.CurrencyFrom,
                    src => src.MapFrom(x => x.From))
                .ForMember(
                    dest => dest.CurrencyTo,
                    src => src.MapFrom(x => x.To))
                .ForMember(
                    dest => dest.Conversion,
                    src => src.MapFrom(x => x.Rate))
                .ReverseMap();
                #endregion
            });
            _mapper = config.CreateMapper();
            #endregion

        }

        public IEnumerable<RateDto> GetAll()
        {
            try
            {
                var rates = _rateRepository.All();
                if(rates == null && !rates.Any())
                {
                    _logger.LogWarn("Was not possible find any rates");
                }

                return _mapper.Map<IEnumerable<RateDto>>(rates);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error recovering every rates");
                throw;
            }
        }
    }
}
