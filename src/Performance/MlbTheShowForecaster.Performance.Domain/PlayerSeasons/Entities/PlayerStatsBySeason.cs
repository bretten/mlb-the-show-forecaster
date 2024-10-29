using System.Collections.Immutable;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Participation;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Pitching;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects;
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
    /// A <see cref="PerformanceScore"/> for the player's season batting stats
    /// </summary>
    public PerformanceScore BattingScore { get; private set; }

    /// <summary>
    /// A <see cref="PerformanceScore"/> for the player's season pitching stats
    /// </summary>
    public PerformanceScore PitchingScore { get; private set; }

    /// <summary>
    /// A <see cref="PerformanceScore"/> for the player's season fielding stats
    /// </summary>
    public PerformanceScore FieldingScore { get; private set; }

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
    public FieldingStats SeasonFieldingStats => FieldingStats.Create(_fieldingStatsByGames);

    /// <summary>
    /// Fielding stats by position for the whole season to date
    /// </summary>
    public ImmutableDictionary<Position, FieldingStats> SeasonFieldingStatsByPosition => _fieldingStatsByGames
        .GroupBy(x => x.Position)
        .ToImmutableDictionary(x => x.Key, FieldingStats.Create);

    /// <summary>
    /// Batting stats for an inclusive date range
    /// </summary>
    /// <param name="start">The start date, inclusive</param>
    /// <param name="end">The end date, inclusive</param>
    /// <returns>Aggregated batting stats for the date range</returns>
    public BattingStats BattingStatsFor(DateOnly start, DateOnly end) =>
        BattingStats.Create(_battingStatsByGames.Where(x => x.GameDate >= start && x.GameDate <= end));

    /// <summary>
    /// Pitching stats for an inclusive date range
    /// </summary>
    /// <param name="start">The start date, inclusive</param>
    /// <param name="end">The end date, inclusive</param>
    /// <returns>Aggregated pitching stats for the date range</returns>
    public PitchingStats PitchingStatsFor(DateOnly start, DateOnly end) =>
        PitchingStats.Create(_pitchingStatsByGames.Where(x => x.GameDate >= start && x.GameDate <= end));

    /// <summary>
    /// Fielding stats for an inclusive date range
    /// </summary>
    /// <param name="start">The start date, inclusive</param>
    /// <param name="end">The end date, inclusive</param>
    /// <returns>Aggregated fielding stats for the date range</returns>
    public FieldingStats FieldingStatsFor(DateOnly start, DateOnly end) =>
        FieldingStats.Create(_fieldingStatsByGames.Where(x => x.GameDate >= start && x.GameDate <= end));

    /// <summary>
    /// Fielding stats by position for an inclusive date range
    /// </summary>
    /// <param name="start">The start date, inclusive</param>
    /// <param name="end">The end date, inclusive</param>
    /// <returns>Aggregated fielding stats by position for the date range</returns>
    public ImmutableDictionary<Position, FieldingStats> FieldingStatsByPositionFor(DateOnly start, DateOnly end) =>
        _fieldingStatsByGames
            .Where(x => x.GameDate >= start && x.GameDate <= end)
            .GroupBy(x => x.Position)
            .ToImmutableDictionary(x => x.Key, FieldingStats.Create);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the Player</param>
    /// <param name="seasonYear">The season</param>
    /// <param name="battingScore">A <see cref="PerformanceScore"/> for the player's season batting stats</param>
    /// <param name="pitchingScore">A <see cref="PerformanceScore"/> for the player's season pitching stats</param>
    /// <param name="fieldingScore">A <see cref="PerformanceScore"/> for the player's season fielding stats</param>
    /// <param name="battingStatsByGames">The player's batting stats by game</param>
    /// <param name="pitchingStatsByGames">The player's pitching stats by game</param>
    /// <param name="fieldingStatsByGames">The player's fielding stats by game</param>
    private PlayerStatsBySeason(MlbId playerMlbId, SeasonYear seasonYear,
        PerformanceScore battingScore, PerformanceScore pitchingScore, PerformanceScore fieldingScore,
        List<PlayerBattingStatsByGame> battingStatsByGames, List<PlayerPitchingStatsByGame> pitchingStatsByGames,
        List<PlayerFieldingStatsByGame> fieldingStatsByGames) : base(Guid.NewGuid())
    {
        PlayerMlbId = playerMlbId;
        SeasonYear = seasonYear;
        BattingScore = battingScore;
        PitchingScore = pitchingScore;
        FieldingScore = fieldingScore;
        _battingStatsByGames = battingStatsByGames;
        _pitchingStatsByGames = pitchingStatsByGames;
        _fieldingStatsByGames = fieldingStatsByGames;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the Player</param>
    /// <param name="seasonYear">The season</param>
    /// <param name="battingScore">A <see cref="PerformanceScore"/> for the player's season batting stats</param>
    /// <param name="pitchingScore">A <see cref="PerformanceScore"/> for the player's season pitching stats</param>
    /// <param name="fieldingScore">A <see cref="PerformanceScore"/> for the player's season fielding stats</param>
    private PlayerStatsBySeason(MlbId playerMlbId, SeasonYear seasonYear, PerformanceScore battingScore,
        PerformanceScore pitchingScore, PerformanceScore fieldingScore) : base(Guid.NewGuid())
    {
        PlayerMlbId = playerMlbId;
        SeasonYear = seasonYear;
        BattingScore = battingScore;
        PitchingScore = pitchingScore;
        FieldingScore = fieldingScore;
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
    /// Analyzes the player's season batting stats and calculates a score based off of their performance
    /// </summary>
    /// <param name="performanceAssessor">Service that assesses the player's batting performance</param>
    public void AssessBattingPerformance(IPerformanceAssessor performanceAssessor)
    {
        // Assess the player's to-date performance
        var newScore = performanceAssessor.AssessBatting(SeasonBattingStats);

        // Compare the performance of the first half of games with the second half
        var numberOfGames = BattingStatsByGamesChronologically.Count;
        var firstHalf = BattingStats.Create(BattingStatsByGamesChronologically.Take(numberOfGames / 2));
        var secondHalf = BattingStats.Create(BattingStatsByGamesChronologically.Skip(numberOfGames / 2));
        var firstHalfScore = performanceAssessor.AssessBatting(firstHalf);
        var secondHalfScore = performanceAssessor.AssessBatting(secondHalf);
        var comparison = performanceAssessor.Compare(firstHalfScore, secondHalfScore);

        // If the stats have improved, raise an event. If they have declined, raise an event
        if (comparison.IsSignificantIncrease)
        {
            RaiseDomainEvent(new BattingImprovementEvent(SeasonYear, PlayerMlbId, comparison,
                BattingStatsByGamesChronologically[^1].GameDate));
        }
        else if (comparison.IsSignificantDecrease)
        {
            RaiseDomainEvent(new BattingDeclineEvent(SeasonYear, PlayerMlbId, comparison,
                BattingStatsByGamesChronologically[^1].GameDate));
        }

        // Set the new score
        BattingScore = newScore;
    }

    /// <summary>
    /// Analyzes the player's season pitching stats and calculates a score based off of their performance
    /// </summary>
    /// <param name="performanceAssessor">Service that assesses the player's pitching performance</param>
    public void AssessPitchingPerformance(IPerformanceAssessor performanceAssessor)
    {
        // Assess and compare the player's to-date performance
        var newScore = performanceAssessor.AssessPitching(SeasonPitchingStats);

        // Compare the performance of the first half of games with the second half
        var numberOfGames = PitchingStatsByGamesChronologically.Count;
        var firstHalf = PitchingStats.Create(PitchingStatsByGamesChronologically.Take(numberOfGames / 2));
        var secondHalf = PitchingStats.Create(PitchingStatsByGamesChronologically.Skip(numberOfGames / 2));
        var firstHalfScore = performanceAssessor.AssessPitching(firstHalf);
        var secondHalfScore = performanceAssessor.AssessPitching(secondHalf);
        var comparison = performanceAssessor.Compare(firstHalfScore, secondHalfScore);

        // If the stats have improved, raise an event. If they have declined, raise an event
        if (comparison.IsSignificantIncrease)
        {
            RaiseDomainEvent(new PitchingImprovementEvent(SeasonYear, PlayerMlbId, comparison,
                PitchingStatsByGamesChronologically[^1].GameDate));
        }
        else if (comparison.IsSignificantDecrease)
        {
            RaiseDomainEvent(new PitchingDeclineEvent(SeasonYear, PlayerMlbId, comparison,
                PitchingStatsByGamesChronologically[^1].GameDate));
        }

        // Set the new score
        PitchingScore = newScore;
    }

    /// <summary>
    /// Analyzes the player's season fielding stats and calculates a score based off of their performance
    /// </summary>
    /// <param name="performanceAssessor">Service that assesses the player's fielding performance</param>
    public void AssessFieldingPerformance(IPerformanceAssessor performanceAssessor)
    {
        // Assess and compare the player's to-date performance
        var newScore = performanceAssessor.AssessFielding(SeasonFieldingStats);

        // Compare the performance of the first half of games with the second half
        var numberOfGames = FieldingStatsByGamesChronologically.Count;
        var firstHalf = FieldingStats.Create(FieldingStatsByGamesChronologically.Take(numberOfGames / 2));
        var secondHalf = FieldingStats.Create(FieldingStatsByGamesChronologically.Skip(numberOfGames / 2));
        var firstHalfScore = performanceAssessor.AssessFielding(firstHalf);
        var secondHalfScore = performanceAssessor.AssessFielding(secondHalf);
        var comparison = performanceAssessor.Compare(firstHalfScore, secondHalfScore);

        // If the stats have improved, raise an event. If they have declined, raise an event
        if (comparison.IsSignificantIncrease)
        {
            RaiseDomainEvent(new FieldingImprovementEvent(SeasonYear, PlayerMlbId, comparison,
                FieldingStatsByGamesChronologically[^1].GameDate));
        }
        else if (comparison.IsSignificantDecrease)
        {
            RaiseDomainEvent(new FieldingDeclineEvent(SeasonYear, PlayerMlbId, comparison,
                FieldingStatsByGamesChronologically[^1].GameDate));
        }

        // Set the new score
        FieldingScore = newScore;
    }

    /// <summary>
    /// Creates <see cref="PlayerStatsBySeason"/>
    /// </summary>
    /// <param name="playerMlbId">The MLB ID of the Player</param>
    /// <param name="seasonYear">The season</param>
    /// <param name="battingScore">A <see cref="PerformanceScore"/> for the player's season batting stats</param>
    /// <param name="pitchingScore">A <see cref="PerformanceScore"/> for the player's season pitching stats</param>
    /// <param name="fieldingScore">A <see cref="PerformanceScore"/> for the player's season fielding stats</param>
    /// <param name="battingStatsByGames">The player's batting stats by game</param>
    /// <param name="pitchingStatsByGames">The player's pitching stats by game</param>
    /// <param name="fieldingStatsByGames">The player's fielding stats by game</param>
    /// <returns><see cref="PlayerStatsBySeason"/></returns>
    public static PlayerStatsBySeason Create(MlbId playerMlbId, SeasonYear seasonYear,
        PerformanceScore battingScore, PerformanceScore pitchingScore, PerformanceScore fieldingScore,
        List<PlayerBattingStatsByGame> battingStatsByGames, List<PlayerPitchingStatsByGame> pitchingStatsByGames,
        List<PlayerFieldingStatsByGame> fieldingStatsByGames)
    {
        return new PlayerStatsBySeason(playerMlbId, seasonYear, battingScore: battingScore,
            pitchingScore: pitchingScore, fieldingScore: fieldingScore, battingStatsByGames, pitchingStatsByGames,
            fieldingStatsByGames);
    }
}