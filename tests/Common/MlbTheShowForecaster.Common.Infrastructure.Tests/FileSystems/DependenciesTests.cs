using com.brettnamba.MlbTheShowForecaster.Common.Application.FileSystems;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.FileSystems;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.FileSystems;

public class DependenciesTests
{
    [Fact]
    public void AddFileSystems_ServicesCollection_RegistersFileSystemsModule()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { Dependencies.ConfigKeys.Type, "Local" }
        };
        var config = GetConfig(settings);

        var s = new ServiceCollection();

        // Act
        s.AddFileSystems(config);
        var actual = s.BuildServiceProvider();

        /*
         * Assert
         */
        // IFileSystem registered as LocalFileSystem
        Assert.Equal(ServiceLifetime.Singleton, s.First(x => x.ServiceType == typeof(IFileSystem)).Lifetime);
        Assert.IsType<LocalFileSystem>(actual.GetRequiredService<IFileSystem>());
    }

    private static IConfiguration GetConfig(Dictionary<string, string?> settings)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
    }
}