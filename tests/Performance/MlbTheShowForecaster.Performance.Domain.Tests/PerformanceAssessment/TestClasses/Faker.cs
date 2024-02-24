using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.Comparisons;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PerformanceAssessment.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static PlayerBattingPeriodComparison FakePlayerBattingPeriodComparison(int playerMlbId = 1,
        DateTime? comparisonDate = null, int plateAppearancesBeforeComparisonDate = 0,
        decimal onBasePlusSluggingBeforeComparisonDate = 0, int plateAppearancesSinceComparisonDate = 0,
        decimal onBasePlusSluggingSinceComparisonDate = 0)
    {
        return PlayerBattingPeriodComparison.Create(MlbId.Create(playerMlbId),
            comparisonDate ?? new DateTime(2024, 4, 1),
            plateAppearancesBeforeComparisonDate: plateAppearancesBeforeComparisonDate,
            onBasePlusSluggingBeforeComparisonDate: onBasePlusSluggingBeforeComparisonDate,
            plateAppearancesSinceComparisonDate: plateAppearancesSinceComparisonDate,
            onBasePlusSluggingSinceComparisonDate: onBasePlusSluggingSinceComparisonDate
        );
    }
}