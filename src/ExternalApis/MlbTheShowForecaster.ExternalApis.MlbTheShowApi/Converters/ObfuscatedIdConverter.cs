using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.RosterUpdates;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Converters;

/// <summary>
/// <see cref="JsonConverter"/> used to serialize/deserialize the obfuscated ID on <see cref="PlayerChangeDto"/>
///
/// <para>MLB The Show usually returns the obfuscated ID as a UUID string, but in some cases for newly added players in
/// roster updates, the ID will be a -1 integer. So this converter is used to catch those edge cases</para>
/// </summary>
public sealed class ObfuscatedIdConverter : JsonConverter<ObfuscatedIdDto>
{
    /// <summary>
    /// Uses the reader to check if the obfuscated ID is a number. The raw value of the reader is then passed to
    /// <see cref="ObfuscatedIdDto"/>
    /// </summary>
    /// <param name="reader">The reader that is parsing the JSON</param>
    /// <param name="typeToConvert">The type being converted</param>
    /// <param name="options">The options for the serializer</param>
    /// <returns>The parsed obfuscated ID</returns>
    public override ObfuscatedIdDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.Number
            ? new ObfuscatedIdDto(reader.GetInt32())
            : new ObfuscatedIdDto(reader.GetString());
    }

    /// <summary>
    /// Writes the obfuscated ID to a JSON value
    /// </summary>
    /// <param name="writer">The writer to write to</param>
    /// <param name="value">The value being converted</param>
    /// <param name="options">The options for the serializer</param>
    public override void Write(Utf8JsonWriter writer, ObfuscatedIdDto value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ValueAsString);
    }
}