using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Converters;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

/// <summary>
/// Represents an obfuscated ID on a <see cref="ItemDto"/>
///
/// <para>This value should be a UUID, but the MLB The Show API will sometimes return a -1 integer. To keep track of
/// which were not valid UUIDs, this record allows it to be instantiated as is but will flag it as invalid.</para>
/// </summary>
[JsonConverter(typeof(ObfuscatedIdConverter))]
public sealed record ObfuscatedIdDto
{
    /// <summary>
    /// The raw value from MLB The Show. Should either be a UUID or the integer -1
    /// </summary>
    public object? RawValue { get; }

    /// <summary>
    /// Returns true if the value is a valid UUID
    /// </summary>
    public bool IsValid { get; }

    /// <summary>
    /// The UUID value. If the raw value was a -1 integer, the UUID value will be Guid.Empty
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rawValue">The raw value from MLB The Show. Should either be a UUID or the integer -1</param>
    public ObfuscatedIdDto(object? rawValue)
    {
        RawValue = rawValue;
        IsValid = Guid.TryParse(rawValue?.ToString(), out var guid);
        Value = IsValid ? guid : Guid.Empty;
    }
}