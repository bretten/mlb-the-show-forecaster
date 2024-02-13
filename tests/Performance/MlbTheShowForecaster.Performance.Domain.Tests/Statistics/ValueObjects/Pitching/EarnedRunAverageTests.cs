using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Pitching;

public class EarnedRunAverageTests
{
    [Fact]
    public void Value_EarnedRunsInningsPitched_ReturnsCalculatedValue()
    {
        // Arrange
        const int earnedRuns = 46;
        const decimal inningsPitched = 132m;
        var earnedRunAverage = EarnedRunAverage.Create(earnedRuns, inningsPitched);

        // Act
        var actual = earnedRunAverage.Value;

        // Assert
        Assert.Equal(3.14m, actual);
        Assert.Equal(46, earnedRunAverage.EarnedRuns.Value);
        Assert.Equal(132m, earnedRunAverage.InningsPitched.Value);
    }
}