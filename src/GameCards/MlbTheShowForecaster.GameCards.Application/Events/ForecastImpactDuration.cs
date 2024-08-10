using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events;

/// <summary>
/// Represents the duration (in days) of a <see cref="ForecastImpact"/>
/// </summary>
public class ForecastImpactDuration
{
    /// <summary>
    /// Duration in days of a <see cref="BoostForecastImpact"/>
    /// </summary>
    public int Boost { get; }

    /// <summary>
    /// Duration in days of a <see cref="BattingStatsForecastImpact"/>
    /// </summary>
    public int BattingStatsChange { get; }

    /// <summary>
    /// Duration in days of a <see cref="PitchingStatsForecastImpact"/>
    /// </summary>
    public int PitchingStatsChange { get; }

    /// <summary>
    /// Duration in days of a <see cref="FieldingStatsForecastImpact"/>
    /// </summary>
    public int FieldingStatsChange { get; }

    /// <summary>
    /// Duration in days of a <see cref="OverallRatingChangeForecastImpact"/>
    /// </summary>
    public int OverallRatingChange { get; }

    /// <summary>
    /// Duration in days of a <see cref="PositionChangeForecastImpact"/>
    /// </summary>
    public int PositionChange { get; }

    /// <summary>
    /// Duration in days of a <see cref="PlayerActivationForecastImpact"/>
    /// </summary>
    public int PlayerActivation { get; }

    /// <summary>
    /// Duration in days of a <see cref="PlayerDeactivationForecastImpact"/>
    /// </summary>
    public int PlayerDeactivation { get; }

    /// <summary>
    /// Duration in days of a <see cref="PlayerFreeAgencyForecastImpact"/>
    /// </summary>
    public int PlayerFreeAgency { get; }

    /// <summary>
    /// Duration in days of a <see cref="PlayerTeamSigningForecastImpact"/>
    /// </summary>
    public int PlayerTeamSigning { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="boost">Duration in days of a <see cref="BoostForecastImpact"/></param>
    /// <param name="battingStatsChange">Duration in days of a <see cref="BattingStatsForecastImpact"/></param>
    /// <param name="pitchingStatsChange">Duration in days of a <see cref="PitchingStatsForecastImpact"/></param>
    /// <param name="fieldingStatsChange">Duration in days of a <see cref="FieldingStatsForecastImpact"/></param>
    /// <param name="overallRatingChange">Duration in days of a <see cref="OverallRatingChangeForecastImpact"/></param>
    /// <param name="positionChange">Duration in days of a <see cref="PositionChangeForecastImpact"/></param>
    /// <param name="playerActivation">Duration in days of a <see cref="PlayerActivationForecastImpact"/></param>
    /// <param name="playerDeactivation">Duration in days of a <see cref="PlayerDeactivationForecastImpact"/></param>
    /// <param name="playerFreeAgency">Duration in days of a <see cref="PlayerFreeAgencyForecastImpact"/></param>
    /// <param name="playerTeamSigning">Duration in days of a <see cref="PlayerTeamSigningForecastImpact"/></param>
    public ForecastImpactDuration(int boost, int battingStatsChange, int pitchingStatsChange, int fieldingStatsChange,
        int overallRatingChange, int positionChange, int playerActivation, int playerDeactivation, int playerFreeAgency,
        int playerTeamSigning)
    {
        Boost = boost;
        BattingStatsChange = battingStatsChange;
        PitchingStatsChange = pitchingStatsChange;
        FieldingStatsChange = fieldingStatsChange;
        OverallRatingChange = overallRatingChange;
        PositionChange = positionChange;
        PlayerActivation = playerActivation;
        PlayerDeactivation = playerDeactivation;
        PlayerFreeAgency = playerFreeAgency;
        PlayerTeamSigning = playerTeamSigning;
    }
}