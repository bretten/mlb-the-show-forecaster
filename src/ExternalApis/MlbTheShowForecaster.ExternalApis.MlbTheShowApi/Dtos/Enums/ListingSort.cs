using System.Runtime.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;

/// <summary>
/// Determines how Listings are sorted when requesting them
/// </summary>
public enum ListingSort
{
    /// <summary>
    /// Rank
    /// </summary>
    [EnumMember(Value = "rank")] Rank,

    /// <summary>
    /// Best sell price
    /// </summary>
    [EnumMember(Value = "best_sell_price")]
    BestSellPrice,

    /// <summary>
    /// Best buy price
    /// </summary>
    [EnumMember(Value = "best_buy_price")] BestBuyPrice
}