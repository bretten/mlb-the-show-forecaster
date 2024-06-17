using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PerformanceAssessment.TestClasses;

public static class Faker
{
    public static PerformanceScore FakePerformanceScore(decimal score = 0.5m)
    {
        return PerformanceScore.Create(score);
    }

    public static PerformanceScoreComparison FakePerformanceScoreComparison()
    {
        return PerformanceScoreComparison.Create(FakePerformanceScore(), FakePerformanceScore(), 1m);
    }

    public static PerformanceScoreComparison FakePerformanceScoreComparisonIncrease()
    {
        return PerformanceScoreComparison.Create(FakePerformanceScore(0.1m), FakePerformanceScore(0.9m), 0.1m);
    }

    public static PerformanceScoreComparison FakePerformanceScoreComparisonDecrease()
    {
        return PerformanceScoreComparison.Create(FakePerformanceScore(0.9m), FakePerformanceScore(0.1m), 0.1m);
    }

    public static PerformanceAssessmentResult FakePerformanceAssessmentResult(decimal score = 0.5m)
    {
        return new PerformanceAssessmentResult(FakePerformanceScore(score));
    }
}