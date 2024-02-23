using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.Comparisons;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Batting;

/// <summary>
/// Published when a player has an improvement in their batting performance during a period of time
/// </summary>
/// <param name="Comparison">A comparison of their performance before the improvement period and during the improvement period</param>
public sealed record BattingImprovementEvent(PlayerBattingPeriodComparison Comparison) : IDomainEvent;