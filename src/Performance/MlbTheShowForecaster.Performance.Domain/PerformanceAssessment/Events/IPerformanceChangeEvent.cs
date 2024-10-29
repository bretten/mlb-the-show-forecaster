using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events;

/// <summary>
/// Published when something has a change in its performance
/// </summary>
public interface IPerformanceChangeEvent : IDomainEvent
{
    /// <summary>
    /// The season of the performance change
    /// </summary>
    public SeasonYear Year { get; }

    /// <summary>
    /// The <see cref="Common.Domain.ValueObjects.MlbId"/> of the entity who had the performance change
    /// </summary>
    public MlbId MlbId { get; }

    /// <summary>
    /// A comparison of the previous and new performance
    /// </summary>
    public PerformanceScoreComparison Comparison { get; }

    /// <summary>
    /// The date
    /// </summary>
    public DateOnly Date { get; }
}