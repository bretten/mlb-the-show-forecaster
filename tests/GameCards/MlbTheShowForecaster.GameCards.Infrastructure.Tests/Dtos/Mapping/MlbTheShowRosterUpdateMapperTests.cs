using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.RosterUpdates;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.RosterUpdates;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Dtos.Mapping.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Dtos.Mapping;

public class MlbTheShowRosterUpdateMapperTests
{
    [Fact]
    public void Map_RosterUpdateResponse_ReturnsRosterUpdate()
    {
        // Arrange
        var rosterUpdate = new RosterUpdateDto(1, "April 1, 2024");
        var attributeChange = Faker.FakeAttributeChangeDto(name: "FLD", delta: "3");
        var playerChange = Faker.FakePlayerAttributeChangeDto(currentRank: 80, currentRarity: "Gold",
            attributeChangeDtos: new List<AttributeChangeDto>() { attributeChange });
        var positionChange = Faker.FakePlayerPositionChangeDto(position: "3B");
        var newPlayer = Faker.FakeNewlyAddedPlayerDto(uuid: Faker.FakeGuid1);
        var rosterUpdateResponse = new GetRosterUpdateResponse(new List<PlayerAttributeChangeDto>() { playerChange },
            new List<PlayerPositionChangeDto>() { positionChange },
            new List<NewlyAddedPlayerDto>() { newPlayer }
        );

        var stubItemMapper = new Mock<IMlbTheShowItemMapper>();
        stubItemMapper.Setup(x => x.MapPosition("3B"))
            .Returns(Position.ThirdBase);

        var mapper = new MlbTheShowRosterUpdateMapper(stubItemMapper.Object);

        // Act
        var actual = mapper.Map(rosterUpdate, rosterUpdateResponse);

        // Assert
        Assert.Equal(new DateOnly(2024, 4, 1), actual.Date);
        Assert.Single(actual.RatingChanges);
        Assert.Equal(80, actual.RatingChanges[0].NewRating.Value);
        Assert.Single(actual.PositionChanges);
        Assert.Equal(Position.ThirdBase, actual.PositionChanges[0].NewPosition);
        Assert.Single(actual.NewPlayers);
        Assert.Equal("00000000000000000000000000000001", actual.NewPlayers[0].CardExternalId.Value);
    }

    [Fact]
    public void Map_PlayerAttributeChange_ReturnsPlayerRatingChange()
    {
        // Arrange
        var date = new DateOnly(2024, 4, 1);
        var playerAttributeChange = new PlayerAttributeChangeDto(
            new UuidDto("a71cdf423ea5906c5fa85fff95d90360"),
            Name: "name100",
            Team: "team100",
            Item: Faker.FakeMlbCardDto(uuid: Faker.FakeGuid1),
            CurrentRank: 80,
            CurrentRarity: "Gold",
            OldRank: 75,
            OldRarity: "Silver",
            new List<AttributeChangeDto>()
            {
                Faker.FakeAttributeChangeDto(name: "FLD", delta: "3")
            }
        );

        var stubItemMapper = new Mock<IMlbTheShowItemMapper>();
        stubItemMapper.Setup(x => x.MapRarity("Gold"))
            .Returns(Rarity.Gold);
        stubItemMapper.Setup(x => x.MapRarity("Silver"))
            .Returns(Rarity.Silver);

        var mapper = new MlbTheShowRosterUpdateMapper(stubItemMapper.Object);

        // Act
        var actual = mapper.Map(date, playerAttributeChange);

        // Assert
        Assert.Equal(new DateOnly(2024, 4, 1), actual.Date);
        Assert.Equal("a71cdf423ea5906c5fa85fff95d90360", actual.CardExternalId.Value);
        Assert.Equal(80, actual.NewRating.Value);
        Assert.Equal(Rarity.Gold, actual.NewRarity);
        Assert.Equal(75, actual.OldRating.Value);
        Assert.Equal(Rarity.Silver, actual.OldRarity);
        Assert.Equal(3, actual.AttributeChanges.FieldingAbility);
    }

    [Fact]
    public void Map_PositionChange_ReturnsPlayerPositionChange()
    {
        // Arrange
        var id = new UuidDto("a71cdf423ea5906c5fa85fff95d90360");
        const string position = "3B";
        var positionChange = new PlayerPositionChangeDto(
            id,
            Name: "name1",
            Team: "team1",
            Item: Faker.FakeMlbCardDto(),
            Position: position
        );

        var stubItemMapper = new Mock<IMlbTheShowItemMapper>();
        stubItemMapper.Setup(x => x.MapPosition("3B"))
            .Returns(Position.ThirdBase);

        var mapper = new MlbTheShowRosterUpdateMapper(stubItemMapper.Object);

        // Act
        var actual = mapper.Map(positionChange);

        // Assert
        Assert.Equal("a71cdf423ea5906c5fa85fff95d90360", actual.CardExternalId.Value);
        Assert.Equal(Position.ThirdBase, actual.NewPosition);
    }

    [Fact]
    public void Map_NewlyAddedPlayer_ReturnsPlayerAddition()
    {
        // Arrange
        var id = new UuidDto("a71cdf423ea5906c5fa85fff95d90360");
        var newlyAddedPlayer = new NewlyAddedPlayerDto(
            id,
            Name: "name1",
            Team: "team1",
            Position: "3B",
            CurrentRank: 80,
            CurrentRarity: "Gold"
        );

        var mapper = new MlbTheShowRosterUpdateMapper(Mock.Of<IMlbTheShowItemMapper>());

        // Act
        var actual = mapper.Map(newlyAddedPlayer);

        // Assert
        Assert.Equal("a71cdf423ea5906c5fa85fff95d90360", actual.CardExternalId.Value);
    }

    [Fact]
    public void Map_InvalidAttributeAmountForCurrentValue_ThrowsException()
    {
        // Arrange
        var attributeChanges = new List<AttributeChangeDto>()
        {
            new(Name: "ACC", CurrentValue: "90a", Delta: "-5", Direction: "", Color: ""),
        };
        var mapper = new MlbTheShowRosterUpdateMapper(Mock.Of<IMlbTheShowItemMapper>());
        Action action = () => mapper.Map(attributeChanges);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidTheShowAttributeAmountException>(actual);
    }

    [Fact]
    public void Map_InvalidAttributeAmountForDelta_ThrowsException()
    {
        // Arrange
        var attributeChanges = new List<AttributeChangeDto>()
        {
            new(Name: "ACC", CurrentValue: "90", Delta: "-5a", Direction: "", Color: ""),
        };
        var mapper = new MlbTheShowRosterUpdateMapper(Mock.Of<IMlbTheShowItemMapper>());
        Action action = () => mapper.Map(attributeChanges);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidTheShowAttributeAmountException>(actual);
    }

    [Fact]
    public void Map_AttributeChangeCollection_ReturnsCombinedAttributeChange()
    {
        // Arrange
        var attributeChanges = new List<AttributeChangeDto>()
        {
            new(Name: "ACC", CurrentValue: "57", Delta: "-13", Direction: "", Color: ""),
            new(Name: "ARM", CurrentValue: "58", Delta: "-12", Direction: "", Color: ""),
            new(Name: "BB/9", CurrentValue: "59", Delta: "-11", Direction: "", Color: ""),
            new(Name: "BLK", CurrentValue: "60", Delta: "-10", Direction: "", Color: ""),
            new(Name: "BNT", CurrentValue: "61", Delta: "-9", Direction: "", Color: ""),
            new(Name: "BRK", CurrentValue: "62", Delta: "-8", Direction: "", Color: ""),
            new(Name: "BR AGG", CurrentValue: "63", Delta: "-7", Direction: "", Color: ""),
            new(Name: "CLT", CurrentValue: "64", Delta: "-6", Direction: "", Color: ""),
            new(Name: "CON L", CurrentValue: "65", Delta: "-5", Direction: "", Color: ""),
            new(Name: "CON R", CurrentValue: "66", Delta: "-4", Direction: "", Color: ""),
            new(Name: "CTRL", CurrentValue: "67", Delta: "-3", Direction: "", Color: ""),
            new(Name: "DISC", CurrentValue: "68", Delta: "-2", Direction: "", Color: ""),
            new(Name: "DRG BNT", CurrentValue: "69", Delta: "-1", Direction: "", Color: ""),
            new(Name: "DUR", CurrentValue: "70", Delta: "0", Direction: "", Color: ""),
            new(Name: "FLD", CurrentValue: "71", Delta: "1", Direction: "", Color: ""),
            new(Name: "H/9", CurrentValue: "72", Delta: "2", Direction: "", Color: ""),
            new(Name: "HR/9", CurrentValue: "73", Delta: "3", Direction: "", Color: ""),
            new(Name: "K/9", CurrentValue: "74", Delta: "4", Direction: "", Color: ""),
            new(Name: "PCLT", CurrentValue: "75", Delta: "5", Direction: "", Color: ""),
            new(Name: "POW L", CurrentValue: "76", Delta: "6", Direction: "", Color: ""),
            new(Name: "POW R", CurrentValue: "77", Delta: "7", Direction: "", Color: ""),
            new(Name: "REAC", CurrentValue: "78", Delta: "8", Direction: "", Color: ""),
            new(Name: "SPD", CurrentValue: "79", Delta: "9", Direction: "", Color: ""),
            new(Name: "STA", CurrentValue: "80", Delta: "10", Direction: "", Color: ""),
            new(Name: "STEAL", CurrentValue: "81", Delta: "11", Direction: "", Color: ""),
            new(Name: "VEL", CurrentValue: "82", Delta: "12", Direction: "", Color: ""),
            new(Name: "VIS", CurrentValue: "83", Delta: "13", Direction: "", Color: "")
        };
        var mapper = new MlbTheShowRosterUpdateMapper(Mock.Of<IMlbTheShowItemMapper>());

        // Act
        var actual = mapper.Map(attributeChanges);

        // Assert
        Assert.Equal(-13, actual.ArmAccuracy);
        Assert.Equal(-12, actual.ArmStrength);
        Assert.Equal(-11, actual.BbPerBf);
        Assert.Equal(-10, actual.Blocking);
        Assert.Equal(-9, actual.BuntingAbility);
        Assert.Equal(-8, actual.PitchMovement);
        Assert.Equal(-7, actual.BaseRunningAggression);
        Assert.Equal(-6, actual.BattingClutch);
        Assert.Equal(-5, actual.ContactLeft);
        Assert.Equal(-4, actual.ContactRight);
        Assert.Equal(-3, actual.PitchControl);
        Assert.Equal(-2, actual.PlateDiscipline);
        Assert.Equal(-1, actual.DragBuntingAbility);
        Assert.Equal(0, actual.HittingDurability);
        Assert.Equal(0, actual.FieldingDurability);
        Assert.Equal(1, actual.FieldingAbility);
        Assert.Equal(2, actual.HitsPerBf);
        Assert.Equal(3, actual.HrPerBf);
        Assert.Equal(4, actual.KPerBf);
        Assert.Equal(5, actual.PitchingClutch);
        Assert.Equal(6, actual.PowerLeft);
        Assert.Equal(7, actual.PowerRight);
        Assert.Equal(8, actual.ReactionTime);
        Assert.Equal(9, actual.Speed);
        Assert.Equal(10, actual.Stamina);
        Assert.Equal(11, actual.BaseRunningAbility);
        Assert.Equal(12, actual.PitchVelocity);
        Assert.Equal(13, actual.PlateVision);
    }

    [Fact]
    public void Map_AttributeChangeCollectionAlternateNames_ReturnsCombinedAttributeChange()
    {
        // Arrange
        var attributeChanges = new List<AttributeChangeDto>()
        {
            new(Name: "ARM ACC", CurrentValue: "67", Delta: "-3", Direction: "", Color: ""),
            new(Name: "ARM STR", CurrentValue: "68", Delta: "-2", Direction: "", Color: ""),
            new(Name: "CLU", CurrentValue: "69", Delta: "-1", Direction: "", Color: ""),
            new(Name: "PCLU", CurrentValue: "70", Delta: "0", Direction: "", Color: ""),
            new(Name: "PWR L", CurrentValue: "71", Delta: "1", Direction: "", Color: ""),
            new(Name: "PWR R", CurrentValue: "72", Delta: "2", Direction: "", Color: ""),
            new(Name: "STL", CurrentValue: "73", Delta: "3", Direction: "", Color: ""),
        };
        var mapper = new MlbTheShowRosterUpdateMapper(Mock.Of<IMlbTheShowItemMapper>());

        // Act
        var actual = mapper.Map(attributeChanges);

        // Assert
        Assert.Equal(-3, actual.ArmAccuracy);
        Assert.Equal(-2, actual.ArmStrength);
        Assert.Equal(-1, actual.BattingClutch);
        Assert.Equal(0, actual.PitchingClutch);
        Assert.Equal(1, actual.PowerLeft);
        Assert.Equal(2, actual.PowerRight);
        Assert.Equal(3, actual.BaseRunningAbility);
    }
}