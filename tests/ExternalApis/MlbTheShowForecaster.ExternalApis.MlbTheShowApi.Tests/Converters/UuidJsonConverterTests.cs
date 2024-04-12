using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Converters;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Tests.Converters;

public class UuidJsonConverterTests
{
    [Fact]
    public void Read_Uuid_ReturnsUuid()
    {
        // Arrange
        const string json = "{\"obfuscated_id\": \"a71cdf423ea5906c5fa85fff95d90360\"}";

        // Act
        var actual = JsonSerializer.Deserialize<Wrapper>(json);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal("a71cdf423ea5906c5fa85fff95d90360", actual.Uuid.RawValue);
        Assert.True(actual.Uuid.IsValid);
        Assert.Equal("a71cdf423ea5906c5fa85fff95d90360", actual.Uuid.ValueAsString);
    }

    [Fact]
    public void Read_NegativeOneInteger_ReturnsNull()
    {
        // Arrange
        const string json = "{\"obfuscated_id\": -1}";

        // Act
        var actual = JsonSerializer.Deserialize<Wrapper>(json);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(-1, actual.Uuid.RawValue);
        Assert.False(actual.Uuid.IsValid);
        Assert.Null(actual.Uuid.Value);
    }

    [Fact]
    public void Write_Uuid_WritesAsString()
    {
        // Arrange
        var uuid = new UuidDto("a71cdf423ea5906c5fa85fff95d90360");

        // Act
        var actual = JsonSerializer.Serialize(uuid);

        // Assert
        Assert.Equal("\"a71cdf423ea5906c5fa85fff95d90360\"", actual); // Quotes are written since it is a JSON value
    }

    private class Wrapper
    {
        [JsonPropertyName("obfuscated_id")]
        [JsonConverter(typeof(UuidJsonConverter))]
        public UuidDto Uuid { get; init; }

        public Wrapper(UuidDto uuid)
        {
            Uuid = uuid;
        }
    }
}