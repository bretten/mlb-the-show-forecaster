using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

/// <summary>
/// An <see cref="ItemDto"/> that represents a player's sponsorship
/// </summary>
/// <param name="Uuid">The unique ID</param>
/// <param name="Type">The type of item</param>
/// <param name="ImageUrl">The URL of the image associated with the item</param>
/// <param name="Name">The name of the item</param>
/// <param name="Rarity">The rarity of the item</param>
/// <param name="IsSellable">True if the item is sellable on the marketplace, otherwise false</param>
/// <param name="Brand">The sponsorship brand</param>
/// <param name="Bonus">The sponsorship bonus</param>
public sealed record SponsorshipDto(
    string Uuid,
    string Type,
    string ImageUrl,
    string Name,
    string Rarity,
    bool IsSellable,
    [property: JsonPropertyName("brand")]
    string Brand,
    [property: JsonPropertyName("bonus")]
    string Bonus
) : ItemDto(Uuid, Type, ImageUrl, Name, Rarity, IsSellable);