using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;

/// <summary>
/// Represents the impact the player's stats has on the <see cref="PlayerCardForecast"/>
/// </summary>
/// <param name="oldScore">The old performance score</param>
/// <param name="newScore">The new performance score</param>
/// <param name="endDate"><inheritdoc /></param>
public abstract class StatsForecastImpact(decimal oldScore, decimal newScore, DateOnly endDate)
    : ForecastImpact(endDate)
{
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
    public PercentageChange PercentageChange => PercentageChange.Create(OldScore, NewScore);
}