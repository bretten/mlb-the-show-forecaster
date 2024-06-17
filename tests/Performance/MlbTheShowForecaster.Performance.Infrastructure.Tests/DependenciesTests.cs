using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization;
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
    public void AddPerformanceAssessment_ServiceCollection_RegistersDependencies()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { Dependencies.ConfigKeys.ScorePercentageChangeThreshold, "25" },

            // Batting criteria[0]
            { Dependencies.ConfigKeys.MinMaxNormBattingCriteria + ":0:Name", "Hits" },
            { Dependencies.ConfigKeys.MinMaxNormBattingCriteria + ":0:Weight", "0.9" },
            { Dependencies.ConfigKeys.MinMaxNormBattingCriteria + ":0:IsLowerValueBetter", "false" },
            { Dependencies.ConfigKeys.MinMaxNormBattingCriteria + ":0:Min", "10" },
            { Dependencies.ConfigKeys.MinMaxNormBattingCriteria + ":0:Max", "100" },
            // Batting criteria[1]
            { Dependencies.ConfigKeys.MinMaxNormBattingCriteria + ":1:Name", "Strikeouts" },
            { Dependencies.ConfigKeys.MinMaxNormBattingCriteria + ":1:Weight", "0.1" },
            { Dependencies.ConfigKeys.MinMaxNormBattingCriteria + ":1:IsLowerValueBetter", "true" },
            { Dependencies.ConfigKeys.MinMaxNormBattingCriteria + ":1:Min", "5" },
            { Dependencies.ConfigKeys.MinMaxNormBattingCriteria + ":1:Max", "50" },

            // Pitching criteria[0]
            { Dependencies.ConfigKeys.MinMaxNormPitchingCriteria + ":0:Name", "Wins" },
            { Dependencies.ConfigKeys.MinMaxNormPitchingCriteria + ":0:Weight", "0.6" },
            { Dependencies.ConfigKeys.MinMaxNormPitchingCriteria + ":0:IsLowerValueBetter", "false" },
            { Dependencies.ConfigKeys.MinMaxNormPitchingCriteria + ":0:Min", "2" },
            { Dependencies.ConfigKeys.MinMaxNormPitchingCriteria + ":0:Max", "20" },
            // Pitching criteria[1]
            { Dependencies.ConfigKeys.MinMaxNormPitchingCriteria + ":1:Name", "Losses" },
            { Dependencies.ConfigKeys.MinMaxNormPitchingCriteria + ":1:Weight", "0.4" },
            { Dependencies.ConfigKeys.MinMaxNormPitchingCriteria + ":1:IsLowerValueBetter", "true" },
            { Dependencies.ConfigKeys.MinMaxNormPitchingCriteria + ":1:Min", "3" },
            { Dependencies.ConfigKeys.MinMaxNormPitchingCriteria + ":1:Max", "30" },

            // Fielding criteria[0]
            { Dependencies.ConfigKeys.MinMaxNormFieldingCriteria + ":0:Name", "Errors" },
            { Dependencies.ConfigKeys.MinMaxNormFieldingCriteria + ":0:Weight", "0.2" },
            { Dependencies.ConfigKeys.MinMaxNormFieldingCriteria + ":0:IsLowerValueBetter", "true" },
            { Dependencies.ConfigKeys.MinMaxNormFieldingCriteria + ":0:Min", "1" },
            { Dependencies.ConfigKeys.MinMaxNormFieldingCriteria + ":0:Max", "10" },
            // Fielding criteria[1]
            { Dependencies.ConfigKeys.MinMaxNormFieldingCriteria + ":1:Name", "Putouts" },
            { Dependencies.ConfigKeys.MinMaxNormFieldingCriteria + ":1:Weight", "0.8" },
            { Dependencies.ConfigKeys.MinMaxNormFieldingCriteria + ":1:IsLowerValueBetter", "false" },
            { Dependencies.ConfigKeys.MinMaxNormFieldingCriteria + ":1:Min", "8" },
            { Dependencies.ConfigKeys.MinMaxNormFieldingCriteria + ":1:Max", "80" },
        };
        var config = GetConfig(settings);

        var s = new ServiceCollection();

        // Act
        s.AddPerformanceAssessment(config);
        var actual = s.BuildServiceProvider();

        /*
         * Assert
         */
        // Normalization criteria
        var criteria = actual.GetRequiredService<MinMaxNormalizationCriteria>();
        Assert.Equal(ServiceLifetime.Singleton,
            s.First(x => x.ServiceType == typeof(MinMaxNormalizationCriteria)).Lifetime);
        Assert.IsType<MinMaxNormalizationCriteria>(criteria);
        Assert.Equal(25m, criteria.ScorePercentageChangeThreshold);

        // Batting criteria[0]
        var battingCriteria0 = criteria.BattingCriteria[0];
        Assert.Equal("Hits", battingCriteria0.Name);
        Assert.Equal(0.9m, battingCriteria0.Weight);
        Assert.False(battingCriteria0.IsLowerValueBetter);
        Assert.Equal(10, battingCriteria0.Min);
        Assert.Equal(100, battingCriteria0.Max);
        // Batting criteria[1]
        var battingCriteria1 = criteria.BattingCriteria[1];
        Assert.Equal("Strikeouts", battingCriteria1.Name);
        Assert.Equal(0.1m, battingCriteria1.Weight);
        Assert.True(battingCriteria1.IsLowerValueBetter);
        Assert.Equal(5, battingCriteria1.Min);
        Assert.Equal(50, battingCriteria1.Max);

        // Pitching criteria[0]
        var pitchingCriteria0 = criteria.PitchingCriteria[0];
        Assert.Equal("Wins", pitchingCriteria0.Name);
        Assert.Equal(0.6m, pitchingCriteria0.Weight);
        Assert.False(pitchingCriteria0.IsLowerValueBetter);
        Assert.Equal(2, pitchingCriteria0.Min);
        Assert.Equal(20, pitchingCriteria0.Max);
        // Pitching criteria[1]
        var pitchingCriteria1 = criteria.PitchingCriteria[1];
        Assert.Equal("Losses", pitchingCriteria1.Name);
        Assert.Equal(0.4m, pitchingCriteria1.Weight);
        Assert.True(pitchingCriteria1.IsLowerValueBetter);
        Assert.Equal(3, pitchingCriteria1.Min);
        Assert.Equal(30, pitchingCriteria1.Max);

        // Fielding criteria[0]
        var fieldingCriteria0 = criteria.FieldingCriteria[0];
        Assert.Equal("Errors", fieldingCriteria0.Name);
        Assert.Equal(0.2m, fieldingCriteria0.Weight);
        Assert.True(fieldingCriteria0.IsLowerValueBetter);
        Assert.Equal(1, fieldingCriteria0.Min);
        Assert.Equal(10, fieldingCriteria0.Max);
        // Fielding criteria[1]
        var fieldingCriteria1 = criteria.FieldingCriteria[1];
        Assert.Equal("Putouts", fieldingCriteria1.Name);
        Assert.Equal(0.8m, fieldingCriteria1.Weight);
        Assert.False(fieldingCriteria1.IsLowerValueBetter);
        Assert.Equal(8, fieldingCriteria1.Min);
        Assert.Equal(80, fieldingCriteria1.Max);

        // Assessor
        Assert.Equal(ServiceLifetime.Singleton,
            s.First(x => x.ServiceType == typeof(IPerformanceAssessor)).Lifetime);
        Assert.IsType<MinMaxNormalizationPerformanceAssessor>(actual.GetRequiredService<IPerformanceAssessor>());
    }

    [Fact]
    public void AddPerformanceMapping_ServiceCollection_RegistersDependencies()
    {
        // Arrange
        var s = new ServiceCollection();
        s.AddSingleton(Mock.Of<IPerformanceAssessor>());

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
        var s = new ServiceCollection();
        s.AddSingleton(Mock.Of<IPerformanceAssessor>());

        // Act
        s.AddPerformancePlayerSeasonScorekeeper();
        var actual = s.BuildServiceProvider();

        // Assert
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