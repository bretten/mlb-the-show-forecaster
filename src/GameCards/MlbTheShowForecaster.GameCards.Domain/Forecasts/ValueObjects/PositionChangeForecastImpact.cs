using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;

/// <summary>
/// Represents the impact a player's position change has on a <see cref="PlayerCardForecast"/>
/// </summary>
/// <param name="oldPosition">The player's old position</param>
/// <param name="newPosition">The player's new position</param>
/// <param name="endDate"><inheritdoc /></param>
public sealed class PositionChangeForecastImpact(Position oldPosition, Position newPosition, DateOnly endDate)
    : ForecastImpact(endDate)
{
    /// <summary>
    /// The player's old position
    /// </summary>
    public Position OldPosition { get; } = oldPosition;

    /// <summary>
    /// The player's new position
    /// </summary>
    public Position NewPosition { get; } = newPosition;

    /// <inheritdoc />
    public override Demand Demand =>
        IsDesiredPositionPlayer(NewPosition) ? Demand.High() : Demand.Stable();

    /// <summary>
    /// True if the player card's position is sought after in the marketplace
    /// </summary>
    /// <param name="position">The player card's position</param>
    /// <returns>True if the player card's position is sought after in the marketplace</returns>
    private static bool IsDesiredPositionPlayer(Position position)
    {
        return position is Position.Catcher or Position.LeftField or Position.SecondBase;
    }
}