namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Results;

/// <summary>
/// Represents the result of <see cref="ICardPriceTracker.TrackCardPrices"/>
/// </summary>
/// <param name="TotalCards">The total number of player cards in the domain</param>
/// <param name="TotalNewListings">The number of player cards in the domain that did not have prior listings</param>
/// <param name="TotalUpdatedListings">The number of player cards in the domain that had listings, but were updated</param>
public readonly record struct CardPriceTrackerResult(int TotalCards, int TotalNewListings, int TotalUpdatedListings)
{
    /// <summary>
    /// The number of unchanged player card listings
    /// </summary>
    public int TotalUnchangedListings => TotalCards - TotalNewListings - TotalUpdatedListings;
};