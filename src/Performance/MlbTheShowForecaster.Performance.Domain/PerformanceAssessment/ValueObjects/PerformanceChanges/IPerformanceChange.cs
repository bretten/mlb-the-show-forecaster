namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.PerformanceChanges;

/// <summary>
/// Defines something that had a performance change
/// </summary>
public interface IPerformanceChange
{
    /// <summary>
    /// A comparison of the previous and new performance
    /// </summary>
    public PerformanceScoreComparison Comparison { get; }
}