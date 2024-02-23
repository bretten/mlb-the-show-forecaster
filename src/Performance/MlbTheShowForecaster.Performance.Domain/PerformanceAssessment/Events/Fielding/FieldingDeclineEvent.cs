﻿using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.Comparisons;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Fielding;

/// <summary>
/// Published when a player has a decline in their fielding performance during a period of time
/// </summary>
/// <param name="Comparison">A comparison of their performance before the decline period and during the decline period</param>
public sealed record FieldingDeclineEvent(PlayerFieldingPeriodComparison Comparison) : IDomainEvent;