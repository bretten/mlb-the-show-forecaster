using AutoMapper;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Mapping.AutoMapper;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Tests.Mapping.AutoMapper;

public class MlbApiProfileTests
{
    [Fact]
    public void Constructor_MappingConfiguration_ChecksForValidConfiguration()
    {
        // Arrange
        var config = new MapperConfiguration(x => x.AddMaps(typeof(MlbApiProfile)));
        var action = () => config.AssertConfigurationIsValid();

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.Null(actual);
    }
}