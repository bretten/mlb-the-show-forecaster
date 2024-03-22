using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

/// <summary>
/// An <see cref="ItemDto"/> that represents a something that is unlockable in MLB The Show
/// </summary>
/// <param name="Uuid">The unique ID</param>
/// <param name="Type">The type of item</param>
/// <param name="ImageUrl">The URL of the image associated with the item</param>
/// <param name="Name">The name of the item</param>
/// <param name="Rarity">The rarity of the item</param>
/// <param name="IsSellable">True if the item is sellable on the marketplace, otherwise false</param>
/// <param name="CategoryId">The category ID</param>
/// <param name="SubCategoryId">The sub-category ID</param>
public sealed record UnlockableDto(
    string Uuid,
    string Type,
    string ImageUrl,
    string Name,
    string Rarity,
    bool IsSellable,
    [property: JsonPropertyName("category_id")]
    int CategoryId,
    [property: JsonPropertyName("sub_category_id")]
    int SubCategoryId
) : ItemDto(Uuid, Type, ImageUrl, Name, Rarity, IsSellable);