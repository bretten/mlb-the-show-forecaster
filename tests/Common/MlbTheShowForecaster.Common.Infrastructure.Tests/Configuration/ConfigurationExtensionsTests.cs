using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Configuration;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Configuration.Exceptions;
using Microsoft.Extensions.Configuration;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Configuration;

public class ConfigurationExtensionsTests
{
    [Fact]
    public void GetRequiredValue_KeyDoesNotExist_ThrowsException()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "Key1", "Value1" },
            { "Key2", "Value2" }
        };
        var config = GetConfig(settings);

        var action = () => config.GetRequiredValue<string>("Key3");

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<ConfigurationNotSetException>(actual);
    }

    [Fact]
    public void GetRequiredValue_ValueDoesNotExist_ThrowsException()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "Key1", "Value1" },
            { "Key2", "Value2" },
            { "Key3", null },
        };
        var config = GetConfig(settings);

        var action = () => config.GetRequiredValue<string>("Key3");

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<ConfigurationNotSetException>(actual);
    }

    [Fact]
    public void GetRequiredValue_KeyAndStringValueExist_ReturnsValue()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "Key1", "Value1" },
            { "Key2", "Value2" },
            { "Key3", "Value3" },
        };
        var config = GetConfig(settings);

        // Act
        var actual = config.GetRequiredValue<string>("Key3");

        // Assert
        Assert.Equal("Value3", actual);
    }

    [Fact]
    public void GetRequiredValue_KeyAndIntValueExist_ReturnsValue()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "Key1", "Value1" },
            { "Key2", "Value2" },
            { "Key3", "3" },
        };
        var config = GetConfig(settings);

        // Act
        var actual = config.GetRequiredValue<int>("Key3");

        // Assert
        Assert.Equal(3, actual);
    }

    [Fact]
    public void GetRequiredValue_KeyAndDecimalValueExist_ReturnsValue()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "Key1", "Value1" },
            { "Key2", "Value2" },
            { "Key3", "3.14" },
        };
        var config = GetConfig(settings);

        // Act
        var actual = config.GetRequiredValue<decimal>("Key3");

        // Assert
        Assert.Equal(3.14m, actual);
    }

    [Fact]
    public void GetRequiredConnectionString_KeyDoesNotExist_ThrowsException()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "ConnectionStrings:Key1", "Conn1" },
            { "ConnectionStrings:Key2", "Conn2" }
        };
        var config = GetConfig(settings);

        var action = () => config.GetRequiredConnectionString("Key3");

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<ConfigurationNotSetException>(actual);
    }

    [Fact]
    public void GetRequiredConnectionString_ConnectionStringDoesNotExist_ThrowsException()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "ConnectionStrings:Key1", "Conn1" },
            { "ConnectionStrings:Key2", "Conn2" },
            { "ConnectionStrings:Key3", null }
        };
        var config = GetConfig(settings);

        var action = () => config.GetRequiredConnectionString("Key3");

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<ConfigurationNotSetException>(actual);
    }

    [Fact]
    public void GetRequiredConnectionString_KeyAndConnectionStringExist_ReturnsValue()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            { "ConnectionStrings:Key1", "Conn1" },
            { "ConnectionStrings:Key2", "Conn2" },
            { "ConnectionStrings:Key3", "Conn3" }
        };
        var config = GetConfig(settings);

        // Act
        var actual = config.GetRequiredConnectionString("Key3");

        // Assert
        Assert.Equal("Conn3", actual);
    }

    private static IConfiguration GetConfig(Dictionary<string, string?> settings)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
    }
}