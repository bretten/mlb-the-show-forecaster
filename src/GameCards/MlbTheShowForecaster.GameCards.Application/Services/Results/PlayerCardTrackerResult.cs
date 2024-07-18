using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Results;

/// <summary>
/// Represents the result of <see cref="IPlayerCardTracker.TrackPlayerCards"/>
/// </summary>
/// <param name="TotalCatalogCards">The total number of cards found in the external source <see cref="ICardCatalog"/></param>
/// <param name="TotalNewCatalogCards">The number of cards present in the <see cref="ICardCatalog"/> but not present in the domain</param>
/// <param name="TotalUpdatedPlayerCards">The number of <see cref="PlayerCard"/>s already present in the domain, but needed updating</param>
public readonly record struct PlayerCardTrackerResult(
    int TotalCatalogCards,
    int TotalNewCatalogCards,
    int TotalUpdatedPlayerCards)
{
    /// <summary>
    /// The number player cards from <see cref="ICardCatalog"/> that already existed in the domain
    /// </summary>
    public int TotalUnchangedPlayerCards => TotalCatalogCards - TotalNewCatalogCards - TotalUpdatedPlayerCards;
}