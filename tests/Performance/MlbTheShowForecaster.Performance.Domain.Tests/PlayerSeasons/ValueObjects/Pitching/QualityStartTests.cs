using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects.Pitching;

public class QualityStartTests
{
    [Fact]
    public void Value_LessThan6InningsPitched_ReturnsFalse()
    {
        // Arrange
        const decimal inningsPitched = 5;
        const uint earnedRuns = 2;
        var qualityStart = QualityStart.Create(inningsPitched, earnedRuns);

        // Act
        var actual = qualityStart.Value;

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Value_MoreThan3EarnedRuns_ReturnsFalse()
    {
        // Arrange
        const decimal inningsPitched = 7;
        const uint earnedRuns = 4;
        var qualityStart = QualityStart.Create(inningsPitched, earnedRuns);

        // Act
        var actual = qualityStart.Value;

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Value_MoreThan6InningsPitchedLessThan3EarnedRuns_ReturnsTrue()
    {
        // Arrange
        const decimal inningsPitched = 6;
        const uint earnedRuns = 3;
        var qualityStart = QualityStart.Create(inningsPitched, earnedRuns);

        // Act
        var actual = qualityStart.Value;

        // Assert
        Assert.True(actual);
    }
}