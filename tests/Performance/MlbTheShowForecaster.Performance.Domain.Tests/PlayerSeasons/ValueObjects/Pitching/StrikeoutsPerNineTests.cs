using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects.Pitching;

public class StrikeoutsPerNineTests
{
    [Fact]
    public void Value_StrikeoutsInnings_ReturnsCalculatedValue()
    {
        // Arrange
        const uint strikeouts = 167;
        const decimal inningsPitched = 132;
        var strikeoutsPerNine = StrikeoutsPerNine.Create(strikeouts, inningsPitched);

        // Act
        var actual = strikeoutsPerNine.Value;

        // Assert
        Assert.Equal(11.386m, actual);
        Assert.Equal(167U, strikeoutsPerNine.Strikeouts.Value);
        Assert.Equal(132, strikeoutsPerNine.InningsPitched.Value);
    }
}