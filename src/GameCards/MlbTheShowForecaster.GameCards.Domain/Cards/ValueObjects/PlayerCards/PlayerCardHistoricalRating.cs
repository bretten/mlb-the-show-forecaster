using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

/// <summary>
/// Represents the previous rating state of a player card
/// </summary>
public sealed class PlayerCardHistoricalRating : ValueObject
{
    /// <summary>
    /// The type of historical rating
    /// </summary>
    public PlayerCardHistoricalRatingType Type { get; }

    /// <summary>
    /// The first date the player card had this rating
    /// </summary>
    public DateOnly StartDate { get; }

    /// <summary>
    /// The last date the player card had this rating
    /// </summary>
    public DateOnly? EndDate { get; private set; }

    /// <summary>
    /// The overall rating during this time period
    /// </summary>
    public OverallRating OverallRating { get; }

    /// <summary>
    /// The player attributes during this time period
    /// </summary>
    public PlayerCardAttributes Attributes { get; } = null!;

    /// <summary>
    /// True if the historical rating was a baseline change
    /// </summary>
    public bool IsBaseline => Type == PlayerCardHistoricalRatingType.Baseline;

    /// <summary>
    /// True if the historical rating was a temporary change
    /// </summary>
    public bool IsTemporary => Type == PlayerCardHistoricalRatingType.Temporary;

    /// <summary>
    /// True if the historical rating was the result of a boosted player card
    /// </summary>
    public bool IsBoost => Type == PlayerCardHistoricalRatingType.Boost;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="type">The type of historical rating</param>
    /// <param name="startDate">The first date the player card had this rating</param>
    /// <param name="endDate">The last date the player card had this rating</param>
    /// <param name="overallRating">The overall rating during this time period</param>
    /// <param name="attributes">The player attributes during this time period</param>
    private PlayerCardHistoricalRating(PlayerCardHistoricalRatingType type, DateOnly startDate, DateOnly? endDate,
        OverallRating overallRating, PlayerCardAttributes attributes)
    {
        Type = type;
        StartDate = startDate;
        EndDate = endDate;
        OverallRating = overallRating;
        Attributes = attributes;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="type">The type of historical rating</param>
    /// <param name="startDate">The first date the player card had this rating</param>
    /// <param name="endDate">The last date the player card had this rating</param>
    /// <param name="overallRating">The overall rating during this time period</param>
    private PlayerCardHistoricalRating(PlayerCardHistoricalRatingType type, DateOnly startDate, DateOnly? endDate,
        OverallRating overallRating)
    {
        Type = type;
        StartDate = startDate;
        EndDate = endDate;
        OverallRating = overallRating;
    }

    /// <summary>
    /// Ends the historical rating
    /// </summary>
    /// <param name="endDate">The last date the player card had this rating</param>
    public void End(DateOnly endDate)
    {
        if (EndDate.HasValue)
        {
            throw new PlayerCardHistoricalRatingAlreadyEndedException(
                $"The rating already ended on {EndDate.Value.ToShortDateString()}");
        }

        EndDate = endDate;
    }

    /// <summary>
    /// Creates a <see cref="PlayerCardHistoricalRating"/> with <see cref="Type"/> <see cref="PlayerCardHistoricalRatingType.Baseline"/>
    /// </summary>
    /// <param name="startDate">The first date the player card had this rating</param>
    /// <param name="endDate">The last date the player card had this rating</param>
    /// <param name="overallRating">The overall rating during this time period</param>
    /// <param name="attributes">The player attributes during this time period</param>
    /// <returns><see cref="PlayerCardHistoricalRating"/></returns>
    public static PlayerCardHistoricalRating Baseline(DateOnly startDate, DateOnly? endDate,
        OverallRating overallRating, PlayerCardAttributes attributes)
    {
        return new PlayerCardHistoricalRating(
            PlayerCardHistoricalRatingType.Baseline, startDate, endDate, overallRating, attributes);
    }

    /// <summary>
    /// Creates a <see cref="PlayerCardHistoricalRating"/> with <see cref="Type"/> <see cref="PlayerCardHistoricalRatingType.Temporary"/>
    /// </summary>
    /// <param name="startDate">The first date the player card had this rating</param>
    /// <param name="endDate">The last date the player card had this rating</param>
    /// <param name="overallRating">The overall rating during this time period</param>
    /// <param name="attributes">The player attributes during this time period</param>
    /// <returns><see cref="PlayerCardHistoricalRating"/></returns>
    public static PlayerCardHistoricalRating Temporary(DateOnly startDate, DateOnly? endDate,
        OverallRating overallRating, PlayerCardAttributes attributes)
    {
        return new PlayerCardHistoricalRating(
            PlayerCardHistoricalRatingType.Temporary, startDate, endDate, overallRating, attributes);
    }

    /// <summary>
    /// Creates a <see cref="PlayerCardHistoricalRating"/> with <see cref="Type"/> <see cref="PlayerCardHistoricalRatingType.Boost"/>
    /// </summary>
    /// <param name="startDate">The first date the player card had this rating</param>
    /// <param name="endDate">The last date the player card had this rating</param>
    /// <param name="overallRating">The overall rating during this time period</param>
    /// <param name="attributes">The player attributes during this time period</param>
    /// <returns><see cref="PlayerCardHistoricalRating"/></returns>
    public static PlayerCardHistoricalRating Boost(DateOnly startDate, DateOnly? endDate, OverallRating overallRating,
        PlayerCardAttributes attributes)
    {
        return new PlayerCardHistoricalRating(
            PlayerCardHistoricalRatingType.Boost, startDate, endDate, overallRating, attributes);
    }
}