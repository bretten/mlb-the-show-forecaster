using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests;

public class DependenciesTests
{
    [Fact]
    public void AddGameCardsMapping_ServiceCollection_RegistersDependencies()
    {
        // Arrange
        var s = new ServiceCollection();

        // Act
        s.AddGameCardsMapping();
        var actual = s.BuildServiceProvider();

        // Assert
        Assert.Equal(ServiceLifetime.Singleton, s.First(x => x.ServiceType == typeof(ICalendar)).Lifetime);
        Assert.IsType<Calendar>(actual.GetRequiredService<ICalendar>());

        Assert.Equal(ServiceLifetime.Singleton, s.First(x => x.ServiceType == typeof(IPlayerCardMapper)).Lifetime);
        Assert.IsType<PlayerCardMapper>(actual.GetRequiredService<IPlayerCardMapper>());

        Assert.Equal(ServiceLifetime.Singleton, s.First(x => x.ServiceType == typeof(IListingMapper)).Lifetime);
        Assert.IsType<ListingMapper>(actual.GetRequiredService<IListingMapper>());
    }

    [Fact]
    public void AddGameCardsPlayerCardTracker_ServiceCollection_RegistersDependencies()
    {
        // Arrange
        var s = new ServiceCollection();
        var settings = new Dictionary<string, string?>
        {
        };
        var config = GetConfig(settings);

        // Act
        s.AddGameCardsPlayerCardTracker(config);
        var actual = s.BuildServiceProvider();

        // Assert
        Assert.Equal(ServiceLifetime.Singleton, s.First(x => x.ServiceType == typeof(IMlbTheShowApiFactory)).Lifetime);
        Assert.IsType<MlbTheShowApiFactory>(actual.GetRequiredService<IMlbTheShowApiFactory>());

        Assert.Equal(ServiceLifetime.Singleton, s.First(x => x.ServiceType == typeof(IMlbTheShowItemMapper)).Lifetime);
        Assert.IsType<MlbTheShowItemMapper>(actual.GetRequiredService<IMlbTheShowItemMapper>());

        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(ICardCatalog)).Lifetime);
        Assert.IsType<MlbTheShowApiCardCatalog>(actual.GetRequiredService<ICardCatalog>());

        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(IPlayerCardTracker)).Lifetime);
        Assert.IsType<PlayerCardTracker>(actual.GetRequiredService<IPlayerCardTracker>());
    }

    [Fact]
    public void AddGameCardsPriceTracker_ServiceCollection_RegistersDependencies()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { Dependencies.ConfigKeys.BuyPricePercentageChangeThreshold, "1" },
            { Dependencies.ConfigKeys.SellPricePercentageChangeThreshold, "2" },
            { Dependencies.ConfigKeys.UseWebsiteForHistoricalPrices, "false" },
        };
        var config = GetConfig(settings);

        var s = new ServiceCollection();

        // Act
        s.AddSingleton<ICalendar, Calendar>();
        s.AddGameCardsPriceTracker(config);
        var actual = s.BuildServiceProvider();

        // Assert
        var threshold = actual.GetRequiredService<IListingPriceSignificantChangeThreshold>();
        Assert.Equal(ServiceLifetime.Singleton,
            s.First(x => x.ServiceType == typeof(IListingPriceSignificantChangeThreshold)).Lifetime);
        Assert.IsType<ListingPriceSignificantChangeThreshold>(threshold);
        Assert.Equal(1, threshold.BuyPricePercentageChangeThreshold);
        Assert.Equal(2, threshold.SellPricePercentageChangeThreshold);

        Assert.Equal(ServiceLifetime.Singleton, s.First(x => x.ServiceType == typeof(IMlbTheShowApiFactory)).Lifetime);
        Assert.IsType<MlbTheShowApiFactory>(actual.GetRequiredService<IMlbTheShowApiFactory>());

        Assert.Equal(ServiceLifetime.Singleton,
            s.First(x => x.ServiceType == typeof(IMlbTheShowListingMapper)).Lifetime);
        Assert.IsType<MlbTheShowListingMapper>(actual.GetRequiredService<IMlbTheShowListingMapper>());

        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(ICardMarketplace)).Lifetime);
        Assert.IsType<MlbTheShowApiCardMarketplace>(actual.GetRequiredService<ICardMarketplace>());

        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(ICardPriceTracker)).Lifetime);
        Assert.IsType<CardPriceTracker>(actual.GetRequiredService<ICardPriceTracker>());
    }

    [Fact]
    public void AddGameCardsPriceTracker_ServiceCollectionWithWebsitePriceTracker_RegistersDependencies()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { Dependencies.ConfigKeys.BuyPricePercentageChangeThreshold, "1" },
            { Dependencies.ConfigKeys.SellPricePercentageChangeThreshold, "2" },
            { Dependencies.ConfigKeys.UseWebsiteForHistoricalPrices, "true" },
        };
        var config = GetConfig(settings);

        var s = new ServiceCollection();

        // Act
        s.TryAddSingleton<ICalendar, Calendar>();
        s.AddGameCardsPriceTracker(config);
        var actual = s.BuildServiceProvider();

        // Assert
        var threshold = actual.GetRequiredService<IListingPriceSignificantChangeThreshold>();
        Assert.Equal(ServiceLifetime.Singleton,
            s.First(x => x.ServiceType == typeof(IListingPriceSignificantChangeThreshold)).Lifetime);
        Assert.IsType<ListingPriceSignificantChangeThreshold>(threshold);
        Assert.Equal(1, threshold.BuyPricePercentageChangeThreshold);
        Assert.Equal(2, threshold.SellPricePercentageChangeThreshold);

        Assert.Equal(ServiceLifetime.Singleton, s.First(x => x.ServiceType == typeof(IMlbTheShowApiFactory)).Lifetime);
        Assert.IsType<MlbTheShowApiFactory>(actual.GetRequiredService<IMlbTheShowApiFactory>());

        Assert.Equal(ServiceLifetime.Singleton,
            s.First(x => x.ServiceType == typeof(IMlbTheShowListingMapper)).Lifetime);
        Assert.IsType<MlbTheShowListingMapper>(actual.GetRequiredService<IMlbTheShowListingMapper>());

        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(ICardMarketplace)).Lifetime);
        Assert.IsType<MlbTheShowComCardMarketplace>(actual.GetRequiredService<ICardMarketplace>());

        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(ICardPriceTracker)).Lifetime);
        Assert.IsType<CardPriceTracker>(actual.GetRequiredService<ICardPriceTracker>());
    }

    [Fact]
    public void AddGameCardsRosterUpdates_ServiceCollection_RegistersDependencies()
    {
        // Arrange
        var s = new ServiceCollection();
        var settings = new Dictionary<string, string?>
        {
        };
        var config = GetConfig(settings);

        // Act
        s.AddLogging();
        s.AddGameCardsRosterUpdates(config);
        var actual = s.BuildServiceProvider();

        // Assert
        Assert.Equal(ServiceLifetime.Singleton, s.First(x => x.ServiceType == typeof(IMlbTheShowApiFactory)).Lifetime);
        Assert.IsType<MlbTheShowApiFactory>(actual.GetRequiredService<IMlbTheShowApiFactory>());

        Assert.Equal(ServiceLifetime.Singleton, s.First(x => x.ServiceType == typeof(IMlbTheShowItemMapper)).Lifetime);
        Assert.IsType<MlbTheShowItemMapper>(actual.GetRequiredService<IMlbTheShowItemMapper>());

        Assert.Equal(ServiceLifetime.Singleton,
            s.First(x => x.ServiceType == typeof(IMlbTheShowRosterUpdateMapper)).Lifetime);
        Assert.IsType<MlbTheShowRosterUpdateMapper>(actual.GetRequiredService<IMlbTheShowRosterUpdateMapper>());

        Assert.Equal(ServiceLifetime.Singleton, s.First(x => x.ServiceType == typeof(IMemoryCache)).Lifetime);
        Assert.IsType<MemoryCache>(actual.GetRequiredService<IMemoryCache>());

        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(ICardCatalog)).Lifetime);
        Assert.IsType<MlbTheShowApiCardCatalog>(actual.GetRequiredService<ICardCatalog>());

        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(IRosterUpdateFeed)).Lifetime);
        Assert.IsType<MlbTheShowApiRosterUpdateFeed>(actual.GetRequiredService<IRosterUpdateFeed>());

        Assert.Equal(ServiceLifetime.Transient,
            s.First(x => x.ServiceType == typeof(IRosterUpdateOrchestrator)).Lifetime);
        Assert.IsType<RosterUpdateOrchestrator>(actual.GetRequiredService<IRosterUpdateOrchestrator>());

        Assert.Equal(ServiceLifetime.Transient,
            s.First(x => x.ServiceType == typeof(IPlayerRatingHistoryService)).Lifetime);
        Assert.IsType<PlayerRatingHistoryService>(actual.GetRequiredService<IPlayerRatingHistoryService>());
    }

    [Fact]
    public void AddForecasting_ServiceCollection_RegistersDependencies()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { Dependencies.ConfigKeys.PlayerStatusApiBaseAddress, "http://localhost" },
            { $"{Dependencies.ConfigKeys.ImpactDurations}:Boost", "1" },
            { $"{Dependencies.ConfigKeys.ImpactDurations}:BattingStatsChange", "2" },
            { $"{Dependencies.ConfigKeys.ImpactDurations}:PitchingStatsChange", "3" },
            { $"{Dependencies.ConfigKeys.ImpactDurations}:FieldingStatsChange", "4" },
            { $"{Dependencies.ConfigKeys.ImpactDurations}:OverallRatingChange", "5" },
            { $"{Dependencies.ConfigKeys.ImpactDurations}:PositionChange", "6" },
            { $"{Dependencies.ConfigKeys.ImpactDurations}:PlayerActivation", "7" },
            { $"{Dependencies.ConfigKeys.ImpactDurations}:PlayerDeactivation", "8" },
            { $"{Dependencies.ConfigKeys.ImpactDurations}:PlayerFreeAgency", "9" },
            { $"{Dependencies.ConfigKeys.ImpactDurations}:PlayerTeamSigning", "10" }
        };
        var config = GetConfig(settings);
        var s = new ServiceCollection();

        // Act
        s.AddLogging();
        s.AddForecasting(config);
        var actual = s.BuildServiceProvider();

        // Assert
        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(IPlayerStatusApi)).Lifetime);
        Assert.IsAssignableFrom<IPlayerStatusApi>(actual.GetRequiredService<IPlayerStatusApi>());

        Assert.Equal(ServiceLifetime.Singleton, s.First(x => x.ServiceType == typeof(IPlayerMatcher)).Lifetime);
        Assert.IsType<PlayerMatcher>(actual.GetRequiredService<IPlayerMatcher>());

        var durations = actual.GetRequiredService<ForecastImpactDuration>();
        Assert.Equal(ServiceLifetime.Singleton, s.First(x => x.ServiceType == typeof(ForecastImpactDuration)).Lifetime);
        Assert.IsType<ForecastImpactDuration>(durations);
        Assert.Equal(1, durations.Boost);
        Assert.Equal(2, durations.BattingStatsChange);
        Assert.Equal(3, durations.PitchingStatsChange);
        Assert.Equal(4, durations.FieldingStatsChange);
        Assert.Equal(5, durations.OverallRatingChange);
        Assert.Equal(6, durations.PositionChange);
        Assert.Equal(7, durations.PlayerActivation);
        Assert.Equal(8, durations.PlayerDeactivation);
        Assert.Equal(9, durations.PlayerFreeAgency);
        Assert.Equal(10, durations.PlayerTeamSigning);
    }

    [Fact]
    public void AddTrendReporting_ServiceCollection_RegistersDependencies()
    {
        // Arrange
        const string cs = "Server=localhost;Port=5432;Database=test;Uid=postgres;Pwd=postgres;";
        var settings = new Dictionary<string, string?>
        {
            { $"ConnectionStrings:{Dependencies.ConfigKeys.CardsConnection}", cs },
            { $"ConnectionStrings:{Dependencies.ConfigKeys.ForecastsConnection}", cs },
            { $"ConnectionStrings:{Dependencies.ConfigKeys.MarketplaceConnection}", cs },
            { Dependencies.ConfigKeys.PerformanceApiBaseAddress, "http://localhost" },
            {
                $"ConnectionStrings:{Dependencies.ConfigKeys.TrendsMongoDbConnection}",
                "mongodb://u:p@localhost:27017/?authSource=admin"
            },
            { $"{Dependencies.ConfigKeys.MongoDbTrendReportConfig}:Database", "local" },
            { $"{Dependencies.ConfigKeys.MongoDbTrendReportConfig}:Collection", "trends" }
        };
        var config = GetConfig(settings);
        var s = new ServiceCollection();

        // Act
        s.AddLogging();
        s.TryAddSingleton<ICalendar, Calendar>();
        var mockPlayerMatcher = Mock.Of<IPlayerMatcher>();
        s.TryAddSingleton(mockPlayerMatcher);
        s.AddGameCardsEntityFrameworkCoreRepositories(config);
        s.AddTrendReporting(config);
        var actual = s.BuildServiceProvider();

        // Assert
        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(IPerformanceApi)).Lifetime);
        Assert.IsAssignableFrom<IPerformanceApi>(actual.GetRequiredService<IPerformanceApi>());

        Assert.Equal(ServiceLifetime.Scoped, s.First(x => x.ServiceType == typeof(ITrendReportFactory)).Lifetime);
        Assert.IsType<TrendReportFactory>(actual.GetRequiredService<ITrendReportFactory>());

        Assert.Equal(ServiceLifetime.Scoped, s.First(x => x.ServiceType == typeof(IMongoClient)).Lifetime);
        Assert.IsType<MongoClient>(actual.GetRequiredService<IMongoClient>());

        Assert.Equal(ServiceLifetime.Scoped,
            s.First(x => x.ServiceType == typeof(MongoDbTrendReporter.MongoDbTrendReporterConfig)).Lifetime);
        Assert.IsType<MongoDbTrendReporter.MongoDbTrendReporterConfig>(
            actual.GetRequiredService<MongoDbTrendReporter.MongoDbTrendReporterConfig>());
        var mongoConfig = actual.GetRequiredService<MongoDbTrendReporter.MongoDbTrendReporterConfig>();
        Assert.Equal("local", mongoConfig.Database);
        Assert.Equal("trends", mongoConfig.Collection);

        Assert.Equal(ServiceLifetime.Scoped, s.First(x => x.ServiceType == typeof(ITrendReporter)).Lifetime);
        Assert.IsType<MongoDbTrendReporter>(actual.GetRequiredService<ITrendReporter>());
    }

    [Fact]
    public void AddGameCardsEntityFrameworkCoreRepositories_ServiceCollection_RegistersDependencies()
    {
        // Arrange
        const string cs = "Server=localhost;Port=5432;Database=test;Uid=postgres;Pwd=postgres;";
        var settings = new Dictionary<string, string?>
        {
            { $"ConnectionStrings:{Dependencies.ConfigKeys.CardsConnection}", cs },
            { $"ConnectionStrings:{Dependencies.ConfigKeys.ForecastsConnection}", cs },
            { $"ConnectionStrings:{Dependencies.ConfigKeys.MarketplaceConnection}", cs },
        };
        var config = GetConfig(settings);

        var s = new ServiceCollection();
        s.AddSingleton(Mock.Of<IDomainEventDispatcher>());

        // Act
        s.AddGameCardsEntityFrameworkCoreRepositories(config);
        var actual = s.BuildServiceProvider();

        // Assert
        Assert.Equal(ServiceLifetime.Scoped, s.First(x => x.ServiceType == typeof(CardsDbContext)).Lifetime);
        Assert.IsType<CardsDbContext>(actual.GetRequiredService<CardsDbContext>());
        Assert.Equal(ServiceLifetime.Scoped, s.First(x => x.ServiceType == typeof(ForecastsDbContext)).Lifetime);
        Assert.IsType<ForecastsDbContext>(actual.GetRequiredService<ForecastsDbContext>());
        Assert.Equal(ServiceLifetime.Scoped, s.First(x => x.ServiceType == typeof(MarketplaceDbContext)).Lifetime);
        Assert.IsType<MarketplaceDbContext>(actual.GetRequiredService<MarketplaceDbContext>());

        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(IPlayerCardRepository)).Lifetime);
        Assert.IsType<EntityFrameworkCorePlayerCardRepository>(actual.GetRequiredService<IPlayerCardRepository>());
        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(IForecastRepository)).Lifetime);
        Assert.IsType<EntityFrameworkCoreForecastRepository>(actual.GetRequiredService<IForecastRepository>());
        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(IListingRepository)).Lifetime);
        Assert.IsType<HybridNpgsqlEntityFrameworkCoreListingRepository>(
            actual.GetRequiredService<IListingRepository>());

        Assert.Equal(ServiceLifetime.Scoped, s.First(x => x.ServiceType == typeof(IAtomicDatabaseOperation)).Lifetime);
        Assert.IsType<DbAtomicDatabaseOperation>(actual.GetRequiredService<IAtomicDatabaseOperation>());
        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(IUnitOfWork<ICardWork>)).Lifetime);
        Assert.IsType<UnitOfWork<CardsDbContext>>(actual.GetRequiredService<IUnitOfWork<ICardWork>>());
        Assert.Equal(ServiceLifetime.Transient,
            s.First(x => x.ServiceType == typeof(IUnitOfWork<IForecastWork>)).Lifetime);
        Assert.IsType<UnitOfWork<ForecastsDbContext>>(actual.GetRequiredService<IUnitOfWork<IForecastWork>>());
        Assert.Equal(ServiceLifetime.Transient,
            s.First(x => x.ServiceType == typeof(IUnitOfWork<IMarketplaceWork>)).Lifetime);
        Assert.IsType<DbUnitOfWork<MarketplaceDbContext>>(actual.GetRequiredService<IUnitOfWork<IMarketplaceWork>>());
    }

    private static IConfiguration GetConfig(Dictionary<string, string?> settings)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
    }
}