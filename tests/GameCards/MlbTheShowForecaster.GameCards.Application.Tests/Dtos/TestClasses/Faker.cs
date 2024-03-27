using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static MlbPlayerCard FakeMlbPlayerCard(ushort? year = null, string? externalId = null,
        CardType type = CardType.MlbCard, string? image = null, string? name = null,
        Rarity rarity = Rarity.Bronze, CardSeries series = CardSeries.Live, Position position = Position.RightField,
        string? teamShortName = null, int? overallRating = null, int stamina = 1, int pitchingClutch = 2,
        int hitsPerNine = 3, int strikeoutsPerNine = 4, int baseOnBallsPerNine = 5, int homeRunsPerNine = 6,
        int pitchVelocity = 7, int pitchControl = 8, int pitchMovement = 9, int contactLeft = 10, int contactRight = 1,
        int powerLeft = 2, int powerRight = 3, int plateVision = 4, int plateDiscipline = 5, int battingClutch = 6,
        int buntingAbility = 7, int dragBuntingAbility = 8, int hittingDurability = 9, int fieldingDurability = 10,
        int fieldingAbility = 1, int armStrength = 2, int armAccuracy = 3, int reactionTime = 4, int blocking = 5,
        int speed = 6, int baseRunningAbility = 7, int baseRunningAggression = 8, int scalar = 1)
    {
        return new MlbPlayerCard(
            Year: year.HasValue ? SeasonYear.Create(year.Value) : SeasonYear.Create(2024),
            ExternalUuid: !string.IsNullOrWhiteSpace(externalId)
                ? CardExternalId.Create(externalId)
                : CardExternalId.Create("id1"),
            Type: type,
            ImageUrl: !string.IsNullOrWhiteSpace(image)
                ? CardImageLocation.Create(image)
                : CardImageLocation.Create("img.png"),
            Name: !string.IsNullOrWhiteSpace(name) ? CardName.Create(name) : CardName.Create("name1"),
            Rarity: rarity,
            IsSellable: true,
            Series: series,
            Position: position,
            TeamShortName: !string.IsNullOrWhiteSpace(teamShortName)
                ? TeamShortName.Create(teamShortName)
                : TeamShortName.Create("DOT"),
            Overall: overallRating.HasValue ? OverallRating.Create(overallRating.Value) : OverallRating.Create(90),
            Stamina: AbilityAttribute.Create(scalar * stamina),
            PitchingClutch: AbilityAttribute.Create(scalar * pitchingClutch),
            HitsPerBf: AbilityAttribute.Create(scalar * hitsPerNine),
            KPerBf: AbilityAttribute.Create(scalar * strikeoutsPerNine),
            BbPerBf: AbilityAttribute.Create(scalar * baseOnBallsPerNine),
            HrPerBf: AbilityAttribute.Create(scalar * homeRunsPerNine),
            PitchVelocity: AbilityAttribute.Create(scalar * pitchVelocity),
            PitchControl: AbilityAttribute.Create(scalar * pitchControl),
            PitchMovement: AbilityAttribute.Create(scalar * pitchMovement),
            ContactLeft: AbilityAttribute.Create(scalar * contactLeft),
            ContactRight: AbilityAttribute.Create(scalar * contactRight),
            PowerLeft: AbilityAttribute.Create(scalar * powerLeft),
            PowerRight: AbilityAttribute.Create(scalar * powerRight),
            PlateVision: AbilityAttribute.Create(scalar * plateVision),
            PlateDiscipline: AbilityAttribute.Create(scalar * plateDiscipline),
            BattingClutch: AbilityAttribute.Create(scalar * battingClutch),
            BuntingAbility: AbilityAttribute.Create(scalar * buntingAbility),
            DragBuntingAbility: AbilityAttribute.Create(scalar * dragBuntingAbility),
            HittingDurability: AbilityAttribute.Create(scalar * hittingDurability),
            FieldingDurability: AbilityAttribute.Create(scalar * fieldingDurability),
            FieldingAbility: AbilityAttribute.Create(scalar * fieldingAbility),
            ArmStrength: AbilityAttribute.Create(scalar * armStrength),
            ArmAccuracy: AbilityAttribute.Create(scalar * armAccuracy),
            ReactionTime: AbilityAttribute.Create(scalar * reactionTime),
            Blocking: AbilityAttribute.Create(scalar * blocking),
            Speed: AbilityAttribute.Create(scalar * speed),
            BaseRunningAbility: AbilityAttribute.Create(scalar * baseRunningAbility),
            BaseRunningAggression: AbilityAttribute.Create(scalar * baseRunningAggression)
        );
    }
}