using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Configuration;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Cqrs.MediatR;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Database;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.DomainApis.PerformanceApi;
using com.brettnamba.MlbTheShowForecaster.DomainApis.PlayerStatusApi;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Cards.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Forecasts.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using Npgsql;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure;

/// <summary>
/// Registers dependencies for GameCards
/// </summary>
public static class Dependencies
{
    /// <summary>
    /// Configuration keys
    /// </summary>
    public static class ConfigKeys
    {
        /// <summary>
        /// Cards connection string key
        /// </summary>
        public const string CardsConnection = "Cards";

        /// <summary>
        /// Forecasts connection string key
        /// </summary>
        public const string ForecastsConnection = "Forecasts";

        /// <summary>
        /// Marketplace connection string key
        /// </summary>
        public const string MarketplaceConnection = "Marketplace";

        /// <summary>
        /// Trends MongoDB connection string key
        /// </summary>
        public const string TrendsMongoDbConnection = "TrendsMongoDb";

        /// <summary>
        /// <see cref="ListingPriceSignificantChangeThreshold.BuyPricePercentageChangeThreshold"/> config key
        /// </summary>
        public const string BuyPricePercentageChangeThreshold = "CardPriceTracker:BuyPricePercentageChangeThreshold";

        /// <summary>
        /// <see cref="ListingPriceSignificantChangeThreshold.SellPricePercentageChangeThreshold"/> conig key
        /// </summary>
        public const string SellPricePercentageChangeThreshold = "CardPriceTracker:SellPricePercentageChangeThreshold";

        /// <summary>
        /// PlayerStatus API base address config key
        /// </summary>
        public const string PlayerStatusApiBaseAddress = "Forecasting:PlayerMatcher:BaseAddress";

        /// <summary>
        /// Forecast impact durations config key
        /// </summary>
        public const string ImpactDurations = "Forecasting:ImpactDurations";

        /// <summary>
        /// Performance API base address config key
        /// </summary>
        public const string PerformanceApiBaseAddress = "PerformanceApi:BaseAddress";

        /// <summary>
        /// Trend report Mongo DB config
        /// </summary>
        public const string MongoDbTrendReportConfig = "Trends:MongoDb:Config";
    }

    /// <summary>
    /// Registers GameCards domain mapping
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    public static void AddGameCardsMapping(this IServiceCollection services)
    {
        services.TryAddSingleton<IListingMapper, ListingMapper>();
        services.TryAddSingleton<ICalendar, Calendar>();
        services.TryAddSingleton<IPlayerCardMapper, PlayerCardMapper>();
    }

    /// <summary>
    /// Registers GameCards <see cref="PlayerCardTracker"/>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    public static void AddGameCardsPlayerCardTracker(this IServiceCollection services)
    {
        services.AddMediatRCqrs(new List<Assembly>() { typeof(IPlayerCardTracker).Assembly });

        services.TryAddSingleton<IMlbTheShowApiFactory, MlbTheShowApiFactory>();
        services.TryAddSingleton<IMlbTheShowItemMapper, MlbTheShowItemMapper>();
        services.TryAddTransient<ICardCatalog, MlbTheShowApiCardCatalog>();
        services.TryAddTransient<IPlayerCardTracker, PlayerCardTracker>();
    }

    /// <summary>
    /// Registers GameCards <see cref="CardPriceTracker"/>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    /// <param name="config">Configuration with marketplace threshold values</param>
    public static void AddGameCardsPriceTracker(this IServiceCollection services, IConfiguration config)
    {
        services.AddMediatRCqrs(new List<Assembly>() { typeof(ICardPriceTracker).Assembly });

        services.TryAddSingleton<IListingPriceSignificantChangeThreshold>(sp =>
        {
            var buyThreshold = config.GetRequiredValue<decimal>(ConfigKeys.BuyPricePercentageChangeThreshold);
            var sellThreshold = config.GetRequiredValue<decimal>(ConfigKeys.SellPricePercentageChangeThreshold);
            return ListingPriceSignificantChangeThreshold.Create(buyThreshold, sellThreshold);
        });
        services.TryAddSingleton<IMlbTheShowApiFactory, MlbTheShowApiFactory>();
        services.TryAddSingleton<IMlbTheShowListingMapper, MlbTheShowListingMapper>();
        services.TryAddTransient<ICardMarketplace, MlbTheShowApiCardMarketplace>();
        services.TryAddTransient<ICardPriceTracker, CardPriceTracker>();
    }

    /// <summary>
    /// Registers GameCards <see cref="RosterUpdateOrchestrator"/>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    public static void AddGameCardsRosterUpdates(this IServiceCollection services)
    {
        services.AddMediatRCqrs(new List<Assembly>() { typeof(IRosterUpdateOrchestrator).Assembly });

        services.TryAddSingleton<IMlbTheShowApiFactory, MlbTheShowApiFactory>();
        services.TryAddSingleton<IMlbTheShowItemMapper, MlbTheShowItemMapper>();
        services.TryAddSingleton<IMlbTheShowRosterUpdateMapper, MlbTheShowRosterUpdateMapper>();
        services.TryAddSingleton<IMemoryCache>(new MemoryCache(new MemoryCacheOptions()));
        services.TryAddTransient<ICardCatalog, MlbTheShowApiCardCatalog>();
        services.TryAddTransient<IRosterUpdateFeed, MlbTheShowApiRosterUpdateFeed>();
        services.TryAddTransient<IRosterUpdateOrchestrator, RosterUpdateOrchestrator>();
        services.TryAddTransient<IPlayerRatingHistoryService, PlayerRatingHistoryService>();
    }

    /// <summary>
    /// Registers Forecasting services
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    /// <param name="config">Config for forecasting</param>
    public static void AddForecasting(this IServiceCollection services, IConfiguration config)
    {
        services.AddRefitClient<IPlayerStatusApi>(new RefitSettings()
        {
            ContentSerializer = new SystemTextJsonContentSerializer(
                new JsonSerializerOptions()
                {
                    Converters = { new JsonStringEnumConverter() }
                }
            )
        }).ConfigureHttpClient(c => c.BaseAddress = new Uri(config[ConfigKeys.PlayerStatusApiBaseAddress]!));
        services.TryAddSingleton<IPlayerMatcher, PlayerMatcher>();
        services.TryAddSingleton(config.GetRequiredSection(ConfigKeys.ImpactDurations).Get<ForecastImpactDuration>()!);
    }

    /// <summary>
    /// Registers TrendReport services
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    /// <param name="config">Config</param>
    public static void AddTrendReporting(this IServiceCollection services, IConfiguration config)
    {
        services.AddRefitClient<IPerformanceApi>(new RefitSettings()
        {
            ContentSerializer = new SystemTextJsonContentSerializer(
                new JsonSerializerOptions()
                {
                    Converters = { new JsonStringEnumConverter() }
                }
            )
        }).ConfigureHttpClient(c => c.BaseAddress = new Uri(config[ConfigKeys.PerformanceApiBaseAddress]!));

        services.TryAddScoped<ITrendReportFactory, TrendReportFactory>();

        services.TryAddScoped<IMongoClient>(sp =>
            new MongoClient(config.GetRequiredConnectionString(ConfigKeys.TrendsMongoDbConnection)));

        services.TryAddScoped<MongoDbTrendReporter.MongoDbTrendReporterConfig>(sp =>
        {
            const string configKey = ConfigKeys.MongoDbTrendReportConfig;
            return config.GetRequiredValue<MongoDbTrendReporter.MongoDbTrendReporterConfig>(configKey);
        });
        services.TryAddScoped<ITrendReporter, MongoDbTrendReporter>();
    }

    /// <summary>
    /// Registers GameCards EntityFrameworkCore dependencies
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    /// <param name="config">Configuration with connection strings</param>
    public static void AddGameCardsEntityFrameworkCoreRepositories(this IServiceCollection services,
        IConfiguration config)
    {
        // Add EntityFrameworkCore
        services.AddDbContext<CardsDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(config.GetRequiredConnectionString(ConfigKeys.CardsConnection));
        });
        services.AddDbContext<ForecastsDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(config.GetRequiredConnectionString(ConfigKeys.ForecastsConnection));
        });
        services.AddDbContext<MarketplaceDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(config.GetRequiredConnectionString(ConfigKeys.MarketplaceConnection));
        });
        // Add repositories
        services.AddTransient<IPlayerCardRepository, EntityFrameworkCorePlayerCardRepository>();
        services.AddTransient<IForecastRepository, EntityFrameworkCoreForecastRepository>();
        services.AddTransient<IListingRepository, HybridNpgsqlEntityFrameworkCoreListingRepository>();
        // UnitOfWork
        services.AddScoped<IAtomicDatabaseOperation, DbAtomicDatabaseOperation>(sp =>
        {
            var dataSource =
                new NpgsqlDataSourceBuilder(config.GetRequiredConnectionString(ConfigKeys.MarketplaceConnection))
                    .Build();
            return new DbAtomicDatabaseOperation(dataSource);
        });
        services.AddTransient<IUnitOfWork<ICardWork>, UnitOfWork<CardsDbContext>>();
        services.AddTransient<IUnitOfWork<IForecastWork>, UnitOfWork<ForecastsDbContext>>();
        services.AddTransient<IUnitOfWork<IMarketplaceWork>, DbUnitOfWork<MarketplaceDbContext>>();
    }
}