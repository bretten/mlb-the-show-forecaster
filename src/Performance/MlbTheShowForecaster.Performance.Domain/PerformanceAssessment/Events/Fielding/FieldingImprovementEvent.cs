using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.PerformanceChanges;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Fielding;

/// <summary>
/// Published when there is an improvement in fielding performance
/// </summary>
public sealed record FieldingImprovementEvent(IPerformanceChange Change) : IPerformanceChangeEvent;