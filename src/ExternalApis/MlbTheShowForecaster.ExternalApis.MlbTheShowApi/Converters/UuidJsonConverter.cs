using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.RosterUpdates;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Converters;

/// <summary>
/// <see cref="JsonConverter"/> used to serialize/deserialize the UUID on <see cref="ItemDto"/> and <see cref="PlayerChangeDto"/>
///
/// <para>This will parse the UUID from MLB The Show to a DTO. In most responses, the UUID is returned as a valid
/// UUID without hyphens. Although in one edge case (roster update for newly added players), it is not returned as
/// a valid UUID, but instead just the integer -1. This converter is meant to handle this case.</para>
/// </summary>
public sealed class UuidJsonConverter : JsonConverter<UuidDto>
{
    /// <summary>
    /// Uses the reader to check if the UUID is a number. The raw value of the reader is then passed to <see cref="UuidDto"/>
    /// </summary>
    /// <param name="reader">The reader that is parsing the JSON</param>
    /// <param name="typeToConvert">The type being converted</param>
    /// <param name="options">The options for the serializer</param>
    /// <returns>The parsed UUID</returns>
    public override UuidDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.Number
            ? new UuidDto(reader.GetInt32())
            : new UuidDto(reader.GetString());
    }

    /// <summary>
    /// Writes the UUID to a JSON value
    /// </summary>
    /// <param name="writer">The writer to write to</param>
    /// <param name="value">The value being converted</param>
    /// <param name="options">The options for the serializer</param>
    public override void Write(Utf8JsonWriter writer, UuidDto value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ValueAsString);
    }
}