﻿using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Listings;

/// <summary>
/// Represents a Listing of an <see cref="ItemDto"/>
/// </summary>
/// <param name="ListingName">The name of the Listing</param>
/// <param name="BestSellPrice">The current, best sell price</param>
/// <param name="BestBuyPrice">The current, best buy price</param>
/// <typeparam name="T"><see cref="ItemDto"/></typeparam>
public sealed record ListingDto<T>(
    [property: JsonPropertyName("listing_name")]
    string ListingName,
    [property: JsonPropertyName("best_sell_price")]
    int BestSellPrice,
    [property: JsonPropertyName("best_buy_price")]
    int BestBuyPrice,
    [property: JsonPropertyName("item")]
    T Item
) where T : ItemDto;