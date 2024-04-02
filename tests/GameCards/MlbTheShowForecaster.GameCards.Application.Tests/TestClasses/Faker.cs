using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static PlayerCard FakePlayerCard(ushort year = 2024, string cardExternalId = "1",
        CardType type = CardType.MlbCard, string image = "img.jpg", string name = "cardA",
        Rarity rarity = Rarity.Bronze, CardSeries series = CardSeries.Live, Position position = Position.RightField,
        string teamShortName = "SEA", int overallRating = 50, PlayerCardAttributes? playerCardAttributes = null)
    {
        return PlayerCard.Create(SeasonYear.Create(year),
            CardExternalId.Create(cardExternalId),
            type,
            CardImageLocation.Create(image),
            CardName.Create(name),
            rarity,
            series,
            position,
            TeamShortName.Create(teamShortName),
            OverallRating.Create(overallRating),
            playerCardAttributes ?? FakePlayerCardAttributes());
    }

    public static PlayerCardHistoricalRating FakePlayerCardHistoricalRating(DateOnly? startDate = null,
        DateOnly? endDate = null, int overallRating = 50, PlayerCardAttributes? playerCardAttributes = null)
    {
        return PlayerCardHistoricalRating.Create(startDate: startDate ?? new DateOnly(2024, 4, 1),
            endDate: endDate ?? new DateOnly(2024, 4, 2),
            OverallRating.Create(overallRating),
            playerCardAttributes ?? FakePlayerCardAttributes()
        );
    }

    public static OverallRating FakeOverallRating(int rating = 50)
    {
        return OverallRating.Create(rating);
    }

    public static PlayerCardAttributes FakePlayerCardAttributes(int stamina = 1, int pitchingClutch = 2,
        int hitsPerNine = 3, int strikeoutsPerNine = 4, int baseOnBallsPerNine = 5, int homeRunsPerNine = 6,
        int pitchVelocity = 7, int pitchControl = 8, int pitchMovement = 9, int contactLeft = 10, int contactRight = 1,
        int powerLeft = 2, int powerRight = 3, int plateVision = 4, int plateDiscipline = 5, int battingClutch = 6,
        int buntingAbility = 7, int dragBuntingAbility = 8, int hittingDurability = 9, int fieldingDurability = 10,
        int fieldingAbility = 1, int armStrength = 2, int armAccuracy = 3, int reactionTime = 4, int blocking = 5,
        int speed = 6, int baseRunningAbility = 7, int baseRunningAggression = 8, int scalar = 1)
    {
        return PlayerCardAttributes.Create(stamina: scalar * stamina,
            pitchingClutch: scalar * pitchingClutch,
            hitsPerNine: scalar * hitsPerNine,
            strikeoutsPerNine: scalar * strikeoutsPerNine,
            baseOnBallsPerNine: scalar * baseOnBallsPerNine,
            homeRunsPerNine: scalar * homeRunsPerNine,
            pitchVelocity: scalar * pitchVelocity,
            pitchControl: scalar * pitchControl,
            pitchMovement: scalar * pitchMovement,
            contactLeft: scalar * contactLeft,
            contactRight: scalar * contactRight,
            powerLeft: scalar * powerLeft,
            powerRight: scalar * powerRight,
            plateVision: scalar * plateVision,
            plateDiscipline: scalar * plateDiscipline,
            battingClutch: scalar * battingClutch,
            buntingAbility: scalar * buntingAbility,
            dragBuntingAbility: scalar * dragBuntingAbility,
            hittingDurability: scalar * hittingDurability,
            fieldingDurability: scalar * fieldingDurability,
            fieldingAbility: scalar * fieldingAbility,
            armStrength: scalar * armStrength,
            armAccuracy: scalar * armAccuracy,
            reactionTime: scalar * reactionTime,
            blocking: scalar * blocking,
            speed: scalar * speed,
            baseRunningAbility: scalar * baseRunningAbility,
            baseRunningAggression: scalar * baseRunningAggression
        );
    }

    public static Listing FakeListing(string cardExternalId = "1", int buyPrice = 0, int sellPrice = 0,
        List<ListingHistoricalPrice>? historicalPrices = null)
    {
        return Listing.Create(CardExternalId.Create(cardExternalId), NaturalNumber.Create(buyPrice),
            NaturalNumber.Create(sellPrice), historicalPrices ?? new List<ListingHistoricalPrice>());
    }

    public static ListingHistoricalPrice FakeListingHistoricalPrice(DateOnly date, int buyPrice = 0, int sellPrice = 0)
    {
        return ListingHistoricalPrice.Create(date, NaturalNumber.Create(buyPrice), NaturalNumber.Create(sellPrice));
    }
}