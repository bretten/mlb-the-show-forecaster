﻿using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFrameworkCore;

/// <summary>
/// Constants for EF in the Performance subdomain
/// </summary>
public static class Constants
{
    /// <summary>
    /// The schema name
    /// </summary>
    public const string Schema = "performance";

    /// <summary>
    /// Table and column names for <see cref="PlayerStatsBySeason"/>
    /// </summary>
    public static class PlayerStatsBySeasons
    {
        /// <summary>
        /// Table name
        /// </summary>
        public const string TableName = "player_stats_by_seasons";

        /// <summary>
        /// Primary key column name
        /// </summary>
        public const string Id = "id";

        /// <summary>
        /// Player MLB ID column name
        /// </summary>
        public const string PlayerMlbId = "player_mlb_id";

        /// <summary>
        /// The season
        /// </summary>
        public const string Season = "season";

        /// <summary>
        /// Batting score
        /// </summary>
        public const string BattingScore = "batting_score";

        /// <summary>
        /// Pitching score
        /// </summary>
        public const string PitchingScore = "pitching_score";

        /// <summary>
        /// Fielding score
        /// </summary>
        public const string FieldingScore = "fielding_score";

        /// <summary>
        /// Key names for <see cref="PlayerStatsBySeason"/>
        /// </summary>
        public static class Keys
        {
            /// <summary>
            /// Primary key
            /// </summary>
            public const string PrimaryKey = $"{TableName}_pkey";
        }

        /// <summary>
        /// Index names for <see cref="PlayerStatsBySeason"/>
        /// </summary>
        public static class Indexes
        {
            /// <summary>
            /// For querying by season year
            /// </summary>
            public const string Year = $"{TableName}_{Season}_idx";
        }
    }

    /// <summary>
    /// Table and column names for <see cref="PlayerBattingStatsByGame"/>
    /// </summary>
    public static class PlayerBattingStatsByGames
    {
        /// <summary>
        /// Table name
        /// </summary>
        public const string TableName = "player_batting_stats_by_games";

        /// <summary>
        /// Foreign key column name that references <see cref="PlayerStatsBySeason"/>
        /// </summary>
        public const string PlayerStatsBySeasonId = "player_stats_by_season_id";

        /// <summary>
        /// Player MLB ID column name
        /// </summary>
        public const string PlayerMlbId = PlayerStatsBySeasons.PlayerMlbId;

        /// <summary>
        /// The season
        /// </summary>
        public const string Season = PlayerStatsBySeasons.Season;

        /// <summary>
        /// The game date
        /// </summary>
        public const string GameDate = "date";

        /// <summary>
        /// The game MLB ID
        /// </summary>
        public const string GameMlbId = "game_mlb_id";

        /// <summary>
        /// The team MLB ID
        /// </summary>
        public const string TeamMlbId = "team_mlb_id";

        /// <summary>
        /// The number of plate appearances
        /// </summary>
        public const string PlateAppearances = "plate_appearances";

        /// <summary>
        /// The number of at bats
        /// </summary>
        public const string AtBats = "at_bats";

        /// <summary>
        /// The number of runs scored
        /// </summary>
        public const string Runs = "runs";

        /// <summary>
        /// The number of hits
        /// </summary>
        public const string Hits = "hits";

        /// <summary>
        /// The number of doubles
        /// </summary>
        public const string Doubles = "doubles";

        /// <summary>
        /// The number of triples
        /// </summary>
        public const string Triples = "triples";

        /// <summary>
        /// The number of home runs
        /// </summary>
        public const string HomeRuns = "home_runs";

        /// <summary>
        /// The number of runs batted in
        /// </summary>
        public const string RunsBattedIn = "runs_batted_in";

        /// <summary>
        /// The number of walks
        /// </summary>
        public const string BaseOnBalls = "base_on_balls";

        /// <summary>
        /// The number of intentional walks
        /// </summary>
        public const string IntentionalWalks = "intentional_walks";

        /// <summary>
        /// The number of strikeouts
        /// </summary>
        public const string Strikeouts = "strikeouts";

        /// <summary>
        /// The number of stolen bases
        /// </summary>
        public const string StolenBases = "stolen_bases";

        /// <summary>
        /// The number of times caught stealing
        /// </summary>
        public const string CaughtStealing = "caught_stealing";

        /// <summary>
        /// The number of times the player was hit by a pitch
        /// </summary>
        public const string HitByPitches = "hit_by_pitches";

        /// <summary>
        /// The number of sacrifice bunts
        /// </summary>
        public const string SacrificeBunts = "sacrifice_bunts";

        /// <summary>
        /// The number of sacrifice flies
        /// </summary>
        public const string SacrificeFlies = "sacrifice_flies";

        /// <summary>
        /// The number of pitches the player saw as a batter
        /// </summary>
        public const string NumberOfPitchesSeen = "number_of_pitches_seen";

        /// <summary>
        /// The number of runners the player did not advance when batting and their out results in the end of the inning
        /// </summary>
        public const string LeftOnBase = "left_on_base";

        /// <summary>
        /// The number of times the batter grounded out
        /// </summary>
        public const string GroundOuts = "ground_outs";

        /// <summary>
        /// The number of times the batter grounded into a double play
        /// </summary>
        public const string GroundIntoDoublePlays = "ground_into_double_plays";

        /// <summary>
        /// The number of times the batter grounded into a triple play
        /// </summary>
        public const string GroundIntoTriplePlays = "ground_into_triple_plays";

        /// <summary>
        /// The number of times the batter hit a fly ball that led to an out
        /// </summary>
        public const string AirOuts = "air_outs";

        /// <summary>
        /// The number of times a catcher interfered with the batter's plate appearance
        /// </summary>
        public const string CatcherInterferences = "catcher_interferences";

        /// <summary>
        /// Key names for <see cref="PlayerBattingStatsByGame"/>
        /// </summary>
        public static class Keys
        {
            /// <summary>
            /// Primary key
            /// </summary>
            public const string PrimaryKey = $"{TableName}_pkey";

            /// <summary>
            /// <see cref="PlayerBattingStatsByGame"/> references <see cref="PlayerStatsBySeason"/>
            /// </summary>
            public const string PlayerStatsBySeasonsForeignKeyConstraint =
                $"{TableName}_{PlayerStatsBySeasons.TableName}_{PlayerStatsBySeasons.Id}_fkey";
        }
    }

    /// <summary>
    /// <see cref="PlayerPitchingStatsByGame"/>
    /// </summary>
    public static class PlayerPitchingStatsByGames
    {
        /// <summary>
        /// Table name
        /// </summary>
        public const string TableName = "player_pitching_stats_by_games";

        /// <summary>
        /// Foreign key column name that references <see cref="PlayerStatsBySeason"/>
        /// </summary>
        public const string PlayerStatsBySeasonId = "player_stats_by_season_id";

        /// <summary>
        /// Player MLB ID column name
        /// </summary>
        public const string PlayerMlbId = PlayerStatsBySeasons.PlayerMlbId;

        /// <summary>
        /// The season
        /// </summary>
        public const string Season = PlayerStatsBySeasons.Season;

        /// <summary>
        /// The game date
        /// </summary>
        public const string GameDate = "date";

        /// <summary>
        /// The game MLB ID
        /// </summary>
        public const string GameMlbId = "game_mlb_id";

        /// <summary>
        /// The team MLB ID
        /// </summary>
        public const string TeamMlbId = "team_mlb_id";

        /// <summary>
        /// The number of wins
        /// </summary>
        public const string Wins = "wins";

        /// <summary>
        /// The number of losses
        /// </summary>
        public const string Losses = "losses";

        /// <summary>
        /// The number of games started as a pitcher
        /// </summary>
        public const string GamesStarted = "games_started";

        /// <summary>
        /// The number of times the pitcher was the last pitcher in the game as a relief pitcher
        /// </summary>
        public const string GamesFinished = "games_finished";

        /// <summary>
        /// The number of complete games pitched
        /// </summary>
        public const string CompleteGames = "complete_games";

        /// <summary>
        /// The number of shutouts
        /// </summary>
        public const string Shutouts = "shutouts";

        /// <summary>
        /// The number of holds
        /// </summary>
        public const string Holds = "holds";

        /// <summary>
        /// The number of saves
        /// </summary>
        public const string Saves = "saves";

        /// <summary>
        /// The number of blown saves
        /// </summary>
        public const string BlownSaves = "blown_saves";

        /// <summary>
        /// The number of save opportunities
        /// </summary>
        public const string SaveOpportunities = "save_opportunities";

        /// <summary>
        /// The number of innings pitched
        /// </summary>
        public const string InningsPitched = "innings_pitched";

        /// <summary>
        /// The number of hits given up
        /// </summary>
        public const string Hits = "hits";

        /// <summary>
        /// The number of doubles given up
        /// </summary>
        public const string Doubles = "doubles";

        /// <summary>
        /// The number of triples given up
        /// </summary>
        public const string Triples = "triples";

        /// <summary>
        /// The number of home runs given up
        /// </summary>
        public const string HomeRuns = "home_runs";

        /// <summary>
        /// The number of runs given up
        /// </summary>
        public const string Runs = "runs";

        /// <summary>
        /// The number of earned runs given up (runs that were a result of this pitcher giving up a hit)
        /// </summary>
        public const string EarnedRuns = "earned_runs";

        /// <summary>
        /// The number of strikeouts
        /// </summary>
        public const string Strikeouts = "strikeouts";

        /// <summary>
        /// The number of times the pitcher walked the batter
        /// </summary>
        public const string BaseOnBalls = "base_on_balls";

        /// <summary>
        /// The number of times the pitcher intentionally walked the batter
        /// </summary>
        public const string IntentionalWalks = "intentional_walks";

        /// <summary>
        /// The number of times the pitcher hit a batter with a pitch
        /// </summary>
        public const string HitBatsmen = "hit_batsmen";

        /// <summary>
        /// The number of outs made by the team while this pitcher was active
        /// </summary>
        public const string Outs = "outs";

        /// <summary>
        /// The number of times a pitch resulted in a ground out
        /// </summary>
        public const string GroundOuts = "ground_outs";

        /// <summary>
        /// The number of times a pitch resulted in a air/fly out
        /// </summary>
        public const string AirOuts = "air_outs";

        /// <summary>
        /// The number of double play ground outs induced
        /// </summary>
        public const string GroundIntoDoublePlays = "ground_into_double_plays";

        /// <summary>
        /// The number of pitches thrown this game
        /// </summary>
        public const string NumberOfPitches = "number_of_pitches";

        /// <summary>
        /// The number of strikes thrown by the pitcher
        /// </summary>
        public const string Strikes = "strikes";

        /// <summary>
        /// The number of wild pitches thrown
        /// </summary>
        public const string WildPitches = "wild_pitches";

        /// <summary>
        /// The number of balks
        /// </summary>
        public const string Balks = "balks";

        /// <summary>
        /// The number of batters faced, pitcher version of plate appearance
        /// </summary>
        public const string BattersFaced = "batters_faced";

        /// <summary>
        /// The number of at-bats
        /// </summary>
        public const string AtBats = "at_bats";

        /// <summary>
        /// The number of bases stolen against this pitcher
        /// </summary>
        public const string StolenBases = "stolen_bases";

        /// <summary>
        /// The number of times a runner was caught stealing against this pitcher
        /// </summary>
        public const string CaughtStealing = "caught_stealing";

        /// <summary>
        /// The number of pick offs made by this pitcher
        /// </summary>
        public const string Pickoffs = "pickoffs";

        /// <summary>
        /// The number of runners on base when the pitcher enters the game
        /// </summary>
        public const string InheritedRunners = "inherited_runners";

        /// <summary>
        /// The number of inherited runners allowed to score
        /// </summary>
        public const string InheritedRunnersScored = "inherited_runners_scored";

        /// <summary>
        /// The number of times a catcher interfered with the batter's plate appearance
        /// </summary>
        public const string CatcherInterferences = "catcher_interferences";

        /// <summary>
        /// The number of sacrifice bunts made against the pitcher
        /// </summary>
        public const string SacrificeBunts = "sacrifice_bunts";

        /// <summary>
        /// The number of sacrifice flies made against the pitcher
        /// </summary>
        public const string SacrificeFlies = "sacrifice_flies";

        /// <summary>
        /// Key names for <see cref="PlayerPitchingStatsByGame"/>
        /// </summary>
        public static class Keys
        {
            /// <summary>
            /// Primary key
            /// </summary>
            public const string PrimaryKey = $"{TableName}_pkey";

            /// <summary>
            /// <see cref="PlayerPitchingStatsByGame"/> references <see cref="PlayerStatsBySeason"/>
            /// </summary>
            public const string PlayerStatsBySeasonsForeignKeyConstraint =
                $"{TableName}_{PlayerStatsBySeasons.TableName}_{PlayerStatsBySeasons.Id}_fkey";
        }
    }

    /// <summary>
    /// Table and column names for <see cref="PlayerFieldingStatsByGame"/>
    /// </summary>
    public static class PlayerFieldingStatsByGames
    {
        /// <summary>
        /// Table name
        /// </summary>
        public const string TableName = "player_fielding_stats_by_games";

        /// <summary>
        /// Foreign key column name that references <see cref="PlayerStatsBySeason"/>
        /// </summary>
        public const string PlayerStatsBySeasonId = "player_stats_by_season_id";

        /// <summary>
        /// Player MLB ID column name
        /// </summary>
        public const string PlayerMlbId = PlayerStatsBySeasons.PlayerMlbId;

        /// <summary>
        /// The season
        /// </summary>
        public const string Season = PlayerStatsBySeasons.Season;

        /// <summary>
        /// The game date
        /// </summary>
        public const string GameDate = "date";

        /// <summary>
        /// The game MLB ID
        /// </summary>
        public const string GameMlbId = "game_mlb_id";

        /// <summary>
        /// The team MLB ID
        /// </summary>
        public const string TeamMlbId = "team_mlb_id";

        /// <summary>
        /// The position the player is fielding
        /// </summary>
        public const string Position = "position";

        /// <summary>
        /// The number of times the player started the game at this <see cref="Position"/>
        /// </summary>
        public const string GamesStarted = "games_started";

        /// <summary>
        /// The number of innings this player fielded at this <see cref="Position"/>
        /// </summary>
        public const string InningsPlayed = "innings_played";

        /// <summary>
        /// The number of outs on a play where the fielder touched the ball excluding when this player does the actual putout
        /// </summary>
        public const string Assists = "assists";

        /// <summary>
        /// The number of times the fielder tags, forces, or appeals a runner and they are called out
        /// </summary>
        public const string Putouts = "putouts";

        /// <summary>
        /// The number of times a fielder fails to make a play that is considered to be doable with common effort
        /// </summary>
        public const string Errors = "errors";

        /// <summary>
        /// The number of errors that were the result of a bad throw
        /// </summary>
        public const string ThrowingErrors = "throwing_errors";

        /// <summary>
        /// The number of double plays where the fielder recorded a putout or an assist
        /// </summary>
        public const string DoublePlays = "double_plays";

        /// <summary>
        /// The number of triple plays where the fielder recorded a putout or an assist
        /// </summary>
        public const string TriplePlays = "triple_plays";

        /// <summary>
        /// Catcher stat: The number of times the catcher was able to throw out a base runner attempting to steal
        /// </summary>
        public const string CaughtStealing = "caught_stealing";

        /// <summary>
        /// Catcher stat: The number of times a base runner successfully stole a base against the catcher
        /// </summary>
        public const string StolenBases = "stolen_bases";

        /// <summary>
        /// Catcher stat: The number of times the catcher dropped the ball and a runner was able to advance
        /// </summary>
        public const string PassedBalls = "passed_balls";

        /// <summary>
        /// Catcher stat: The number of times a catcher interfered with the batter's plate appearance
        /// </summary>
        public const string CatcherInterferences = "catcher_interferences";

        /// <summary>
        /// Catcher stat: The number of wild pitches the catcher saw from the pitcher
        /// </summary>
        public const string WildPitches = "wild_pitches";

        /// <summary>
        /// Catcher stat: The number of pick offs made by the pitcher while this catcher was behind the plate
        /// </summary>
        public const string Pickoffs = "pickoffs";

        /// <summary>
        /// Key names for <see cref="PlayerFieldingStatsByGame"/>
        /// </summary>
        public static class Keys
        {
            /// <summary>
            /// Primary key
            /// </summary>
            public const string PrimaryKey = $"{TableName}_pkey";

            /// <summary>
            /// <see cref="PlayerFieldingStatsByGame"/> references <see cref="PlayerStatsBySeason"/>
            /// </summary>
            public const string PlayerStatsBySeasonsForeignKeyConstraint =
                $"{TableName}_{PlayerStatsBySeasons.TableName}_{PlayerStatsBySeasons.Id}_fkey";
        }
    }
}