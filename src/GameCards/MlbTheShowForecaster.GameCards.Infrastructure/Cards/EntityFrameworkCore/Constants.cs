using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Cards.EntityFrameworkCore;

/// <summary>
/// Constants for EF in the GameCards subdomain
/// </summary>
public static class Constants
{
    /// <summary>
    /// The schema name
    /// </summary>
    public const string Schema = "game_cards";

    /// <summary>
    /// Table and column names for <see cref="Card"/>
    /// </summary>
    public static class Cards
    {
        /// <summary>
        /// Primary key column name
        /// </summary>
        public const string Id = "id";

        /// <summary>
        /// The year of MLB The Show
        /// </summary>
        public const string Year = "year";

        /// <summary>
        /// The card ID from MLB The Show
        /// </summary>
        public const string ExternalId = "external_id";

        /// <summary>
        /// The card type
        /// </summary>
        public const string Type = "type";

        /// <summary>
        /// The card image location
        /// </summary>
        public const string ImageLocation = "image_location";

        /// <summary>
        /// The name of the card
        /// </summary>
        public const string Name = "name";

        /// <summary>
        /// The rarity of the card
        /// </summary>
        public const string Rarity = "rarity";

        /// <summary>
        /// The series of the card
        /// </summary>
        public const string Series = "series";
    }

    /// <summary>
    /// Table and column names for <see cref="PlayerCard"/>
    /// </summary>
    public static class PlayerCards
    {
        /// <summary>
        /// Table name
        /// </summary>
        public const string TableName = "player_cards";

        /// <summary>
        /// Primary key
        /// </summary>
        public const string PrimaryKeyName = $"{TableName}_pkey";

        /// <summary>
        /// The player card's primary position
        /// </summary>
        public const string Position = "position";

        /// <summary>
        /// The player's team name abbreviated
        /// </summary>
        public const string TeamShortName = "team_short_name";

        /// <summary>
        /// The overall rating of the card
        /// </summary>
        public const string OverallRating = "overall_rating";

        /// <summary>
        /// Pitcher's stamina
        /// </summary>
        public const string Stamina = "stamina";

        /// <summary>
        /// Pitcher's ability to pitch with runners in scoring position
        /// </summary>
        public const string PitchingClutch = "pitching_clutch";

        /// <summary>
        /// Pitcher's ability to prevent hits
        /// </summary>
        public const string HitsPerNine = "hits_per_nine";

        /// <summary>
        /// Pitcher's ability to cause a batter to swing and miss
        /// </summary>
        public const string StrikeoutsPerNine = "strikeouts_per_nine";

        /// <summary>
        /// Pitcher's ability to prevent walks
        /// </summary>
        public const string BaseOnBallsPerNine = "base_on_balls_per_nine";

        /// <summary>
        /// Pitcher's ability to prevent home runs
        /// </summary>
        public const string HomeRunsPerNine = "home_runs_per_nine";

        /// <summary>
        /// The pitcher's velocity
        /// </summary>
        public const string PitchVelocity = "pitch_velocity";

        /// <summary>
        /// The pitcher's control
        /// </summary>
        public const string PitchControl = "pitch_control";

        /// <summary>
        /// The pitcher's ability to throw breaking pitches
        /// </summary>
        public const string PitchMovement = "pitch_movement";

        /// <summary>
        /// The batter's ability to make contact with left handed pitchers
        /// </summary>
        public const string ContactLeft = "contact_left";

        /// <summary>
        /// The batter's ability to make contact with right handed pitchers
        /// </summary>
        public const string ContactRight = "contact_right";

        /// <summary>
        /// The batter's power against left handed pitchers
        /// </summary>
        public const string PowerLeft = "power_left";

        /// <summary>
        /// The batter's power against right handed pitchers
        /// </summary>
        public const string PowerRight = "power_right";

        /// <summary>
        /// The batter's ability to see the ball and prevent strikeouts
        /// </summary>
        public const string PlateVision = "plate_vision";

        /// <summary>
        /// The batter's ability to check a swing
        /// </summary>
        public const string PlateDiscipline = "plate_discipline";

        /// <summary>
        /// The batter's ability to hit with runners in scoring position
        /// </summary>
        public const string BattingClutch = "batting_clutch";

        /// <summary>
        /// The batter's bunting ability
        /// </summary>
        public const string BuntingAbility = "bunting_ability";

        /// <summary>
        /// The batter's drag bunting ability
        /// </summary>
        public const string DragBuntingAbility = "drag_bunting_ability";

        /// <summary>
        /// The ability of a player to prevent injury when batting
        /// </summary>
        public const string HittingDurability = "hitting_durability";

        /// <summary>
        /// The ability of a player to prevent injury when fielding
        /// </summary>
        public const string FieldingDurability = "fielding_durability";

        /// <summary>
        /// The player's fielding ability
        /// </summary>
        public const string FieldingAbility = "fielding_ability";

        /// <summary>
        /// The player's ability to throw the ball with velocity and distance
        /// </summary>
        public const string ArmStrength = "arm_strength";

        /// <summary>
        /// The player's ability to throw the ball accurately
        /// </summary>
        public const string ArmAccuracy = "arm_accuracy";

        /// <summary>
        /// The ability of a fielder to react when a batter makes contact with the ball
        /// </summary>
        public const string ReactionTime = "reaction_time";

        /// <summary>
        /// The ability of a catcher to block wild pitches
        /// </summary>
        public const string Blocking = "blocking";

        /// <summary>
        /// The speed of the player
        /// </summary>
        public const string Speed = "speed";

        /// <summary>
        /// How well the player can run around the bases
        /// </summary>
        public const string BaseRunningAbility = "base_running_ability";

        /// <summary>
        /// How likely it is the player can steal a base
        /// </summary>
        public const string BaseRunningAggression = "base_running_aggression";

        /// <summary>
        /// Indexes for <see cref="PlayerCard"/>
        /// </summary>
        public static class Indexes
        {
            /// <summary>
            /// For querying by game year and then the card's external ID
            /// </summary>
            public const string YearAndExternalId = $"{TableName}_{Cards.Year}_{Cards.ExternalId}_idx";
        }
    }

    /// <summary>
    /// Table and column names for <see cref="PlayerCardHistoricalRating"/>
    /// </summary>
    public static class PlayerCardHistoricalRatings
    {
        /// <summary>
        /// Table name
        /// </summary>
        public const string TableName = "player_card_historical_ratings";

        /// <summary>
        /// Primary key
        /// </summary>
        public const string PrimaryKeyName = $"{TableName}_pkey";

        /// <summary>
        /// Foreign key that references <see cref="PlayerCard"/>
        /// </summary>
        public const string PlayerCardId = "player_card_id";

        /// <summary>
        /// The first date the player card had this rating
        /// </summary>
        public const string StartDate = "start_date";

        /// <summary>
        /// The last date the player card had this rating
        /// </summary>
        public const string EndDate = "end_date";

        /// <summary>
        /// Foreign keys
        /// </summary>
        public static class ForeignKeys
        {
            /// <summary>
            /// <see cref="PlayerCardHistoricalRatings"/> references <see cref="PlayerCard"/>
            /// </summary>
            public const string PlayerCardsConstraint = $"{TableName}_{PlayerCards.TableName}_{Cards.Id}_fkey";
        }
    }
}