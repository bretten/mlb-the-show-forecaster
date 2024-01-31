using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Pitching;

public class BaseOnBallsPerNineTests
{
    [Fact]
    public void Value_WalksInnings_ReturnsCalculatedValue()
    {
        // Arrange
        const uint baseOnBalls = 55;
        const decimal inningsPitched = 132;
        var baseOnBallsPerNine = BaseOnBallsPerNine.Create(baseOnBalls, inningsPitched);

        // Act
        var actual = baseOnBallsPerNine.Value;

        // Assert
        Assert.Equal(3.750m, actual);
        Assert.Equal(55U, baseOnBallsPerNine.BaseOnBalls.Value);
        Assert.Equal(132, baseOnBallsPerNine.InningsPitched.Value);
    }
}