using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums.Extensions;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.Tests.Enums;

public class PositionExtensionsTests
{
    [Theory]
    [InlineData(Position.Pitcher)]
    [InlineData(Position.StartingPitcher)]
    [InlineData(Position.ReliefPitcher)]
    [InlineData(Position.ClosingPitcher)]
    public void IsOnlyPitcher_PitchingPosition_ReturnsTrue(Position p)
    {
        // Arrange
        var position = p;

        // Act
        var actual = position.IsOnlyPitcher();

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [InlineData(Position.Catcher)]
    [InlineData(Position.FirstBase)]
    [InlineData(Position.SecondBase)]
    [InlineData(Position.ThirdBase)]
    [InlineData(Position.Shortstop)]
    [InlineData(Position.Infield)]
    [InlineData(Position.LeftField)]
    [InlineData(Position.CenterField)]
    [InlineData(Position.RightField)]
    [InlineData(Position.OutField)]
    [InlineData(Position.DesignatedHitter)]
    [InlineData(Position.PinchHitter)]
    [InlineData(Position.TwoWayPlayer)]
    public void IsOnlyPitcher_BattingPosition_ReturnsFalse(Position p)
    {
        // Arrange
        var position = p;

        // Act
        var actual = position.IsOnlyPitcher();

        // Assert
        Assert.False(actual);
    }

    [Theory]
    [InlineData(Position.Catcher)]
    [InlineData(Position.FirstBase)]
    [InlineData(Position.SecondBase)]
    [InlineData(Position.ThirdBase)]
    [InlineData(Position.Shortstop)]
    [InlineData(Position.Infield)]
    [InlineData(Position.LeftField)]
    [InlineData(Position.CenterField)]
    [InlineData(Position.RightField)]
    [InlineData(Position.OutField)]
    [InlineData(Position.DesignatedHitter)]
    [InlineData(Position.PinchHitter)]
    public void IsOnlyBatter_BattingPosition_ReturnsTrue(Position p)
    {
        // Arrange
        var position = p;

        // Act
        var actual = position.IsOnlyBatter();

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [InlineData(Position.Pitcher)]
    [InlineData(Position.StartingPitcher)]
    [InlineData(Position.ReliefPitcher)]
    [InlineData(Position.ClosingPitcher)]
    [InlineData(Position.TwoWayPlayer)]
    public void IsOnlyBatter_PitchingPosition_ReturnsFalse(Position p)
    {
        // Arrange
        var position = p;

        // Act
        var actual = position.IsOnlyBatter();

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void IsTwoWayPlayer_TwoWayPlayer_ReturnsTrue()
    {
        // Arrange
        const Position position = Position.TwoWayPlayer;

        // Act
        var actual = position.IsTwoWayPlayer();

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void IsTwoWayPlayer_NonTwoWayPlayer_ReturnsTrue()
    {
        // Arrange
        const Position position = Position.StartingPitcher;

        // Act
        var actual = position.IsTwoWayPlayer();

        // Assert
        Assert.False(actual);
    }
}