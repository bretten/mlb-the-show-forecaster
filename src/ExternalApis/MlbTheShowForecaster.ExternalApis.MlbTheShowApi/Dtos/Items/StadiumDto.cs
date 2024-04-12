using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

/// <summary>
/// An <see cref="ItemDto"/> that represents a baseball stadium
/// </summary>
/// <param name="Uuid">The unique ID</param>
/// <param name="Type">The type of item</param>
/// <param name="ImageUrl">The URL of the image associated with the item</param>
/// <param name="Name">The name of the item</param>
/// <param name="Rarity">The rarity of the item</param>
/// <param name="IsSellable">True if the item is sellable on the marketplace, otherwise false</param>
/// <param name="TeamShortName">The stadium's team name abbreviation</param>
/// <param name="Capacity">The stadium capacity</param>
/// <param name="Surface">The stadium surface type</param>
/// <param name="Elevation">The stadium elevation</param>
/// <param name="Built">The year the stadium was built</param>
public sealed record StadiumDto(
    ObfuscatedIdDto Uuid,
    string Type,
    string ImageUrl,
    string Name,
    string Rarity,
    bool IsSellable,
    [property: JsonPropertyName("team_short_name")]
    string TeamShortName,
    [property: JsonPropertyName("capacity")]
    string Capacity,
    [property: JsonPropertyName("surface")]
    string Surface,
    [property: JsonPropertyName("elevation")]
    string Elevation,
    [property: JsonPropertyName("built")]
    int Built
) : ItemDto(Uuid, Type, ImageUrl, Name, Rarity, IsSellable);