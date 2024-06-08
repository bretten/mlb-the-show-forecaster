using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;

/// <summary>
/// Represents the result of <see cref="IPerformanceAssessor"/>
/// </summary>
/// <param name="Score">The score derived by <see cref="IPerformanceAssessor"/></param>
public sealed record PerformanceAssessmentResult(PerformanceScore Score);