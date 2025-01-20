using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.Common.Application.RealTime;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Jobs;

public class DependenciesTests
{
    [Fact]
    public void AddJobManager_ServicesCollection_RegistersJobManager()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "Jobs:Schedules:0", "SomeJob-00:00:00:05" },
            { "Jobs:RunOnStartup", "false" },
            { "Jobs:Seasons:0", "2024" },
        };
        var config = GetConfig(settings);

        var s = new ServiceCollection();

        // Act
        s.AddSingleton(Mock.Of<IRealTimeCommService>());
        s.AddSingleton(Mock.Of<ILogger<ScopedSingleInstanceJobManager>>());
        s.AddJobManager(config);
        var actual = s.BuildServiceProvider();

        /*
         * Assert
         */
        // IJobManager registered as ScopedSingleInstanceJobManager
        Assert.Equal(ServiceLifetime.Singleton, s.First(x => x.ServiceType == typeof(IJobManager)).Lifetime);
        Assert.IsType<ScopedSingleInstanceJobManager>(actual.GetRequiredService<IJobManager>());
    }

    private static IConfiguration GetConfig(Dictionary<string, string?> settings)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
    }
}