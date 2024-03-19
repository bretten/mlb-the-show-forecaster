using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

/// <summary>
/// Represents the basic fields of an Item
/// </summary>
/// <param name="Uuid">The unique ID</param>
/// <param name="Type">The type of item</param>
/// <param name="ImageUrl">The URL of the image associated with the item</param>
/// <param name="Name">The name of the item</param>
/// <param name="Rarity">The rarity of the item</param>
/// <param name="Series">The series the item is a part of</param>
public abstract record ItemDto(
    [property: JsonPropertyName("uuid")]
    string Uuid,
    [property: JsonPropertyName("type")]
    string Type,
    [property: JsonPropertyName("img")]
    string ImageUrl,
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("rarity")]
    string Rarity,
    [property: JsonPropertyName("series")]
    string Series
);