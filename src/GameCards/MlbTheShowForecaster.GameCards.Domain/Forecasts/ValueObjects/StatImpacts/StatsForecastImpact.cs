﻿using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;

/// <summary>
/// Represents the impact the player's stats has on the <see cref="PlayerCardForecast"/>
/// </summary>
/// <param name="oldScore">The old performance score</param>
/// <param name="newScore">The new performance score</param>
/// <param name="startDate"><inheritdoc /></param>
/// <param name="endDate"><inheritdoc /></param>
public abstract class StatsForecastImpact(decimal oldScore, decimal newScore, DateOnly startDate, DateOnly endDate)
    : ForecastImpact(startDate, endDate)
{
    /// <summary>
    /// Determines the threshold that a performance score must change by in order to be considered significant
    /// Value does not need to be configured at runtime since this is based on historical MLB The Show roster changes
    /// </summary>
    private const decimal PerformanceScoreChangeThreshold = 40m;

    /// <summary>
    /// The old performance score
    /// </summary>
    public decimal OldScore { get; } = oldScore;

    /// <summary>
    /// The new performance score
    /// </summary>
    public decimal NewScore { get; } = newScore;

    /// <summary>
    /// The percentage change between their old and new performance scores
    /// </summary>
    public PercentageChange PercentageChange => PercentageChange.Create(OldScore, NewScore, true);

    /// <summary>
    /// True if the new score is an improvement
    /// </summary>
    public bool IsImprovement => PercentageChange.PercentageChangeValue > 0m;

    /// <inheritdoc />
    public override Demand Demand
    {
        get
        {
            if (IsNegligible)
            {
                return Demand.Stable();
            }

            // If the stats improved there will be high demand, otherwise demand will slightly diminish
            return IsImprovement ? Demand.High() : Demand.Loss();
        }
    }

    /// <summary>
    /// The demand only changes when the percentage change between the old and new score passes the
    /// <see cref="PerformanceScoreChangeThreshold"/>
    /// </summary>
    private bool IsNegligible =>
        PercentageChange.PercentageChangeValue is > -PerformanceScoreChangeThreshold
            and < PerformanceScoreChangeThreshold;
}