using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.Comparisons;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PerformanceAssessment.ValueObjects.Comparisons;

public class PlayerFieldingPeriodComparisonTests
{
    [Fact]
    public void Create_TotalChancesAndFieldingPercentage_Created()
    {
        // Arrange
        var playerMlbId = MlbId.Create(1);
        var comparisonDate = new DateTime(2024, 4, 1);
        const int totalChancesBeforeComparisonDate = 423;
        const decimal fieldingPercentageBeforeComparisonDate = 0.931m;
        const int totalChancesSinceComparisonDate = 53;
        const decimal fieldingPercentageSinceComparisonDate = 0.983m;

        // Act
        var actual = PlayerFieldingPeriodComparison.Create(playerMlbId, comparisonDate,
            totalChancesBeforeComparisonDate: totalChancesBeforeComparisonDate,
            fieldingPercentageBeforeComparisonDate: fieldingPercentageBeforeComparisonDate,
            totalChancesSinceComparisonDate: totalChancesSinceComparisonDate,
            fieldingPercentageSinceComparisonDate: fieldingPercentageSinceComparisonDate
        );

        Assert.Equal(1, actual.PlayerMlbId.Value);
        Assert.Equal(new DateTime(2024, 4, 1), actual.ComparisonDate);
        Assert.Equal(423, actual.TotalChancesBeforeComparisonDate.Value);
        Assert.Equal(0.931m, actual.FieldingPercentageBeforeComparisonDate.Value);
        Assert.Equal(53, actual.TotalChancesSinceComparisonDate.Value);
        Assert.Equal(0.983m, actual.FieldingPercentageSinceComparisonDate.Value);
        Assert.Equal(5.59m, actual.PercentageChangeValue);
    }
}