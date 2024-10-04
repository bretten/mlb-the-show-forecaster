using System.Collections.Immutable;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

/// <summary>
/// Represents the forecast of a <see cref="PlayerCard"/>'s demand
/// </summary>
public sealed class PlayerCardForecast : AggregateRoot
{
    /// <summary>
    /// Any <see cref="ForecastImpact"/> that has or had influence over the forecast
    /// </summary>
    private readonly List<ForecastImpact> _forecastImpacts;

    /// <summary>
    /// The year of MLB The Show
    /// </summary>
    public SeasonYear Year { get; }

    /// <summary>
    /// The card ID from MLB The Show
    /// </summary>
    public CardExternalId CardExternalId { get; }

    /// <summary>
    /// The MLB ID of the Player
    /// </summary>
    public MlbId? MlbId { get; }

    /// <summary>
    /// Player's primary position
    /// </summary>
    public Position PrimaryPosition { get; private set; }

    /// <summary>
    /// The overall rating of the card
    /// </summary>
    public OverallRating OverallRating { get; private set; }

    /// <summary>
    /// All <see cref="ForecastImpact"/>s in chronological order of <see cref="ForecastImpact.EndDate"/>
    /// </summary>
    public IReadOnlyList<ForecastImpact> ForecastImpactsChronologically =>
        _forecastImpacts.OrderBy(x => x.StartDate).ToImmutableList();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="year">The year of MLB The Show</param>
    /// <param name="cardExternalId">The card ID from MLB The Show</param>
    /// <param name="mlbId">The MLB ID of the Player</param>
    /// <param name="primaryPosition">Player's primary position</param>
    /// <param name="overallRating">The overall rating of the card</param>
    private PlayerCardForecast(SeasonYear year, CardExternalId cardExternalId, MlbId? mlbId, Position primaryPosition,
        OverallRating overallRating) : base(Guid.NewGuid())
    {
        Year = year;
        CardExternalId = cardExternalId;
        MlbId = mlbId;
        PrimaryPosition = primaryPosition;
        OverallRating = overallRating;
        _forecastImpacts = new List<ForecastImpact>();
    }

    /// <summary>
    /// Reassesses the effect of the <see cref="ForecastImpact"/> on the forecast
    /// </summary>
    /// <param name="impact"><see cref="ForecastImpact"/></param>
    /// <param name="date">The date that the demand should be estimated for: <see cref="EstimateDemandFor"/></param>
    public void Reassess(ForecastImpact impact, DateOnly date)
    {
        var oldDemand = EstimateDemandFor(date);

        _forecastImpacts.Add(impact);

        var newDemand = EstimateDemandFor(date);

        if (newDemand > oldDemand)
        {
            RaiseDemandIncreasedEvent();
        }
        else if (newDemand < oldDemand)
        {
            RaiseDemandDecreasedEvent();
        }
    }

    /// <summary>
    /// Reassesses the effect of a player's position change on the forecast
    /// </summary>
    /// <param name="impact"><see cref="PositionChangeForecastImpact"/></param>
    /// <param name="date">The date that the demand should be estimated for: <see cref="EstimateDemandFor"/></param>
    public void Reassess(PositionChangeForecastImpact impact, DateOnly date)
    {
        PrimaryPosition = impact.NewPosition;

        Reassess(impact as ForecastImpact, date);
    }

    /// <summary>
    /// Reassesses the effect of the batting stats on the forecast
    /// </summary>
    /// <param name="impact"><see cref="BattingStatsForecastImpact"/></param>
    /// <param name="date">The date that the demand should be estimated for: <see cref="EstimateDemandFor"/></param>
    public void Reassess(BattingStatsForecastImpact impact, DateOnly date)
    {
        if (!IsBatter())
        {
            // No domain events triggered for non-batters
            return;
        }

        Reassess(impact as ForecastImpact, date);
    }

    /// <summary>
    /// Reassesses the effect of the pitching stats on the forecast
    /// </summary>
    /// <param name="impact"><see cref="PitchingStatsForecastImpact"/></param>
    /// <param name="date">The date that the demand should be estimated for: <see cref="EstimateDemandFor"/></param>
    public void Reassess(PitchingStatsForecastImpact impact, DateOnly date)
    {
        if (!IsPitcher())
        {
            // No domain events triggered for non-batters
            return;
        }

        Reassess(impact as ForecastImpact, date);
    }

    /// <summary>
    /// Reassesses the effect of an overall rating change
    /// </summary>
    /// <param name="impact"><see cref="OverallRatingChangeForecastImpact"/></param>
    /// <param name="date">The date that the demand should be estimated for: <see cref="EstimateDemandFor"/></param>
    public void Reassess(OverallRatingChangeForecastImpact impact, DateOnly date)
    {
        if (!impact.RarityImproved && !impact.RarityDeclined)
        {
            // No domain events triggered for negligible changes
            return;
        }

        Reassess(impact as ForecastImpact, date);
    }


    /// <summary>
    /// Estimates the demand on the specified date
    /// </summary>
    /// <param name="date">Any <see cref="ForecastImpact"/> whose influence ended before this inclusive date is not used in calculating the demand</param>
    /// <returns>The demand for the forecast on the specified date</returns>
    public Demand EstimateDemandFor(DateOnly date)
    {
        return _forecastImpacts
            .Where(x => x.StartDate <= date && x.EndDate >= date)
            .Select(x => x.DemandOn(date))
            .Aggregate(Demand.Stable(), (x, y) => x + y);
    }

    /// <summary>
    /// True if the player of this forecast is a pitcher, otherwise false
    /// </summary>
    private bool IsPitcher()
    {
        return PrimaryPosition is Position.Pitcher or Position.StartingPitcher or Position.ReliefPitcher
            or Position.ClosingPitcher;
    }

    /// <summary>
    /// True if the player of this forecast is a batter, otherwise false
    /// </summary>
    private bool IsBatter()
    {
        return (!IsPitcher() && PrimaryPosition is not Position.None) || PrimaryPosition is Position.TwoWayPlayer;
    }

    /// <summary>
    /// Raises a domain event indicating that the card's demand has increased
    /// </summary>
    private void RaiseDemandIncreasedEvent()
    {
        RaiseDomainEvent(new CardDemandIncreasedEvent(Year, CardExternalId));
    }

    /// <summary>
    /// Raises a domain event indicating that the card's demand has decreased
    /// </summary>
    private void RaiseDemandDecreasedEvent()
    {
        RaiseDomainEvent(new CardDemandDecreasedEvent(Year, CardExternalId));
    }

    /// <summary>
    /// Creates a <see cref="PlayerCardForecast"/>
    /// </summary>
    /// <param name="year">The year of MLB The Show</param>
    /// <param name="cardExternalId">The card ID from MLB The Show</param>
    /// <param name="mlbId">The MLB ID of the Player</param>
    /// <param name="primaryPosition">Player's primary position</param>
    /// <param name="overallRating">The overall rating of the card</param>
    /// <returns><see cref="PlayerCardForecast"/></returns>
    public static PlayerCardForecast Create(SeasonYear year, CardExternalId cardExternalId, MlbId? mlbId,
        Position primaryPosition, OverallRating overallRating)
    {
        return new PlayerCardForecast(year, cardExternalId, mlbId, primaryPosition, overallRating);
    }
}