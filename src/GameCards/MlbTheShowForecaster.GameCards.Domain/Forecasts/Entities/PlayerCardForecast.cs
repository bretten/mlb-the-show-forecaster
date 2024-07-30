using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

/// <summary>
/// Represents the forecast of a <see cref="PlayerCard"/>'s demand
/// </summary>
public sealed class PlayerCardForecast : AggregateRoot
{
    /// <summary>
    /// Determines the threshold that a performance score must change by in order to be considered significant
    /// </summary>
    private const decimal PerformanceScoreChangeThreshold = 40m;

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
    public MlbId MlbId { get; }

    /// <summary>
    /// Player's primary position
    /// </summary>
    public Position PrimaryPosition { get; }

    /// <summary>
    /// The overall rating of the card
    /// </summary>
    public OverallRating OverallRating { get; }

    /// <summary>
    /// The effect of a card attribute boost on the forecast
    /// </summary>
    public BoostForecastImpact? BoostImpact { get; private set; }

    /// <summary>
    /// The effect of a player card's position change on the forecast
    /// </summary>
    public PositionChangeForecastImpact? PositionChangeImpact { get; private set; }

    /// <summary>
    /// The effect of a price change on the forecast
    /// </summary>
    public PriceForecastImpact? PriceImpact { get; private set; }

    /// <summary>
    /// The effect of the player's batting stats on the forecast
    /// </summary>
    public BattingStatsForecastImpact? BattingStatsImpact { get; private set; }

    /// <summary>
    /// The effect of the player's pitching stats on the forecast
    /// </summary>
    public PitchingStatsForecastImpact? PitchingStatsImpact { get; private set; }

    /// <summary>
    /// The effect of the player's fielding stats on the forecast
    /// </summary>
    public FieldingStatsForecastImpact? FieldingStatsImpact { get; private set; }

    /// <summary>
    /// The effect of the player being activated has on the forecast
    /// </summary>
    public PlayerActivationForecastImpact? ActivationImpact { get; private set; }

    /// <summary>
    /// The effect of the player being deactivated has on the forecast
    /// </summary>
    public PlayerDeactivationForecastImpact? DeactivationImpact { get; private set; }

    /// <summary>
    /// The effect of the player entering free agency has on the forecast
    /// </summary>
    public PlayerFreeAgencyForecastImpact? FreeAgencyImpact { get; private set; }

    /// <summary>
    /// The effect of the player signing with a team has on the forecast
    /// </summary>
    public PlayerTeamSigningForecastImpact? TeamSigningImpact { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="year">The year of MLB The Show</param>
    /// <param name="cardExternalId">The card ID from MLB The Show</param>
    /// <param name="mlbId">The MLB ID of the Player</param>
    /// <param name="primaryPosition">Player's primary position</param>
    /// <param name="overallRating">The overall rating of the card</param>
    /// <param name="boostImpact">The effect of a card attribute boost on the forecast</param>
    /// <param name="positionChangeImpact">The effect of a player card's position change on the forecast</param>
    /// <param name="priceImpact">The effect of a price change on the forecast</param>
    /// <param name="battingStatsImpact">The effect of the player's batting stats on the forecast</param>
    /// <param name="pitchingStatsImpact">The effect of the player's pitching stats on the forecast</param>
    /// <param name="fieldingStatsImpact">The effect of the player's fielding stats on the forecast</param>
    /// <param name="activationImpact">The effect of the player being activated has on the forecast</param>
    /// <param name="deactivationImpact">The effect of the player being deactivated has on the forecast</param>
    /// <param name="freeAgencyImpact">The effect of the player entering free agency has on the forecast</param>
    /// <param name="teamSigningImpact">The effect of the player signing with a team has on the forecast</param>
    private PlayerCardForecast(SeasonYear year, CardExternalId cardExternalId, MlbId mlbId, Position primaryPosition,
        OverallRating overallRating, BoostForecastImpact? boostImpact,
        PositionChangeForecastImpact? positionChangeImpact, PriceForecastImpact? priceImpact,
        BattingStatsForecastImpact? battingStatsImpact, PitchingStatsForecastImpact? pitchingStatsImpact,
        FieldingStatsForecastImpact? fieldingStatsImpact, PlayerActivationForecastImpact? activationImpact,
        PlayerDeactivationForecastImpact? deactivationImpact, PlayerFreeAgencyForecastImpact? freeAgencyImpact,
        PlayerTeamSigningForecastImpact? teamSigningImpact) : base(Guid.NewGuid())
    {
        Year = year;
        CardExternalId = cardExternalId;
        MlbId = mlbId;
        PrimaryPosition = primaryPosition;
        OverallRating = overallRating;
        BoostImpact = boostImpact;
        PositionChangeImpact = positionChangeImpact;
        PriceImpact = priceImpact;
        BattingStatsImpact = battingStatsImpact;
        PitchingStatsImpact = pitchingStatsImpact;
        FieldingStatsImpact = fieldingStatsImpact;
        ActivationImpact = activationImpact;
        DeactivationImpact = deactivationImpact;
        FreeAgencyImpact = freeAgencyImpact;
        TeamSigningImpact = teamSigningImpact;
    }

    /// <summary>
    /// Reassesses the effect of a boost on the forecast
    /// </summary>
    /// <param name="impact"><see cref="BoostForecastImpact"/></param>
    public void Reassess(BoostForecastImpact impact)
    {
        BoostImpact = impact;

        RaiseDemandIncreasedEvent();
    }

    /// <summary>
    /// Reassesses the effect of a player's position change on the forecast
    /// </summary>
    /// <param name="impact"><see cref="PositionChangeForecastImpact"/></param>
    public void Reassess(PositionChangeForecastImpact impact)
    {
        PositionChangeImpact = impact;

        if (IsDesiredPositionPlayer(impact.NewPosition))
        {
            RaiseDemandIncreasedEvent();
        }
    }

    /// <summary>
    /// Reassesses the effect of the card's price on the forecast
    /// </summary>
    /// <param name="impact"><see cref="PriceForecastImpact"/></param>
    public void Reassess(PriceForecastImpact impact)
    {
        // Need some trend prediction here
    }

    /// <summary>
    /// Reassesses the effect of the batting stats on the forecast
    /// </summary>
    /// <param name="impact"><see cref="BattingStatsForecastImpact"/></param>
    public void Reassess(BattingStatsForecastImpact impact)
    {
        BattingStatsImpact = impact;

        if (!IsBatter())
        {
            // No domain events triggered for non-batters
            return;
        }

        ReassessStatChange(impact.PercentageChange);
    }

    /// <summary>
    /// Reassesses the effect of the pitching stats on the forecast
    /// </summary>
    /// <param name="impact"><see cref="PitchingStatsForecastImpact"/></param>
    public void Reassess(PitchingStatsForecastImpact impact)
    {
        PitchingStatsImpact = impact;

        if (!IsPitcher())
        {
            // No domain events triggered for non-pitchers
            return;
        }

        ReassessStatChange(impact.PercentageChange);
    }

    /// <summary>
    /// Reassesses the effect of the fielding stats on the forecast
    /// </summary>
    /// <param name="impact"><see cref="FieldingStatsForecastImpact"/></param>
    public void Reassess(FieldingStatsForecastImpact impact)
    {
        FieldingStatsImpact = impact;

        ReassessStatChange(impact.PercentageChange);
    }

    /// <summary>
    /// Reassesses the effect of the player being activated has on the forecast
    /// </summary>
    /// <param name="impact"><see cref="PlayerActivationForecastImpact"/></param>
    public void Reassess(PlayerActivationForecastImpact impact)
    {
        ActivationImpact = impact;

        RaiseDemandIncreasedEvent();
    }

    /// <summary>
    /// Reassesses the effect of the player being deactivated has on the forecast
    /// </summary>
    /// <param name="impact"><see cref="PlayerDeactivationForecastImpact"/></param>
    public void Reassess(PlayerDeactivationForecastImpact impact)
    {
        DeactivationImpact = impact;

        RaiseDemandDecreasedEvent();
    }

    /// <summary>
    /// Reassesses the effect of the player entering free agency has on the forecast
    /// </summary>
    /// <param name="impact"><see cref="PlayerFreeAgencyForecastImpact"/></param>
    public void Reassess(PlayerFreeAgencyForecastImpact impact)
    {
        FreeAgencyImpact = impact;

        RaiseDemandDecreasedEvent();
    }

    /// <summary>
    /// Reassesses the effect of the player signing with a team has on the forecast
    /// </summary>
    /// <param name="impact"><see cref="PlayerTeamSigningForecastImpact"/></param>
    public void Reassess(PlayerTeamSigningForecastImpact impact)
    {
        TeamSigningImpact = impact;

        RaiseDemandIncreasedEvent();
    }

    /// <summary>
    /// Reassesses a player's performance score change
    /// </summary>
    /// <param name="change">The percentage change in the player's performance score</param>
    private void ReassessStatChange(PercentageChange change)
    {
        switch (change.PercentageChangeValue)
        {
            case > PerformanceScoreChangeThreshold:
                RaiseDemandIncreasedEvent();
                break;
            case < -PerformanceScoreChangeThreshold:
                RaiseDemandDecreasedEvent();
                break;
        }
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
    /// True if the player card's position is sought after in the marketplace
    /// </summary>
    /// <param name="position">The player card's position</param>
    /// <returns>True if the player card's position is sought after in the marketplace</returns>
    private static bool IsDesiredPositionPlayer(Position position)
    {
        return position is Position.Catcher or Position.LeftField or Position.SecondBase;
    }

    /// <summary>
    /// Creates a <see cref="PlayerCardForecast"/>
    /// </summary>
    /// <param name="year">The year of MLB The Show</param>
    /// <param name="cardExternalId">The card ID from MLB The Show</param>
    /// <param name="mlbId">The MLB ID of the Player</param>
    /// <param name="primaryPosition">Player's primary position</param>
    /// <param name="overallRating">The overall rating of the card</param>
    /// <param name="boostImpact">The effect of a card attribute boost on the forecast</param>
    /// <param name="positionChangeImpact">The effect of a player card's position change on the forecast</param>
    /// <param name="priceImpact">The effect of a price change on the forecast</param>
    /// <param name="battingStatsImpact">The effect of the player's batting stats on the forecast</param>
    /// <param name="pitchingStatsImpact">The effect of the player's pitching stats on the forecast</param>
    /// <param name="fieldingStatsImpact">The effect of the player's fielding stats on the forecast</param>
    /// <param name="activationImpact">The effect of the player being activated has on the forecast</param>
    /// <param name="deactivationImpact">The effect of the player being deactivated has on the forecast</param>
    /// <param name="freeAgencyImpact">The effect of the player entering free agency has on the forecast</param>
    /// <param name="teamSigningImpact">The effect of the player signing with a team has on the forecast</param>
    /// <returns><see cref="PlayerCardForecast"/></returns>
    public static PlayerCardForecast Create(SeasonYear year, CardExternalId cardExternalId, MlbId mlbId,
        Position primaryPosition, OverallRating overallRating, BoostForecastImpact? boostImpact,
        PositionChangeForecastImpact? positionChangeImpact, PriceForecastImpact? priceImpact,
        BattingStatsForecastImpact? battingStatsImpact, PitchingStatsForecastImpact? pitchingStatsImpact,
        FieldingStatsForecastImpact? fieldingStatsImpact, PlayerActivationForecastImpact? activationImpact,
        PlayerDeactivationForecastImpact? deactivationImpact, PlayerFreeAgencyForecastImpact? freeAgencyImpact,
        PlayerTeamSigningForecastImpact? teamSigningImpact)
    {
        return new PlayerCardForecast(year, cardExternalId, mlbId, primaryPosition, overallRating, boostImpact,
            positionChangeImpact, priceImpact, battingStatsImpact, pitchingStatsImpact, fieldingStatsImpact,
            activationImpact, deactivationImpact, freeAgencyImpact, teamSigningImpact);
    }
}