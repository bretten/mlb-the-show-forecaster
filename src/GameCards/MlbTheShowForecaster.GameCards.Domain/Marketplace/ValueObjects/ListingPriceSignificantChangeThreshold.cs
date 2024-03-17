using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

/// <summary>
/// The limits that determine if a <see cref="Listing"/>'s price changes are significant
/// </summary>
public sealed class ListingPriceSignificantChangeThreshold : ValueObject, IListingPriceSignificantChangeThreshold
{
    /// <summary>
    /// The percentage change of a buy price required for it to be considered significant
    /// </summary>
    public decimal BuyPricePercentageChangeThreshold { get; }

    /// <summary>
    /// The percentage change of a sell price required for it to be considered significant
    /// </summary>
    public decimal SellPricePercentageChangeThreshold { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="buyPricePercentageChangeThreshold">The percentage change of a buy price required for it to be considered significant</param>
    /// <param name="sellPricePercentageChangeThreshold">The percentage change of a sell price required for it to be considered significant</param>
    private ListingPriceSignificantChangeThreshold(decimal buyPricePercentageChangeThreshold,
        decimal sellPricePercentageChangeThreshold)
    {
        BuyPricePercentageChangeThreshold = buyPricePercentageChangeThreshold;
        SellPricePercentageChangeThreshold = sellPricePercentageChangeThreshold;
    }

    /// <summary>
    /// Creates a <see cref="ListingPriceSignificantChangeThreshold"/>
    /// </summary>
    /// <param name="buyPricePercentageChangeThreshold">The percentage change of a buy price required for it to be considered significant</param>
    /// <param name="sellPricePercentageChangeThreshold">The percentage change of a sell price required for it to be considered significant</param>
    /// <returns><see cref="ListingPriceSignificantChangeThreshold"/></returns>
    public static ListingPriceSignificantChangeThreshold Create(decimal buyPricePercentageChangeThreshold,
        decimal sellPricePercentageChangeThreshold)
    {
        return new ListingPriceSignificantChangeThreshold(
            buyPricePercentageChangeThreshold: buyPricePercentageChangeThreshold,
            sellPricePercentageChangeThreshold: sellPricePercentageChangeThreshold
        );
    }
}