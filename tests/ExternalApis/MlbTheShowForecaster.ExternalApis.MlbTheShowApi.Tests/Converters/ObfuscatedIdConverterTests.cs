using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Converters;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Tests.Converters;

public class ObfuscatedIdConverterTests
{
    [Fact]
    public void Read_Uuid_ReturnsObfuscatedId()
    {
        // Arrange
        const string json = "{\"obfuscated_id\": \"a71cdf423ea5906c5fa85fff95d90360\"}";

        // Act
        var actual = JsonSerializer.Deserialize<Wrapper>(json);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal("a71cdf423ea5906c5fa85fff95d90360", actual.ObfuscatedId.RawValue);
        Assert.True(actual.ObfuscatedId.IsValid);
        Assert.Equal("a71cdf423ea5906c5fa85fff95d90360", actual.ObfuscatedId.Value.ToString("N"));
    }

    [Fact]
    public void Read_NegativeOneInteger_ReturnsObfuscatedId()
    {
        // Arrange
        const string json = "{\"obfuscated_id\": -1}";

        // Act
        var actual = JsonSerializer.Deserialize<Wrapper>(json);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(-1, actual.ObfuscatedId.RawValue);
        Assert.False(actual.ObfuscatedId.IsValid);
        Assert.Equal(Guid.Empty, actual.ObfuscatedId.Value);
    }

    [Fact]
    public void Write_Uuid_WritesAsString()
    {
        // Arrange
        var obfuscatedId = new ObfuscatedIdDto("a71cdf423ea5906c5fa85fff95d90360");

        // Act
        var actual = JsonSerializer.Serialize(obfuscatedId);

        // Assert
        Assert.Equal("\"a71cdf423ea5906c5fa85fff95d90360\"", actual); // Quotes are written since it is a JSON value
    }

    private class Wrapper
    {
        [JsonPropertyName("obfuscated_id")]
        [JsonConverter(typeof(ObfuscatedIdConverter))]
        public ObfuscatedIdDto ObfuscatedId { get; init; }

        public Wrapper(ObfuscatedIdDto obfuscatedId)
        {
            ObfuscatedId = obfuscatedId;
        }
    }
}