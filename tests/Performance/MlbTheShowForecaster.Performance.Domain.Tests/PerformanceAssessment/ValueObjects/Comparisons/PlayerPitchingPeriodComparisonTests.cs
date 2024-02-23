using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.Comparisons;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PerformanceAssessment.ValueObjects.Comparisons;

public class PlayerPitchingPeriodComparisonTests
{
    [Fact]
    public void Create_InningsPitchedAndBattersFacedAndEarnedRunAverage_Created()
    {
        // Arrange
        var playerMlbId = MlbId.Create(1);
        var comparisonDate = new DateTime(2024, 4, 1);
        const decimal inningsPitchedBeforeComparisonDate = 132.0m;
        const int battersFacedBeforeComparisonDate = 531;
        const decimal earnedRunAverageBeforeComparisonDate = 3.14m;
        const decimal inningsPitchedSinceComparisonDate = 30.0m;
        const int battersFacedSinceComparisonDate = 121;
        const decimal earnedRunAverageSinceComparisonDate = 2.21m;

        // Act
        var actual = PlayerPitchingPeriodComparison.Create(playerMlbId, comparisonDate,
            inningsPitchedBeforeComparisonDate: inningsPitchedBeforeComparisonDate,
            battersFacedBeforeComparisonDate: battersFacedBeforeComparisonDate,
            earnedRunAverageBeforeComparisonDate: earnedRunAverageBeforeComparisonDate,
            inningsPitchedSinceComparisonDate: inningsPitchedSinceComparisonDate,
            battersFacedSinceComparisonDate: battersFacedSinceComparisonDate,
            earnedRunAverageSinceComparisonDate: earnedRunAverageSinceComparisonDate
        );

        // Assert
        Assert.Equal(1, actual.PlayerMlbId.Value);
        Assert.Equal(new DateTime(2024, 4, 1), actual.ComparisonDate);
        Assert.Equal(132.0m, actual.InningsPitchedBeforeComparisonDate.Value);
        Assert.Equal(531, actual.BattersFacedBeforeComparisonDate.Value);
        Assert.Equal(3.14m, actual.EarnedRunAverageBeforeComparisonDate.Value);
        Assert.Equal(30.0m, actual.InningsPitchedSinceComparisonDate.Value);
        Assert.Equal(121, actual.BattersFacedSinceComparisonDate.Value);
        Assert.Equal(2.21m, actual.EarnedRunAverageSinceComparisonDate.Value);
        Assert.Equal(-29.62m, actual.PercentageChange);
    }
}