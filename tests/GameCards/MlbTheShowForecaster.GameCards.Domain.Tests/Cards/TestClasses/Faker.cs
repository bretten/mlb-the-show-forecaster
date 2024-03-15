﻿using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static PlayerCard FakePlayerCard(CardId? theShowId = null, CardType type = CardType.MlbCard,
        CardImage? image = null, string? name = null, Rarity rarity = Rarity.Bronze,
        CardSeries series = CardSeries.Live, TeamShortName? teamShortName = null, OverallRating? overallRating = null,
        PlayerCardAttributes? playerCardAttributes = null)
    {
        return PlayerCard.Create(theShowId ?? FakeCardId(),
            type,
            image ?? FakeCardImage(),
            name ?? "Ken Griffey Jr.",
            rarity,
            series,
            teamShortName ?? FakeTeamShortName(),
            overallRating ?? FakeOverallRating(),
            playerCardAttributes ?? FakePlayerCardAttributes());
    }

    public static PlayerCardHistoricalRating FakePlayerCardHistoricalRating(DateOnly? startDate = null,
        DateOnly? endDate = null, OverallRating? overallRating = null,
        PlayerCardAttributes? playerCardAttributes = null)
    {
        return PlayerCardHistoricalRating.Create(startDate: startDate ?? new DateOnly(2024, 4, 1),
            endDate: endDate ?? new DateOnly(2024, 4, 2),
            overallRating ?? FakeOverallRating(),
            playerCardAttributes ?? FakePlayerCardAttributes()
        );
    }

    public static CardId FakeCardId(string cardId = "1")
    {
        return CardId.Create(cardId);
    }

    public static CardImage FakeCardImage(string cardImage = "img.jpg")
    {
        return CardImage.Create(cardImage);
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
        return PlayerCardAttributes.Create(stamina: stamina,
            pitchingClutch: pitchingClutch,
            hitsPerNine: hitsPerNine,
            strikeoutsPerNine: strikeoutsPerNine,
            baseOnBallsPerNine: baseOnBallsPerNine,
            homeRunsPerNine: homeRunsPerNine,
            pitchVelocity: pitchVelocity,
            pitchControl: pitchControl,
            pitchMovement: pitchMovement,
            contactLeft: contactLeft,
            contactRight: contactRight,
            powerLeft: powerLeft,
            powerRight: powerRight,
            plateVision: plateVision,
            plateDiscipline: plateDiscipline,
            battingClutch: battingClutch,
            buntingAbility: buntingAbility,
            dragBuntingAbility: dragBuntingAbility,
            hittingDurability: hittingDurability,
            fieldingDurability: fieldingDurability,
            fieldingAbility: fieldingAbility,
            armStrength: armStrength,
            armAccuracy: armAccuracy,
            reactionTime: reactionTime,
            blocking: blocking,
            speed: speed,
            baseRunningAbility: baseRunningAbility,
            baseRunningAggression: baseRunningAggression
        );
    }
}