namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Reports;

/// <summary>
/// An occurrence that affected a player's stat trends
/// </summary>
/// <param name="Start">When the influence began</param>
/// <param name="End">When the influence ended</param>
/// <param name="Description">A description of what happened</param>
public readonly record struct TrendImpact(DateOnly Start, DateOnly End, string Description);