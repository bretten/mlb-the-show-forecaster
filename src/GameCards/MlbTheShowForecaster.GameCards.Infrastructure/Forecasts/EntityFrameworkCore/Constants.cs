using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Forecasts.EntityFrameworkCore;

/// <summary>
/// Constants for EF in the Forecasts subdomain
/// </summary>
public static class Constants
{
    /// <summary>
    /// The schema name
    /// </summary>
    public const string Schema = "game_cards";

    /// <summary>
    /// Table and column names for <see cref="PlayerCardForecast"/>
    /// </summary>
    public static class PlayerCardForecasts
    {
        /// <summary>
        /// Table name
        /// </summary>
        public const string TableName = "player_card_forecasts";

        /// <summary>
        /// Primary key column name
        /// </summary>
        public const string Id = "id";

        /// <summary>
        /// The year of MLB The Show
        /// </summary>
        public const string Year = "year";

        /// <summary>
        /// The external ID of the card that this forecast is for
        /// </summary>
        public const string CardExternalId = "card_external_id";

        /// <summary>
        /// Player MLB ID column name
        /// </summary>
        public const string PlayerMlbId = "player_mlb_id";

        /// <summary>
        /// The player card's primary position
        /// </summary>
        public const string Position = "position";

        /// <summary>
        /// The overall rating of the card
        /// </summary>
        public const string OverallRating = "overall_rating";

        /// <summary>
        /// Key names for <see cref="PlayerCardForecast"/>
        /// </summary>
        public static class Keys
        {
            /// <summary>
            /// Primary key
            /// </summary>
            public const string PrimaryKey = $"{TableName}_pkey";
        }

        /// <summary>
        /// Index names for <see cref="PlayerCardForecast"/>
        /// </summary>
        public static class Indexes
        {
            /// <summary>
            /// For querying by the card's year and external ID
            /// </summary>
            public const string YearAndExternalId = $"{TableName}_{Year}_{CardExternalId}_idx";

            /// <summary>
            /// For querying by the card's year and MLB ID
            /// </summary>
            public const string YearAndMlbId = $"{TableName}_{Year}_{PlayerMlbId}_idx";
        }

        /// <summary>
        /// Relationship names for <see cref="PlayerCardForecast"/>
        /// </summary>
        public static class Relationships
        {
            /// <summary>
            /// Name of the field that stores the <see cref="ForecastImpact"/>s
            /// </summary>
            public const string ForecastImpactsField = "_forecastImpacts";

            /// <summary>
            /// Name for the <see cref="ForecastImpact"/> discriminator
            /// https://learn.microsoft.com/en-us/ef/core/modeling/inheritance#table-per-hierarchy-and-discriminator-configuration
            /// </summary>
            public const string DiscriminatorName = "type";
        }
    }

    /// <summary>
    /// Table and column names for <see cref="ForecastImpact"/>
    /// </summary>
    public static class ForecastImpacts
    {
        /// <summary>
        /// Table name
        /// </summary>
        public const string TableName = "player_card_forecast_impacts";

        /// <summary>
        /// Foreign key column name that references <see cref="PlayerCardForecast"/>
        /// </summary>
        public const string PlayerCardForecastId = "player_card_forecast_id";

        /// <summary>
        /// End date
        /// </summary>
        public const string EndDate = "end_date";

        /// <summary>
        /// Column names for <see cref="StatsForecastImpact"/>
        /// </summary>
        public static class Stats
        {
            /// <summary>
            /// The old performance score
            /// </summary>
            public const string OldScore = "old_score";

            /// <summary>
            /// The new performance score
            /// </summary>
            public const string NewScore = "new_score";
        }

        /// <summary>
        /// Column names for <see cref="BoostForecastImpact"/>
        /// </summary>
        public static class Boost
        {
            /// <summary>
            /// The boost reason
            /// </summary>
            public const string BoostReason = "boost_reason";
        }

        /// <summary>
        /// Column names for <see cref="OverallRatingChangeForecastImpact"/>
        /// </summary>
        public static class OverallRatingChange
        {
            /// <summary>
            /// Old rating
            /// </summary>
            public const string OldRating = "old_overall_rating";

            /// <summary>
            /// New rating
            /// </summary>
            public const string NewRating = "new_overall_rating";
        }

        /// <summary>
        /// Column names for <see cref="PositionChangeForecastImpact"/>
        /// </summary>
        public static class PositionChange
        {
            /// <summary>
            /// Old position
            /// </summary>
            public const string OldRating = "old_position";

            /// <summary>
            /// New position
            /// </summary>
            public const string NewRating = "new_position";
        }

        /// <summary>
        /// Key names for <see cref="ForecastImpact"/>
        /// </summary>
        public static class Keys
        {
            /// <summary>
            /// Primary key
            /// </summary>
            public const string PrimaryKey = $"{TableName}_pkey";

            /// <summary>
            /// <see cref="ForecastImpact"/> references <see cref="PlayerCardForecast"/>
            /// </summary>
            public const string PlayerCardForecastsForeignKeyConstraint =
                $"{TableName}_{PlayerCardForecasts.TableName}_{PlayerCardForecasts.Id}_fkey";
        }
    }
}