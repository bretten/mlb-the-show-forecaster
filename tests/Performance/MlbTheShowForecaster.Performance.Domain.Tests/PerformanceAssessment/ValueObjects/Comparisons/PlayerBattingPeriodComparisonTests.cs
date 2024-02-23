using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.Comparisons;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PerformanceAssessment.ValueObjects.Comparisons;

public class PlayerBattingPeriodComparisonTests
{
    [Fact]
    public void Create_PlateAppearancesAndOps_Created()
    {
        // Arrange
        var playerId = MlbId.Create(1);
        var comparisonDate = new DateTime(2024, 4, 1);
        const int plateAppearancesBeforeComparisonDate = 536;
        const decimal onBasePlusSluggingBeforeComparisonDate = 1.013m;
        const int plateAppearancesSinceComparisonDate = 125;
        const decimal onBasePlusSluggingSinceComparisonDate = 1.066m;

        // Act
        var actual = PlayerBattingPeriodComparison.Create(playerId, comparisonDate,
            plateAppearancesBeforeComparisonDate: plateAppearancesBeforeComparisonDate,
            onBasePlusSluggingBeforeComparisonDate: onBasePlusSluggingBeforeComparisonDate,
            plateAppearancesSinceComparisonDate: plateAppearancesSinceComparisonDate,
            onBasePlusSluggingSinceComparisonDate: onBasePlusSluggingSinceComparisonDate
        );

        // Assert
        Assert.Equal(1, actual.PlayerMlbId.Value);
        Assert.Equal(new DateTime(2024, 4, 1), actual.ComparisonDate);
        Assert.Equal(536, actual.PlateAppearancesBeforeComparisonDate.Value);
        Assert.Equal(1.013m, actual.OnBasePlusSluggingBeforeComparisonDate.Value);
        Assert.Equal(125, actual.PlateAppearancesSinceComparisonDate.Value);
        Assert.Equal(1.066m, actual.OnBasePlusSluggingSinceComparisonDate.Value);
        Assert.Equal(5.23m, actual.PercentageChange);
    }
}