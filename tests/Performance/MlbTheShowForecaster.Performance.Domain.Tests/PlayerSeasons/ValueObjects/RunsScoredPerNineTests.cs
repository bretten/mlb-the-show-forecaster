using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects;

public class RunsScoredPerNineTests
{
    [Fact]
    public void Value_PitchingStats_ReturnsCalculatedValue()
    {
        // Arrange
        const uint runsAllowed = 50;
        const decimal inningsPitched = 132;
        var runsScoredPerNine = RunsScoredPerNine.Create(runsAllowed, inningsPitched);

        // Act
        var actual = runsScoredPerNine.Value;

        // Assert
        Assert.Equal(3.41m, Math.Round(actual, 2, MidpointRounding.AwayFromZero));
    }
}