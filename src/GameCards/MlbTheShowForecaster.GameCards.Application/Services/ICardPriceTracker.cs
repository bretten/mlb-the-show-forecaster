using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;

/// <summary>
/// Defines an interface that will track the price changes of cards
/// </summary>
public interface ICardPriceTracker
{
    /// <summary>
    /// Should track the prices of card listings and add new listings to the domain if they don't exist
    /// </summary>
    /// <param name="year">The year to track prices for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The completed task</returns>
    Task TrackCardPrices(SeasonYear year, CancellationToken cancellationToken = default);
}