using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Repositories;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Mapping;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Players.EntityFramework;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Constants = com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Constants;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Tests;

public class DependenciesTests
{
    [Fact]
    public void AddPlayerStatusMapping_ServiceCollection_RegistersDependencies()
    {
        // Arrange
        var s = new ServiceCollection();

        // Act
        s.AddPlayerStatusMapping();
        var actual = s.BuildServiceProvider();

        // Assert
        Assert.Equal(ServiceLifetime.Singleton, s.First(x => x.ServiceType == typeof(IPlayerMapper)).Lifetime);
        Assert.IsType<PlayerMapper>(actual.GetRequiredService<IPlayerMapper>());
    }

    [Fact]
    public void AddPlayerTeamProvider_ServiceCollection_RegistersDependencies()
    {
        // Arrange
        var s = new ServiceCollection();

        // Act
        s.AddPlayerTeamProvider();
        var actual = s.BuildServiceProvider();

        // Assert
        Assert.Equal(ServiceLifetime.Singleton, s.First(x => x.ServiceType == typeof(ITeamProvider)).Lifetime);
        Assert.IsType<TeamProvider>(actual.GetRequiredService<ITeamProvider>());
    }

    [Fact]
    public void AddPlayerStatusTracker_ServiceCollection_RegistersDependencies()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { Dependencies.ConfigKeys.MlbApiBaseAddress, Constants.BaseUrl }
        };
        var config = GetConfig(settings);

        var s = new ServiceCollection();

        // Act
        s.AddPlayerStatusTracker(config);
        var actual = s.BuildServiceProvider();

        // Assert
        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(IMlbApi)).Lifetime);
        Assert.IsAssignableFrom<IMlbApi>(actual.GetRequiredService<IMlbApi>());

        Assert.Equal(ServiceLifetime.Singleton, s.First(x => x.ServiceType == typeof(IMlbApiPlayerMapper)).Lifetime);
        Assert.IsType<MlbApiPlayerMapper>(actual.GetRequiredService<IMlbApiPlayerMapper>());

        Assert.Equal(ServiceLifetime.Singleton,
            s.First(x => x.ServiceType == typeof(IPlayerStatusChangeDetector)).Lifetime);
        Assert.IsType<PlayerStatusChangeDetector>(actual.GetRequiredService<IPlayerStatusChangeDetector>());

        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(IPlayerRoster)).Lifetime);
        Assert.IsType<MlbApiPlayerRoster>(actual.GetRequiredService<IPlayerRoster>());

        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(IPlayerStatusTracker)).Lifetime);
        Assert.IsType<PlayerStatusTracker>(actual.GetRequiredService<IPlayerStatusTracker>());
    }

    [Fact]
    public void AddPlayerStatusEntityFrameworkCoreRepositories_ServiceCollection_RegistersDependencies()
    {
        // Arrange
        const string cs = "Server=localhost;Port=5432;Database=test;Uid=postgres;Pwd=postgres;";
        var settings = new Dictionary<string, string?>
        {
            { $"ConnectionStrings:{Dependencies.ConfigKeys.PlayersConnection}", cs }
        };
        var config = GetConfig(settings);

        var s = new ServiceCollection();
        s.AddSingleton(Mock.Of<IDomainEventDispatcher>());

        // Act
        s.AddPlayerStatusEntityFrameworkCoreRepositories(config);
        var actual = s.BuildServiceProvider();

        // Assert
        Assert.Equal(ServiceLifetime.Scoped, s.First(x => x.ServiceType == typeof(PlayersDbContext)).Lifetime);
        Assert.IsType<PlayersDbContext>(actual.GetRequiredService<PlayersDbContext>());

        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(IPlayerRepository)).Lifetime);
        Assert.IsType<EntityFrameworkPlayerRepository>(actual.GetRequiredService<IPlayerRepository>());

        Assert.Equal(ServiceLifetime.Transient,
            s.First(x => x.ServiceType == typeof(IUnitOfWork<IPlayerWork>)).Lifetime);
        Assert.IsType<UnitOfWork<PlayersDbContext>>(actual.GetRequiredService<IUnitOfWork<IPlayerWork>>());
    }

    private static IConfiguration GetConfig(Dictionary<string, string?> settings)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
    }
}