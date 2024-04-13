using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Dtos.Mapping.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Dtos.Mapping;

public class MlbTheShowItemMapperTests
{
    [Fact]
    public void Map_UnexpectedItemType_ThrowsException()
    {
        // Arrange
        var seasonYear = SeasonYear.Create(2024);
        var item = Faker.FakeStadiumDto();
        var mapper = new MlbTheShowItemMapper();
        Action action = () => mapper.Map(seasonYear, item);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<UnexpectedTheShowItemException>(actual);
    }

    [Fact]
    public void Map_MlbCardDto_ReturnsMlbPlayerCard()
    {
        // Arrange
        var seasonYear = SeasonYear.Create(2024);
        var item = new MlbCardDto(
            Uuid: Faker.FakeUuidDto(Faker.FakeGuid1),
            Type: "mlb_card",
            ImageUrl: "img.png",
            Name: "name1",
            Rarity: "Gold",
            IsSellable: true,
            Series: "Live",
            TeamShortName: "DOT",
            DisplayPosition: "1B",
            Overall: 90,
            Stamina: 1,
            PitchingClutch: 2,
            HitsPerBf: 3,
            KPerBf: 4,
            BbPerBf: 5,
            HrPerBf: 6,
            PitchVelocity: 7,
            PitchControl: 8,
            PitchMovement: 9,
            ContactLeft: 10,
            ContactRight: 11,
            PowerLeft: 12,
            PowerRight: 13,
            PlateVision: 14,
            PlateDiscipline: 15,
            BattingClutch: 16,
            BuntingAbility: 17,
            DragBuntingAbility: 18,
            HittingDurability: 19,
            FieldingDurability: 20,
            FieldingAbility: 21,
            ArmStrength: 22,
            ArmAccuracy: 23,
            ReactionTime: 24,
            Blocking: 25,
            Speed: 26,
            BaseRunningAbility: 27,
            BaseRunningAggression: 28
        );
        var mapper = new MlbTheShowItemMapper();

        // Act
        var actual = mapper.Map(seasonYear, item);

        // Assert
        Assert.Equal(2024, actual.Year.Value);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), actual.ExternalUuid.Value);
        Assert.Equal(CardType.MlbCard, actual.Type);
        Assert.Equal("img.png", actual.ImageUrl.Value.OriginalString);
        Assert.Equal("name1", actual.Name.Value);
        Assert.Equal(Rarity.Gold, actual.Rarity);
        Assert.True(actual.IsSellable);
        Assert.Equal(CardSeries.Live, actual.Series);
        Assert.Equal(Position.FirstBase, actual.Position);
        Assert.Equal("DOT", actual.TeamShortName.Value);
        Assert.Equal(90, actual.Overall.Value);
        Assert.Equal(1, actual.Stamina.Value);
        Assert.Equal(2, actual.PitchingClutch.Value);
        Assert.Equal(3, actual.HitsPerBf.Value);
        Assert.Equal(4, actual.KPerBf.Value);
        Assert.Equal(5, actual.BbPerBf.Value);
        Assert.Equal(6, actual.HrPerBf.Value);
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

    [Theory]
    [InlineData("Diamond", Rarity.Diamond)]
    [InlineData("Gold", Rarity.Gold)]
    [InlineData("Silver", Rarity.Silver)]
    [InlineData("Bronze", Rarity.Bronze)]
    [InlineData("Common", Rarity.Common)]
    public void MapRarity_RarityString_ReturnsRarityEnum(string rarityString, Rarity expectedRarity)
    {
        // Arrange
        var mapper = new MlbTheShowItemMapper();

        // Act
        var actual = mapper.MapRarity(rarityString);

        // Assert
        Assert.Equal(expectedRarity, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("InvalidRarity")]
    public void MapRarity_InvalidRarityString_ThrowsException(string rarity)
    {
        // Arrange
        var mapper = new MlbTheShowItemMapper();
        Action action = () => mapper.MapRarity(rarity);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidTheShowRarityException>(actual);
    }

    [Theory]
    [InlineData("Live", CardSeries.Live)]
    [InlineData("Rookie", CardSeries.Rookie)]
    public void MapCardSeries_CardSeriesString_ReturnsCardSeriesEnum(string cardSeriesString,
        CardSeries expectedCardSeries)
    {
        // Arrange
        var mapper = new MlbTheShowItemMapper();

        // Act
        var actual = mapper.MapCardSeries(cardSeriesString);

        // Assert
        Assert.Equal(expectedCardSeries, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("InvalidCardSeries")]
    public void MapCardSeries_InvalidCardSeriesString_ThrowsException(string cardSeries)
    {
        // Arrange
        var mapper = new MlbTheShowItemMapper();
        Action action = () => mapper.MapCardSeries(cardSeries);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidTheShowCardSeriesException>(actual);
    }

    [Theory]
    [InlineData("C", Position.Catcher)]
    [InlineData("1B", Position.FirstBase)]
    [InlineData("2B", Position.SecondBase)]
    [InlineData("SS", Position.Shortstop)]
    [InlineData("3B", Position.ThirdBase)]
    [InlineData("RF", Position.RightField)]
    [InlineData("CF", Position.CenterField)]
    [InlineData("LF", Position.LeftField)]
    [InlineData("SP", Position.StartingPitcher)]
    [InlineData("RP", Position.ReliefPitcher)]
    [InlineData("CP", Position.ClosingPitcher)]
    [InlineData("DH", Position.DesignatedHitter)]
    public void MapPosition_PositionString_ReturnsPositionEnum(string positionString, Position expectedPosition)
    {
        // Arrange
        var mapper = new MlbTheShowItemMapper();

        // Act
        var actual = mapper.MapPosition(positionString);

        // Assert
        Assert.Equal(expectedPosition, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("InvalidPosition")]
    public void MapPosition_InvalidPositionString_ThrowsException(string position)
    {
        // Arrange
        var mapper = new MlbTheShowItemMapper();
        Action action = () => mapper.MapPosition(position);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidTheShowPositionException>(actual);
    }
}