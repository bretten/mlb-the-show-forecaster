using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.Mapping;

public class PlayerCardMapperTests
{
    [Fact]
    public void Map_MlbPlayerCardDto_ReturnsPlayerCard()
    {
        // Arrange
        var mlbPlayerCard = new MlbPlayerCard(
            Year: SeasonYear.Create(2024),
            ExternalUuid: Faker.FakeCardExternalId(Faker.FakeGuid1),
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
            BaseRunningAggression: AbilityAttribute.Create(28)
        );
        var mapper = new PlayerCardMapper();

        // Act
        var actual = mapper.Map(mlbPlayerCard);

        // Assert
        Assert.Equal(2024, actual.Year.Value);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), actual.ExternalId.Value);
        Assert.Equal(CardType.MlbCard, actual.Type);
        Assert.Equal("img.png", actual.ImageLocation.Value.OriginalString);
        Assert.Equal("name1", actual.Name.Value);
        Assert.Equal(Rarity.Gold, actual.Rarity);
        Assert.Equal(CardSeries.Live, actual.Series);
        Assert.Equal(Position.FirstBase, actual.Position);
        Assert.Equal("DOT", actual.TeamShortName.Value);
        Assert.Equal(90, actual.OverallRating.Value);
        Assert.Equal(1, actual.PlayerCardAttributes.Stamina.Value);
        Assert.Equal(2, actual.PlayerCardAttributes.PitchingClutch.Value);
        Assert.Equal(3, actual.PlayerCardAttributes.HitsPerNine.Value);
        Assert.Equal(4, actual.PlayerCardAttributes.StrikeoutsPerNine.Value);
        Assert.Equal(5, actual.PlayerCardAttributes.BaseOnBallsPerNine.Value);
        Assert.Equal(6, actual.PlayerCardAttributes.HomeRunsPerNine.Value);
        Assert.Equal(7, actual.PlayerCardAttributes.PitchVelocity.Value);
        Assert.Equal(8, actual.PlayerCardAttributes.PitchControl.Value);
        Assert.Equal(9, actual.PlayerCardAttributes.PitchMovement.Value);
        Assert.Equal(10, actual.PlayerCardAttributes.ContactLeft.Value);
        Assert.Equal(11, actual.PlayerCardAttributes.ContactRight.Value);
        Assert.Equal(12, actual.PlayerCardAttributes.PowerLeft.Value);
        Assert.Equal(13, actual.PlayerCardAttributes.PowerRight.Value);
        Assert.Equal(14, actual.PlayerCardAttributes.PlateVision.Value);
        Assert.Equal(15, actual.PlayerCardAttributes.PlateDiscipline.Value);
        Assert.Equal(16, actual.PlayerCardAttributes.BattingClutch.Value);
        Assert.Equal(17, actual.PlayerCardAttributes.BuntingAbility.Value);
        Assert.Equal(18, actual.PlayerCardAttributes.DragBuntingAbility.Value);
        Assert.Equal(19, actual.PlayerCardAttributes.HittingDurability.Value);
        Assert.Equal(20, actual.PlayerCardAttributes.FieldingDurability.Value);
        Assert.Equal(21, actual.PlayerCardAttributes.FieldingAbility.Value);
        Assert.Equal(22, actual.PlayerCardAttributes.ArmStrength.Value);
        Assert.Equal(23, actual.PlayerCardAttributes.ArmAccuracy.Value);
        Assert.Equal(24, actual.PlayerCardAttributes.ReactionTime.Value);
        Assert.Equal(25, actual.PlayerCardAttributes.Blocking.Value);
        Assert.Equal(26, actual.PlayerCardAttributes.Speed.Value);
        Assert.Equal(27, actual.PlayerCardAttributes.BaseRunningAbility.Value);
        Assert.Equal(28, actual.PlayerCardAttributes.BaseRunningAggression.Value);
    }
}