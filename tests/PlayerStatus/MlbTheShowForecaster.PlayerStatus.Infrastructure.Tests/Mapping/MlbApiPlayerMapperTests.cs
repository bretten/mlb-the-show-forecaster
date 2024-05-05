using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Mapping;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Mapping.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Tests.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Tests.Mapping;

public class MlbApiPlayerMapperTests
{
    [Fact]
    public void Map_PlayerDto_ReturnsRosterEntry()
    {
        // Arrange
        var playerDto = Faker.FakePlayerDto(mlbId: 10,
            firstName: "firstN",
            lastName: "lastN",
            birthdate: new DateTime(2024, 4, 1),
            position: new PositionDto("First Base", "1B"),
            mlbDebutDate: new DateTime(2022, 2, 1),
            batSide: new ArmSideDto("S", "Switch"),
            throwArm: new ArmSideDto("R", "Right"),
            team: new CurrentTeamDto(140),
            active: true
        );
        var mapper = new MlbApiPlayerMapper();

        // Act
        var actual = mapper.Map(playerDto);

        // Assert
        Assert.Equal(10, actual.MlbId.Value);
        Assert.Equal("firstN", actual.FirstName.Value);
        Assert.Equal("lastN", actual.LastName.Value);
        Assert.Equal(new DateTime(2024, 4, 1), actual.Birthdate);
        Assert.Equal(Position.FirstBase, actual.Position);
        Assert.Equal(new DateTime(2022, 2, 1), actual.MlbDebutDate);
        Assert.Equal(BatSide.Switch, actual.BatSide);
        Assert.Equal(ThrowArm.Right, actual.ThrowArm);
        Assert.Equal(140, actual.CurrentTeamMlbId.Value);
        Assert.True(actual.Active);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("InvalidPosition")]
    public void MapPosition_InvalidPositionAbbreviation_ThrowsException(string positionAbbreviation)
    {
        // Arrange
        var mapper = new MlbApiPlayerMapper();
        Action action = () => mapper.MapPosition(positionAbbreviation);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidMlbApiPositionException>(actual);
    }

    [Theory]
    [InlineData("P", Position.Pitcher)]
    [InlineData("C", Position.Catcher)]
    [InlineData("1B", Position.FirstBase)]
    [InlineData("2B", Position.SecondBase)]
    [InlineData("3B", Position.ThirdBase)]
    [InlineData("SS", Position.Shortstop)]
    [InlineData("LF", Position.LeftField)]
    [InlineData("CF", Position.CenterField)]
    [InlineData("RF", Position.RightField)]
    [InlineData("OF", Position.OutField)]
    [InlineData("DH", Position.DesignatedHitter)]
    [InlineData("PH", Position.PinchHitter)]
    [InlineData("PR", Position.PinchRunner)]
    [InlineData("TWP", Position.TwoWayPlayer)]
    public void MapPosition_ValidPositionAbbreviation_ReturnsPositionEnum(string positionAbbreviation,
        Position expectedPosition)
    {
        // Arrange
        var mapper = new MlbApiPlayerMapper();

        // Act
        var actual = mapper.MapPosition(positionAbbreviation);

        // Assert
        Assert.Equal(expectedPosition, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("InvalidBatSide")]
    public void MapBatSide_InvalidBatSideCode_ThrowsException(string batSideCode)
    {
        // Arrange
        var mapper = new MlbApiPlayerMapper();
        Action action = () => mapper.MapBatSide(batSideCode);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidMlbApiBatSideCodeException>(actual);
    }

    [Theory]
    [InlineData("R", BatSide.Right)]
    [InlineData("L", BatSide.Left)]
    [InlineData("S", BatSide.Switch)]
    public void MapBatSideCode_ValidBatSideCode_ReturnsBatSideEnum(string batSideCode, BatSide expectedBatSide)
    {
        // Arrange
        var mapper = new MlbApiPlayerMapper();

        // Act
        var actual = mapper.MapBatSide(batSideCode);

        // Assert
        Assert.Equal(expectedBatSide, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("InvalidThrowArm")]
    public void MapThrowArm_InvalidThrowArmCode_ThrowsException(string throwArmCode)
    {
        // Arrange
        var mapper = new MlbApiPlayerMapper();
        Action action = () => mapper.MapThrowArm(throwArmCode);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidMlbApiThrowArmCodeException>(actual);
    }

    [Theory]
    [InlineData("R", ThrowArm.Right)]
    [InlineData("L", ThrowArm.Left)]
    [InlineData("S", ThrowArm.Switch)]
    public void MapThrowArm_ValidThrowArmCode_ReturnsThrowArmEnum(string throwArmCode, ThrowArm expectedThrowArm)
    {
        // Arrange
        var mapper = new MlbApiPlayerMapper();

        // Act
        var actual = mapper.MapThrowArm(throwArmCode);

        // Assert
        Assert.Equal(expectedThrowArm, actual);
    }
}