using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Results;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Services.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static PlayerCardTrackerResult FakePlayerCardTrackerResult(int totalCatalogCards = 0,
        int totalNewCatalogCards = 0, int totalUpdatedPlayerCards = 0)
    {
        return new PlayerCardTrackerResult(totalCatalogCards, totalNewCatalogCards, totalUpdatedPlayerCards);
    }

    public static CardPriceTrackerResult FakeCardPriceTrackerResult(int totalCards = 0, int totalNewListings = 0,
        int totalUpdatedListings = 0)
    {
        return new CardPriceTrackerResult(totalCards, totalNewListings, totalUpdatedListings);
    }

    public static RosterUpdateOrchestratorResult FakeRosterUpdateOrchestratorResult(DateOnly? date = null,
        int totalRatingChanges = 0, int totalPositionChanges = 0, int totalNewPlayers = 0)
    {
        return new RosterUpdateOrchestratorResult(date ?? new DateOnly(2024, 10, 18), totalRatingChanges,
            totalPositionChanges, totalNewPlayers);
    }

    public static PlayerRatingHistoryResult FakePlayerRatingHistoryResult(
        Dictionary<PlayerCard, IReadOnlyList<PlayerCardHistoricalRating>>? cards = null)
    {
        return new PlayerRatingHistoryResult(cards ??
                                             new Dictionary<PlayerCard, IReadOnlyList<PlayerCardHistoricalRating>>());
    }
}