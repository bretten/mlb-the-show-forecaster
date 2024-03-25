using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

/// <summary>
/// Sets limits for what it determines as significant changes to a <see cref="Listing"/>'s price
/// </summary>
public interface IListingPriceSignificantChangeThreshold
{
    /// <summary>
    /// The percentage change of a buy price required for it to be considered significant
    /// </summary>
    decimal BuyPricePercentageChangeThreshold { get; }

    /// <summary>
    /// The percentage change of a sell price required for it to be considered significant
    /// </summary>
    decimal SellPricePercentageChangeThreshold { get; }
}