using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos;

public class MlbPlayerCardTests
{
    [Theory]
    [InlineData(Rarity.Common, 0)]
    [InlineData(Rarity.Bronze, 1)]
    [InlineData(Rarity.Silver, 2)]
    [InlineData(Rarity.Gold, 3)]
    [InlineData(Rarity.Diamond, 4)]
    public void Priority_RarityEnum_ReturnsRarityEnumInteger(Rarity rarity, int expected)
    {
        // Arrange
        var mlbPlayerCard = Faker.FakeMlbPlayerCard(rarity: rarity);

        // Act
        var actual = mlbPlayerCard.Priority;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(CardSeries.Live, true)]
    [InlineData(CardSeries.Rookie, false)]
    public void IsSupported_CardSeries_ReturnsTrueIfSupported(CardSeries series, bool expected)
    {
        // Arrange
        var mlbPlayerCard = Faker.FakeMlbPlayerCard(series: series);

        // Act
        var actual = mlbPlayerCard.IsSupported;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("Hit 5 HRs", true)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void IsBoosted_BoostReason_ReturnsTrueIfBoosted(string? boostReason, bool expected)
    {
        // Arrange
        var mlbPlayerCard = Faker.FakeMlbPlayerCard(boostReason: boostReason);

        // Act
        var actual = mlbPlayerCard.IsBoosted;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(90, true)]
    [InlineData(null, false)]
    public void HasTemporaryRating_TempAndNoTemp_ReturnsTrueAndFalse(int? tempOverall, bool expected)
    {
        // Arrange
        var mlbPlayerCard = Faker.FakeMlbPlayerCard(temporaryOverallRating: tempOverall);

        // Act
        var actual = mlbPlayerCard.HasTemporaryRating;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetAttributes_NoParams_ReturnsPlayerCardAttributes()
    {
        // Arrange
        var mlbPlayerCard = new MlbPlayerCard(
            Year: SeasonYear.Create(2024),
            ExternalUuid: Tests.TestClasses.Faker.FakeCardExternalId(),
            Type: CardType.MlbCard,
            ImageUrl: CardImageLocation.Create("img.png"),
            Name: CardName.Create("name1"),
            Rarity: Rarity.Gold,
            IsSellable: true,
            Series: CardSeries.Live,
            Position: Position.FirstBase,
            TeamShortName: TeamShortName.Create("DOT"),
            Overall: OverallRating.Create(90),
            Stamina: AbilityAttribute.Create(1),
            PitchingClutch: AbilityAttribute.Create(2),
            HitsPerBf: AbilityAttribute.Create(3),
            KPerBf: AbilityAttribute.Create(4),
            BbPerBf: AbilityAttribute.Create(5),
            HrPerBf: AbilityAttribute.Create(6),
            PitchVelocity: AbilityAttribute.Create(7),
            PitchControl: AbilityAttribute.Create(8),
            PitchMovement: AbilityAttribute.Create(9),
            ContactLeft: AbilityAttribute.Create(10),
            ContactRight: AbilityAttribute.Create(11),
            PowerLeft: AbilityAttribute.Create(12),
            PowerRight: AbilityAttribute.Create(13),
            PlateVision: AbilityAttribute.Create(14),
            PlateDiscipline: AbilityAttribute.Create(15),
            BattingClutch: AbilityAttribute.Create(16),
            BuntingAbility: AbilityAttribute.Create(17),
            DragBuntingAbility: AbilityAttribute.Create(18),
            HittingDurability: AbilityAttribute.Create(19),
            FieldingDurability: AbilityAttribute.Create(20),
            FieldingAbility: AbilityAttribute.Create(21),
            ArmStrength: AbilityAttribute.Create(22),
            ArmAccuracy: AbilityAttribute.Create(23),
            ReactionTime: AbilityAttribute.Create(24),
            Blocking: AbilityAttribute.Create(25),
            Speed: AbilityAttribute.Create(26),
            BaseRunningAbility: AbilityAttribute.Create(27),
            BaseRunningAggression: AbilityAttribute.Create(28),
            BoostReason: "Hit 5 HRs",
            TemporaryOverallRating: OverallRating.Create(95)
        );

        // Act
        var actual = mlbPlayerCard.GetAttributes();

        // Assert
        Assert.Equal(1, actual.Stamina.Value);
        Assert.Equal(2, actual.PitchingClutch.Value);
        Assert.Equal(3, actual.HitsPerNine.Value);
        Assert.Equal(4, actual.StrikeoutsPerNine.Value);
        Assert.Equal(5, actual.BaseOnBallsPerNine.Value);
        Assert.Equal(6, actual.HomeRunsPerNine.Value);
        Assert.Equal(7, actual.PitchVelocity.Value);
        Assert.Equal(8, actual.PitchControl.Value);
        Assert.Equal(9, actual.PitchMovement.Value);
        Assert.Equal(10, actual.ContactLeft.Value);
        Assert.Equal(11, actual.ContactRight.Value);
        Assert.Equal(12, actual.PowerLeft.Value);
        Assert.Equal(13, actual.PowerRight.Value);
        Assert.Equal(14, actual.PlateVision.Value);
        Assert.Equal(15, actual.PlateDiscipline.Value);
        Assert.Equal(16, actual.BattingClutch.Value);
        Assert.Equal(17, actual.BuntingAbility.Value);
        Assert.Equal(18, actual.DragBuntingAbility.Value);
        Assert.Equal(19, actual.HittingDurability.Value);
        Assert.Equal(20, actual.FieldingDurability.Value);
        Assert.Equal(21, actual.FieldingAbility.Value);
        Assert.Equal(22, actual.ArmStrength.Value);
        Assert.Equal(23, actual.ArmAccuracy.Value);
        Assert.Equal(24, actual.ReactionTime.Value);
        Assert.Equal(25, actual.Blocking.Value);
        Assert.Equal(26, actual.Speed.Value);
        Assert.Equal(27, actual.BaseRunningAbility.Value);
        Assert.Equal(28, actual.BaseRunningAggression.Value);
    }
}