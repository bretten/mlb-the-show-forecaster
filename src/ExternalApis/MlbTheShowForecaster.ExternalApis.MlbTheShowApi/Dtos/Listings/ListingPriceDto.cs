using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Listings;

/// <summary>
/// Represents a price for a <see cref="ListingDto{T}"/> on a given date
/// </summary>
/// <param name="Date">The date of the price, expressed as MM/DD</param>
/// <param name="BestSellPrice">The best sell price of the day</param>
/// <param name="BestBuyPrice">The best buy price of the day</param>
public sealed record ListingPriceDto(
    [property: JsonPropertyName("date")]
    string Date,
    [property: JsonPropertyName("best_sell_price")]
    int BestSellPrice,
    [property: JsonPropertyName("best_buy_price")]
    int BestBuyPrice
);