using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Converters;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.RosterUpdates;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

/// <summary>
/// Represents an UUID on a <see cref="ItemDto"/> or <see cref="PlayerChangeDto"/>
///
/// <para>This value should be a UUID, but the MLB The Show API will sometimes return a -1 integer. To keep track of
/// which were not valid UUIDs, this record allows it to be instantiated as is but will flag it as invalid.</para>
/// </summary>
[JsonConverter(typeof(UuidJsonConverter))]
public sealed record UuidDto
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
    public Guid? Value { get; }

    /// <summary>
    /// Returns the value in the same string format that MLB The Show returns the value as
    /// </summary>
    public string ValueAsString => Value?.ToString("N") ?? "";

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rawValue">The raw value from MLB The Show. Should either be a UUID or the integer -1</param>
    public UuidDto(object? rawValue)
    {
        RawValue = rawValue;
        IsValid = Guid.TryParse(rawValue?.ToString(), out var guid);
        Value = IsValid ? guid : new Guid?();
    }
}