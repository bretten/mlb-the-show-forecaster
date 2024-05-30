using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static Guid FakeGuid1 = new("00000000-0000-0000-0000-000000000001");

    public static CardExternalId FakeCardExternalId(Guid? guid = null)
    {
        return CardExternalId.Create(guid ?? FakeGuid1);
    }

    public static PlayerCard FakePlayerCard(ushort? year = null, Guid? externalId = null,
        CardType type = CardType.MlbCard, CardImageLocation? image = null, CardName? name = null,
        Rarity rarity = Rarity.Bronze, CardSeries series = CardSeries.Live, Position position = Position.RightField,
        TeamShortName? teamShortName = null, int overallRating = 50,
        PlayerCardAttributes? playerCardAttributes = null)
    {
        return PlayerCard.Create(year.HasValue ? SeasonYear.Create(year.Value) : SeasonYear.Create(2024),
            FakeCardExternalId(externalId),
            type,
            image ?? FakeCardImage(),
            name ?? FakeCardName(),
            rarity,
            series,
            position,
            teamShortName ?? FakeTeamShortName(),
            FakeOverallRating(overallRating),
            playerCardAttributes ?? FakePlayerCardAttributes());
    }

    public static PlayerCardHistoricalRating FakeBaselinePlayerCardHistoricalRating(DateOnly? startDate = null,
        DateOnly? endDate = null, int overallRating = 50, PlayerCardAttributes? playerCardAttributes = null)
    {
        return PlayerCardHistoricalRating.Baseline(startDate: startDate ?? new DateOnly(2024, 4, 1),
            endDate: endDate,
            FakeOverallRating(overallRating),
            playerCardAttributes ?? FakePlayerCardAttributes()
        );
    }

    public static PlayerCardHistoricalRating FakeTemporaryPlayerCardHistoricalRating(DateOnly? startDate = null,
        DateOnly? endDate = null, int overallRating = 50, PlayerCardAttributes? playerCardAttributes = null)
    {
        return PlayerCardHistoricalRating.Temporary(startDate: startDate ?? new DateOnly(2024, 4, 1),
            endDate: endDate,
            FakeOverallRating(overallRating),
            playerCardAttributes ?? FakePlayerCardAttributes()
        );
    }

    public static PlayerCardHistoricalRating FakeBoostPlayerCardHistoricalRating(DateOnly? startDate = null,
        DateOnly? endDate = null, int overallRating = 50, PlayerCardAttributes? playerCardAttributes = null)
    {
        return PlayerCardHistoricalRating.Boost(startDate: startDate ?? new DateOnly(2024, 4, 1),
            endDate: endDate,
            FakeOverallRating(overallRating),
            playerCardAttributes ?? FakePlayerCardAttributes()
        );
    }

    public static CardImageLocation FakeCardImage(string cardImage = "img.jpg")
    {
        return CardImageLocation.Create(cardImage);
    }

    public static CardName FakeCardName(string cardName = "cardA")
    {
        return CardName.Create(cardName);
    }

    public static TeamShortName FakeTeamShortName(string teamShortName = "SEA")
    {
        return TeamShortName.Create(teamShortName);
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
}