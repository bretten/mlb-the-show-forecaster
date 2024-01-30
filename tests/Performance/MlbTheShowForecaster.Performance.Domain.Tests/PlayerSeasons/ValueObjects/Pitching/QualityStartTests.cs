using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects.Pitching;

public class QualityStartTests
{
    [Fact]
    public void Value_LessThan6InningsPitched_ReturnsFalse()
    {
        // Arrange
        var inningsPitched = InningsCount.Create(5);
        var earnedRuns = NaturalNumber.Create(2);
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
        var inningsPitched = InningsCount.Create(7);
        var earnedRuns = NaturalNumber.Create(4);
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
        var inningsPitched = InningsCount.Create(6);
        var earnedRuns = NaturalNumber.Create(3);
        var qualityStart = QualityStart.Create(inningsPitched, earnedRuns);

        // Act
        var actual = qualityStart.Value;

        // Assert
        Assert.True(actual);
    }
}