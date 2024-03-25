using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Listings;

/// <summary>
/// Request of Item Listings
/// </summary>
/// <param name="Page">The desired page</param>
/// <param name="Type">The type of Item</param>
/// <param name="Sort">What to sort the results by</param>
/// <param name="Rarity">The item rarity</param>
/// <param name="Order">Sort order, ascending or descending</param>
/// <param name="Name">The name of the item</param>
/// <param name="MinimumBestSellPrice">The minimum, best sell price</param>
/// <param name="MaximumBestSellPrice">The maximum, best sell price</param>
/// <param name="MinimumBestBuyPrice">The minimum, best buy price</param>
/// <param name="MaximumBestBuyPrice">The maximum, best buy price</param>
public sealed record GetListingsRequest(
    int Page,
    [property: AliasAs("type")]
    ItemType? Type = null,
    [property: AliasAs("sort")]
    ListingSort? Sort = null,
    [property: AliasAs("rarity")]
    Rarity? Rarity = null,
    [property: AliasAs("order")]
    Order? Order = null,
    [property: AliasAs("name")]
    string? Name = null,
    [property: AliasAs("min_best_sell_price")]
    int? MinimumBestSellPrice = null,
    [property: AliasAs("max_best_sell_price")]
    int? MaximumBestSellPrice = null,
    [property: AliasAs("min_best_buy_price")]
    int? MinimumBestBuyPrice = null,
    [property: AliasAs("max_best_buy_price")]
    int? MaximumBestBuyPrice = null
) : PaginatedRequest(Page);