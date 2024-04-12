using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Converters;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

/// <summary>
/// Represents the basic fields of an Item
/// </summary>
/// <param name="Uuid">The unique ID</param>
/// <param name="Type">The type of item</param>
/// <param name="ImageUrl">The URL of the image associated with the item</param>
/// <param name="Name">The name of the item</param>
/// <param name="Rarity">The rarity of the item</param>
/// <param name="IsSellable">True if the item is sellable on the marketplace, otherwise false</param>
[JsonConverter(typeof(ItemJsonConverter))]
public abstract record ItemDto(
    [property: JsonPropertyName("uuid")]
    [property: JsonConverter(typeof(ObfuscatedIdConverter))]
    ObfuscatedIdDto Uuid,
    [property: JsonPropertyName("type")]
    string Type,
    [property: JsonPropertyName("img")]
    string ImageUrl,
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("rarity")]
    string Rarity,
    [property: JsonPropertyName("is_sellable")]
    bool IsSellable
);