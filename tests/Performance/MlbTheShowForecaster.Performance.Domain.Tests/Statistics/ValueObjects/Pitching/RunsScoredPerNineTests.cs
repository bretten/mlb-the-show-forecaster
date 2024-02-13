using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Pitching;

public class RunsScoredPerNineTests
{
    [Fact]
    public void Value_RunsInnings_ReturnsCalculatedValue()
    {
        // Arrange
        const int runsAllowed = 50;
        const decimal inningsPitched = 132;
        var runsScoredPerNine = RunsScoredPerNine.Create(runsAllowed, inningsPitched);

        // Act
        var actual = runsScoredPerNine.Value;

        // Assert
        Assert.Equal(3.409m, actual);
        Assert.Equal(50, runsScoredPerNine.RunsAllowed.Value);
        Assert.Equal(132, runsScoredPerNine.InningsPitched.Value);
    }
}