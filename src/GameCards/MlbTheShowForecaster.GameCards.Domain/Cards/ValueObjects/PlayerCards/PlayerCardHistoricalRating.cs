using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

/// <summary>
/// Represents the previous rating state of a player card
/// </summary>
public sealed class PlayerCardHistoricalRating : ValueObject
{
    /// <summary>
    /// The first date the player card had this rating
    /// </summary>
    public DateOnly StartDate { get; }

    /// <summary>
    /// The last date the player card had this rating
    /// </summary>
    public DateOnly EndDate { get; }

    /// <summary>
    /// The overall rating during this time period
    /// </summary>
    public OverallRating OverallRating { get; }

    /// <summary>
    /// The player attributes during this time period
    /// </summary>
    public PlayerCardAttributes Attributes { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="startDate">The first date the player card had this rating</param>
    /// <param name="endDate">The last date the player card had this rating</param>
    /// <param name="overallRating">The overall rating during this time period</param>
    /// <param name="attributes">The player attributes during this time period</param>
    private PlayerCardHistoricalRating(DateOnly startDate, DateOnly endDate, OverallRating overallRating,
        PlayerCardAttributes attributes)
    {
        StartDate = startDate;
        EndDate = endDate;
        OverallRating = overallRating;
        Attributes = attributes;
    }

    /// <summary>
    /// Creates a <see cref="PlayerCardHistoricalRating"/>
    /// </summary>
    /// <param name="startDate">The first date the player card had this rating</param>
    /// <param name="endDate">The last date the player card had this rating</param>
    /// <param name="overallRating">The overall rating during this time period</param>
    /// <param name="attributes">The player attributes during this time period</param>
    /// <returns><see cref="PlayerCardHistoricalRating"/></returns>
    public static PlayerCardHistoricalRating Create(DateOnly startDate, DateOnly endDate, OverallRating overallRating,
        PlayerCardAttributes attributes)
    {
        return new PlayerCardHistoricalRating(startDate, endDate, overallRating, attributes);
    }
}