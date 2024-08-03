using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;

/// <summary>
/// Constants for Forecast impacts
///
/// These don't need to be configured at runtime as only relative values matter
/// </summary>
public static class ImpactConstants
{
    /// <summary>
    /// Base additive demand amount
    /// </summary>
    public const int BaseAdditiveDemand = 1;

    /// <summary>
    /// Base subtractive demand amount
    /// </summary>
    public const int BaseSubtractiveDemand = -1;

    /// <summary>
    /// These determine how much demand is affected by each of the corresponding impacts
    /// </summary>
    public static class Coefficients
    {
        /// <summary>
        /// <see cref="OverallRatingChangeForecastImpact"/>
        /// </summary>
        public const int OverallRatingChange = 7;

        /// <summary>
        /// <see cref="BoostForecastImpact"/>
        /// </summary>
        public const int Boost = 10;

        /// <summary>
        /// <see cref="PositionChangeForecastImpact"/>
        /// </summary>
        public const int DesiredPositionChange = 3;

        /// <summary>
        /// <see cref="PriceForecastImpact"/>
        /// </summary>
        public const int Price = 5;

        /// <summary>
        /// <see cref="BattingStatsForecastImpact"/>
        /// </summary>
        public const int BattingStats = 5;

        /// <summary>
        /// <see cref="PitchingStatsForecastImpact"/>
        /// </summary>
        public const int PitchingStats = 5;

        /// <summary>
        /// <see cref="FieldingStatsForecastImpact"/>
        /// </summary>
        public const int FieldingStats = 5;

        /// <summary>
        /// <see cref="PlayerActivationForecastImpact"/>
        /// </summary>
        public const int Activation = 3;

        /// <summary>
        /// <see cref="PlayerDeactivationForecastImpact"/>
        /// </summary>
        public const int Deactivation = 10;

        /// <summary>
        /// <see cref="PlayerFreeAgencyForecastImpact"/>
        /// </summary>
        public const int FreeAgency = 10;

        /// <summary>
        /// <see cref="PlayerTeamSigningForecastImpact"/>
        /// </summary>
        public const int TeamSigning = 3;
    }
}