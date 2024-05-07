using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Repositories;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Constants = com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Constants;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Tests;

public class DependenciesTests
{
    [Fact]
    public void AddPerformanceMapping_ServiceCollection_RegistersDependencies()
    {
        // Arrange
        var s = new ServiceCollection();

        // Act
        s.AddPerformanceMapping();
        var actual = s.BuildServiceProvider();

        // Assert
        Assert.Equal(ServiceLifetime.Singleton, s.First(x => x.ServiceType == typeof(IPlayerSeasonMapper)).Lifetime);
        Assert.IsType<PlayerSeasonMapper>(actual.GetRequiredService<IPlayerSeasonMapper>());
    }

    [Fact]
    public void AddPerformancePlayerSeasonScorekeeper_ServiceCollection_RegistersDependencies()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { Dependencies.ConfigKeys.StatPercentChangeThreshold, "1.2" },
            { Dependencies.ConfigKeys.MinimumPlateAppearances, "2" },
            { Dependencies.ConfigKeys.MinimumInningsPitched, "3" },
            { Dependencies.ConfigKeys.MinimumBattersFaced, "4" },
            { Dependencies.ConfigKeys.MinimumTotalChances, "5" },
        };
        var config = GetConfig(settings);

        var s = new ServiceCollection();

        // Act
        s.AddPerformancePlayerSeasonScorekeeper(config);
        var actual = s.BuildServiceProvider();

        // Assert
        var requirements = actual.GetRequiredService<IPerformanceAssessmentRequirements>();
        Assert.Equal(ServiceLifetime.Singleton,
            s.First(x => x.ServiceType == typeof(IPerformanceAssessmentRequirements)).Lifetime);
        Assert.IsType<PerformanceAssessmentRequirements>(requirements);
        Assert.Equal(1.2m, requirements.StatPercentChangeThreshold);
        Assert.Equal(2, requirements.MinimumPlateAppearances.Value);
        Assert.Equal(3, requirements.MinimumInningsPitched.Value);
        Assert.Equal(4, requirements.MinimumBattersFaced.Value);
        Assert.Equal(5, requirements.MinimumTotalChances.Value);

        Assert.Equal(ServiceLifetime.Singleton,
            s.First(x => x.ServiceType == typeof(IPlayerSeasonScorekeeper)).Lifetime);
        Assert.IsType<PlayerSeasonScorekeeper>(actual.GetRequiredService<IPlayerSeasonScorekeeper>());
    }

    [Fact]
    public void AddPerformanceTracker_ServiceCollection_RegistersDependencies()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { Dependencies.ConfigKeys.MlbApiBaseAddress, Constants.BaseUrl }
        };
        var config = GetConfig(settings);

        var s = new ServiceCollection();

        // Act
        s.AddPerformanceTracker(config);
        var actual = s.BuildServiceProvider();

        // Assert
        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(IMlbApi)).Lifetime);
        Assert.IsAssignableFrom<IMlbApi>(actual.GetRequiredService<IMlbApi>());

        Assert.Equal(ServiceLifetime.Singleton,
            s.First(x => x.ServiceType == typeof(IMlbApiPlayerStatsMapper)).Lifetime);
        Assert.IsType<MlbApiPlayerStatsMapper>(actual.GetRequiredService<IMlbApiPlayerStatsMapper>());

        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(IPlayerStats)).Lifetime);
        Assert.IsType<MlbApiPlayerStats>(actual.GetRequiredService<IPlayerStats>());

        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(IPerformanceTracker)).Lifetime);
        Assert.IsType<PerformanceTracker>(actual.GetRequiredService<IPerformanceTracker>());
    }

    [Fact]
    public void AddPerformanceEntityFrameworkCoreRepositories_ServiceCollection_RegistersDependencies()
    {
        // Arrange
        const string cs = "Server=localhost;Port=5432;Database=test;Uid=postgres;Pwd=postgres;";
        var settings = new Dictionary<string, string?>
        {
            { $"ConnectionStrings:{Dependencies.ConfigKeys.PlayerSeasonsConnection}", cs }
        };
        var config = GetConfig(settings);

        var s = new ServiceCollection();
        s.AddSingleton(Mock.Of<IDomainEventDispatcher>());

        // Act
        s.AddPerformanceEntityFrameworkCoreRepositories(config);
        var actual = s.BuildServiceProvider();

        // Assert
        Assert.Equal(ServiceLifetime.Scoped, s.First(x => x.ServiceType == typeof(PlayerSeasonsDbContext)).Lifetime);
        Assert.IsType<PlayerSeasonsDbContext>(actual.GetRequiredService<PlayerSeasonsDbContext>());

        Assert.Equal(ServiceLifetime.Transient,
            s.First(x => x.ServiceType == typeof(IPlayerStatsBySeasonRepository)).Lifetime);
        Assert.IsType<EntityFrameworkCorePlayerStatsBySeasonRepository>(
            actual.GetRequiredService<IPlayerStatsBySeasonRepository>());

        Assert.Equal(ServiceLifetime.Transient,
            s.First(x => x.ServiceType == typeof(IUnitOfWork<IPlayerSeasonWork>)).Lifetime);
        Assert.IsType<UnitOfWork<PlayerSeasonsDbContext>>(actual.GetRequiredService<IUnitOfWork<IPlayerSeasonWork>>());
    }

    private static IConfiguration GetConfig(Dictionary<string, string?> settings)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
    }
}