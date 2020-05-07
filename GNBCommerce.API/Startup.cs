using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GNBCommerce.API.Configuration;
using GNBCommerce.Application.Services;
using GNBCommerce.Application.Services.Implementation;
using GNBCommerce.Domain.Context;
using GNBCommerce.Infrastructure.Logger;
using GNBCommerce.Infrastructure.Repository;
using GNBCommerce.Infrastructure.Repository.Decorators;
using GNBCommerce.Infrastructure.Repository.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using NLog;

namespace GNBCommerce.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            #region Data Configuration
            services.AddMvc()
                .AddViewLocalization()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                 .AddJsonOptions(opt =>
                 {
                     opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                     opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                     opt.SerializerSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
                 });
            #endregion

            services.AddOptions();

            #region AppSettings Configuration
            services.Configure<AppConfig>(Configuration.GetSection(typeof(AppConfig).Name));

            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppConfig>>().Value);
            #endregion

            #region Logger
            services.AddSingleton<ILoggerManager, LoggerManager>();
            #endregion

            #region DbContext
            services.AddScoped<IMongoContext, MongoContext>(
                provider => new MongoContext(
                    Configuration["AppConfig:MongoConnection:ConnectionString"],
                    Configuration["AppConfig:MongoConnection:Database"]));
            #endregion

            #region Unit Of Work
            services.AddScoped<IMongoUnitOfWork, MongoUnitOfWork>();
            #endregion

            #region Repositories
            #region Standard-Repository
            //
            #endregion

            #region Cache-Repository
            services.AddScoped(
                provider => new RateRepository(
                    provider.GetService<ILoggerManager>(),
                    provider.GetService<IMongoContext>(),
                    Configuration["AppConfig:ExternalServices:GNBRates:Endpoint"]));
            services.AddScoped<IRateRepository, CachedRateRepositoryDecorator>(
                provider => new CachedRateRepositoryDecorator(
                    provider.GetService<RateRepository>(),
                    provider.GetService<IMemoryCache>(),
                    Configuration.GetValue<int>("AppConfig:CacheConfiguration:Rates:Time"),
                    Configuration["AppConfig:CacheConfiguration:Rates:Key"]
                ));

            services.AddScoped(
                provider => new TransactionRepository(
                    provider.GetService<ILoggerManager>(),
                    provider.GetService<IMongoContext>(),
                    Configuration["AppConfig:ExternalServices:GNBTransactions:Endpoint"]));
            services.AddScoped<ITransactionRepository, CachedTransactionRepositoryDecorator>(
                provider => new CachedTransactionRepositoryDecorator(
                    provider.GetService<TransactionRepository>(),
                    provider.GetService<IMemoryCache>(),
                    Configuration.GetValue<int>("AppConfig:CacheConfiguration:Transactions:Time"),
                    Configuration["AppConfig:CacheConfiguration:Transactions:Key"]
                ));
            #endregion
            #endregion

            #region Services
            services.AddScoped<IRateService, RateService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IProductService, ProductService>();
            #endregion


            #region Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "GNB Api",
                        Description = "Api con Productos y relaciones",
                        Version = "v1",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Name = "GNB Company",
                            Email = string.Empty,
                            Url = new Uri("https://x.com"),
                        },
                    });
                // XML Documentation
                try
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Swagger Enabled
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "GNB Api");
            });
            #endregion

            app.UseMvc();
        }
    }
}
