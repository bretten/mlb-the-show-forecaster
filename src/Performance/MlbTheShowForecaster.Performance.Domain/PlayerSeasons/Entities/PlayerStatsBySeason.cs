using System.Collections.Immutable;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Participation;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Pitching;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.Comparisons;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;

/// <summary>
/// Represents a Player's batting, fielding and pitching statistics by individual game for a whole season
/// </summary>
public sealed class PlayerStatsBySeason : AggregateRoot
{
    /// <summary>
    /// The player's batting stats by game
    /// </summary>
    private readonly List<PlayerBattingStatsByGame> _battingStatsByGames;

    /// <summary>
    /// The player's pitching stats by game
    /// </summary>
    private readonly List<PlayerPitchingStatsByGame> _pitchingStatsByGames;

    /// <summary>
    /// The player's fielding stats by game
    /// </summary>
    private readonly List<PlayerFieldingStatsByGame> _fieldingStatsByGames;

    /// <summary>
    /// The MLB ID of the Player
    /// </summary>
    public MlbId PlayerMlbId { get; }

    /// <summary>
    /// The season
    /// </summary>
    public SeasonYear SeasonYear { get; }

    /// <summary>
    /// The player's batting stats by game in chronological order
    /// </summary>
    public IReadOnlyList<PlayerBattingStatsByGame> BattingStatsByGamesChronologically =>
        _battingStatsByGames.OrderBy(x => x.GameDate).ToImmutableList();

    /// <summary>
    /// The player's pitching stats by game in chronological order
    /// </summary>
    public IReadOnlyList<PlayerPitchingStatsByGame> PitchingStatsByGamesChronologically =>
        _pitchingStatsByGames.OrderBy(x => x.GameDate).ToImmutableList();

    /// <summary>
    /// The player's fielding stats by game in chronological order
    /// </summary>
    public IReadOnlyList<PlayerFieldingStatsByGame> FieldingStatsByGamesChronologically =>
        _fieldingStatsByGames.OrderBy(x => x.GameDate).ToImmutableList();

    /// <summary>
    /// Batting stats for the whole season to date
    /// </summary>
    public BattingStats SeasonBattingStats => BattingStats.Create(_battingStatsByGames);

    /// <summary>
    /// Pitching stats for the whole season to date
    /// </summary>
    public PitchingStats SeasonPitchingStats => PitchingStats.Create(_pitchingStatsByGames);

    /// <summary>
    /// Fielding stats for all positions for the whole season to date
    /// </summary>
    public FieldingStats SeasonAggregateFieldingStats => FieldingStats.Create(_fieldingStatsByGames);

    /// <summary>
    /// Fielding stats by position for the whole season to date
    /// </summary>
    public Dictionary<Position, FieldingStats> SeasonFieldingStatsByPosition => _fieldingStatsByGames
        .GroupBy(x => x.Position)
        .ToDictionary(x => x.Key, FieldingStats.Create);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the Player</param>
    /// <param name="seasonYear">The season</param>
    /// <param name="battingStatsByGames">The player's batting stats by game</param>
    /// <param name="pitchingStatsByGames">The player's pitching stats by game</param>
    /// <param name="fieldingStatsByGames">The player's fielding stats by game</param>
    private PlayerStatsBySeason(MlbId playerMlbId, SeasonYear seasonYear,
        List<PlayerBattingStatsByGame> battingStatsByGames, List<PlayerPitchingStatsByGame> pitchingStatsByGames,
        List<PlayerFieldingStatsByGame> fieldingStatsByGames) : base(Guid.NewGuid())
    {
        PlayerMlbId = playerMlbId;
        SeasonYear = seasonYear;
        _battingStatsByGames = battingStatsByGames;
        _pitchingStatsByGames = pitchingStatsByGames;
        _fieldingStatsByGames = fieldingStatsByGames;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the Player</param>
    /// <param name="seasonYear">The season</param>
    private PlayerStatsBySeason(MlbId playerMlbId, SeasonYear seasonYear) : base(Guid.NewGuid())
    {
        PlayerMlbId = playerMlbId;
        SeasonYear = seasonYear;
        _battingStatsByGames = new List<PlayerBattingStatsByGame>();
        _pitchingStatsByGames = new List<PlayerPitchingStatsByGame>();
        _fieldingStatsByGames = new List<PlayerFieldingStatsByGame>();
    }

    /// <summary>
    /// Logs a game where the player participated in batting
    /// </summary>
    /// <param name="stats">Batting stats for the game participated in</param>
    public void LogBattingGame(PlayerBattingStatsByGame stats)
    {
        _battingStatsByGames.Add(stats);
        RaiseDomainEvent(new PlayerBattedInGameEvent(PlayerMlbId, stats.GameDate));
    }

    /// <summary>
    /// Logs a game where the player participated in pitching
    /// </summary>
    /// <param name="stats">Pitching stats for the game participated in</param>
    public void LogPitchingGame(PlayerPitchingStatsByGame stats)
    {
        _pitchingStatsByGames.Add(stats);
        RaiseDomainEvent(new PlayerPitchedInGameEvent(PlayerMlbId, stats.GameDate));
    }

    /// <summary>
    /// Logs a game where the player participated in fielding
    /// </summary>
    /// <param name="stats">Fielding stats for the game participated in</param>
    public void LogFieldingGame(PlayerFieldingStatsByGame stats)
    {
        _fieldingStatsByGames.Add(stats);
        RaiseDomainEvent(new PlayerFieldedInGameEvent(PlayerMlbId, stats.GameDate));
    }

    /// <summary>
    /// Analyzes the player's season batting stats before and since the specified comparison date and looks for
    /// improvements or declines in performance
    /// </summary>
    /// <param name="comparisonDate">The date of comparison -- batting stats before this date will be compared to stats since this date</param>
    /// <param name="assessmentRequirements">Service that ensures there are enough batting stats this season from before and since the comparison date</param>
    public void AssessBattingPerformance(DateOnly comparisonDate,
        IPerformanceAssessmentRequirements assessmentRequirements)
    {
        // Stats from before the comparison date
        var statsBefore = BattingStatsBeforeDate(comparisonDate);
        // Stats since the comparison date
        var statsSince = BattingStatsSinceDate(comparisonDate);

        // Make sure the player has seen enough batting playtime
        if (!assessmentRequirements.AreBattingAssessmentRequirementsMet(statsBefore.PlateAppearances)
            || !assessmentRequirements.AreBattingAssessmentRequirementsMet(statsSince.PlateAppearances))
        {
            // Player has not had enough playtime to warrant a valid assessment. No domain event needed
            return;
        }

        // Create a comparison of the stats from before and the stats since the comparison date
        var comparison = PlayerBattingPeriodComparison.Create(PlayerMlbId, comparisonDate,
            statsBeforeComparisonDate: statsBefore,
            statsSinceComparisonDate: statsSince
        );

        // If the stats have improved, raise an event. If they have declined, raise an event
        if (comparison.HasIncreasedBy(assessmentRequirements.StatPercentChangeThreshold))
        {
            RaiseDomainEvent(new BattingImprovementEvent(comparison));
        }
        else if (comparison.HasDecreasedBy(assessmentRequirements.StatPercentChangeThreshold))
        {
            RaiseDomainEvent(new BattingDeclineEvent(comparison));
        }
    }

    /// <summary>
    /// Analyzes the player's season pitching stats before and since the specified comparison date and looks for
    /// improvements or declines in performance
    /// </summary>
    /// <param name="comparisonDate">The date of comparison -- pitching stats before this date will be compared to stats since this date</param>
    /// <param name="assessmentRequirements">Service that ensures there are enough pitching stats this season from before and since the comparison date</param>
    public void AssessPitchingPerformance(DateOnly comparisonDate,
        IPerformanceAssessmentRequirements assessmentRequirements)
    {
        // Stats from before the comparison date
        var statsBefore = PitchingStatsBeforeDate(comparisonDate);
        // Stats since the comparison date
        var statsSince = PitchingStatsSinceDate(comparisonDate);

        // Make sure the player has seen enough pitching playtime
        if (!assessmentRequirements.ArePitchingAssessmentRequirementsMet(statsBefore.InningsPitched,
                statsBefore.BattersFaced) ||
            !assessmentRequirements.ArePitchingAssessmentRequirementsMet(statsSince.InningsPitched,
                statsSince.BattersFaced))
        {
            // Player has not had enough playtime to warrant a valid assessment. No domain event needed
            return;
        }

        // Create a comparison of the stats from before and the stats since the comparison date
        var comparison = PlayerPitchingPeriodComparison.Create(PlayerMlbId, comparisonDate,
            statsBeforeComparisonDate: statsBefore,
            statsSinceComparisonDate: statsSince
        );

        // If the stats have improved, raise an event. If they have declined, raise an event
        if (comparison.HasDecreasedBy(assessmentRequirements.StatPercentChangeThreshold)) // Lower ERA is better
        {
            RaiseDomainEvent(new PitchingImprovementEvent(comparison));
        }
        else if (comparison.HasIncreasedBy(assessmentRequirements.StatPercentChangeThreshold))
        {
            RaiseDomainEvent(new PitchingDeclineEvent(comparison));
        }
    }

    /// <summary>
    /// Analyzes the player's season fielding stats before and since the specified comparison date and looks for
    /// improvements or declines in performance
    /// </summary>
    /// <param name="comparisonDate">The date of comparison -- fielding stats before this date will be compared to stats since this date</param>
    /// <param name="assessmentRequirements">Service that ensures there are enough fielding stats this season from before and since the comparison date</param>
    public void AssessFieldingPerformance(DateOnly comparisonDate,
        IPerformanceAssessmentRequirements assessmentRequirements)
    {
        // Stats from before the comparison date
        var statsBefore = FieldingStatsBeforeDate(comparisonDate);
        // Stats since the comparison date
        var statsSince = FieldingStatsSinceDate(comparisonDate);

        // Make sure the player has seen enough fielding playtime
        if (!assessmentRequirements.AreFieldingAssessmentRequirementsMet(statsBefore.TotalChances.ToNaturalNumber())
            || !assessmentRequirements.AreFieldingAssessmentRequirementsMet(statsSince.TotalChances.ToNaturalNumber()))
        {
            return;
        }

        // Create a comparison of the stats from before and the stats since the comparison date
        var comparison = PlayerFieldingPeriodComparison.Create(PlayerMlbId, comparisonDate,
            statsBeforeComparisonDate: statsBefore,
            statsSinceComparisonDate: statsSince
        );

        // If the stats have improved, raise an event. If they have declined, raise an event
        if (comparison.HasIncreasedBy(assessmentRequirements.StatPercentChangeThreshold))
        {
            RaiseDomainEvent(new FieldingImprovementEvent(comparison));
        }
        else if (comparison.HasDecreasedBy(assessmentRequirements.StatPercentChangeThreshold))
        {
            RaiseDomainEvent(new FieldingDeclineEvent(comparison));
        }
    }

    /// <summary>
    /// Returns the player's season batting stats from before the specified date
    /// </summary>
    /// <param name="date">The date used to filter stats</param>
    /// <returns>Player's season batting stats from before the specified date</returns>
    private BattingStats BattingStatsBeforeDate(DateOnly date) =>
        BattingStats.Create(_battingStatsByGames.Where(x => x.GameDate < date));

    /// <summary>
    /// Returns the player's season batting stats since the specified date
    /// </summary>
    /// <param name="date">The date used to filter stats</param>
    /// <returns>Player's season batting stats since the specified date</returns>
    private BattingStats BattingStatsSinceDate(DateOnly date) =>
        BattingStats.Create(_battingStatsByGames.Where(x => x.GameDate >= date));

    /// <summary>
    /// Returns the player's season pitching stats from before the specified date
    /// </summary>
    /// <param name="date">The date used to filter stats</param>
    /// <returns>Player's season pitching stats from before the specified date</returns>
    private PitchingStats PitchingStatsBeforeDate(DateOnly date) =>
        PitchingStats.Create(_pitchingStatsByGames.Where(x => x.GameDate < date));

    /// <summary>
    /// Returns the player's season pitching stats since the specified date
    /// </summary>
    /// <param name="date">The date used to filter stats</param>
    /// <returns>Player's season pitching stats since the specified date</returns>
    private PitchingStats PitchingStatsSinceDate(DateOnly date) =>
        PitchingStats.Create(_pitchingStatsByGames.Where(x => x.GameDate >= date));

    /// <summary>
    /// Returns the player's season fielding stats from before the specified date
    /// </summary>
    /// <param name="date">The date used to filter stats</param>
    /// <returns>Player's season fielding stats from before the specified date</returns>
    private FieldingStats FieldingStatsBeforeDate(DateOnly date) =>
        FieldingStats.Create(_fieldingStatsByGames.Where(x => x.GameDate < date));

    /// <summary>
    /// Returns the player's season fielding stats since the specified date
    /// </summary>
    /// <param name="date">The date used to filter stats</param>
    /// <returns>Player's season fielding stats since the specified date</returns>
    private FieldingStats FieldingStatsSinceDate(DateOnly date) =>
        FieldingStats.Create(_fieldingStatsByGames.Where(x => x.GameDate >= date));

    /// <summary>
    /// Creates <see cref="PlayerStatsBySeason"/>
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the Player</param>
    /// <param name="seasonYear">The season</param>
    /// <param name="battingStatsByGames">The player's batting stats by game</param>
    /// <param name="pitchingStatsByGames">The player's pitching stats by game</param>
    /// <param name="fieldingStatsByGames">The player's fielding stats by game</param>
    /// <returns><see cref="PlayerStatsBySeason"/></returns>
    public static PlayerStatsBySeason Create(MlbId playerMlbId, SeasonYear seasonYear,
        List<PlayerBattingStatsByGame> battingStatsByGames, List<PlayerPitchingStatsByGame> pitchingStatsByGames,
        List<PlayerFieldingStatsByGame> fieldingStatsByGames)
    {
        return new PlayerStatsBySeason(playerMlbId, seasonYear, battingStatsByGames, pitchingStatsByGames,
            fieldingStatsByGames);
    }
}