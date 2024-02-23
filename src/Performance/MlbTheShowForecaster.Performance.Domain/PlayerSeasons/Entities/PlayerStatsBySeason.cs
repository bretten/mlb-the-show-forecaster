using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Participation;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Pitching;
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
    public MlbId PlayerId { get; }

    /// <summary>
    /// The season
    /// </summary>
    public SeasonYear SeasonYear { get; }

    /// <summary>
    /// The player's batting stats by game in chronological order
    /// </summary>
    public List<PlayerBattingStatsByGame> BattingStatsByGamesChronologically =>
        _battingStatsByGames.OrderBy(x => x.GameDate).ToList();

    /// <summary>
    /// The player's pitching stats by game in chronological order
    /// </summary>
    public List<PlayerPitchingStatsByGame> PitchingStatsByGamesChronologically =>
        _pitchingStatsByGames.OrderBy(x => x.GameDate).ToList();

    /// <summary>
    /// The player's fielding stats by game in chronological order
    /// </summary>
    public List<PlayerFieldingStatsByGame> FieldingStatsByGamesChronologically =>
        _fieldingStatsByGames.OrderBy(x => x.GameDate).ToList();

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

    private PlayerStatsBySeason(MlbId playerId, SeasonYear seasonYear,
        List<PlayerBattingStatsByGame> battingStatsByGames, List<PlayerPitchingStatsByGame> pitchingStatsByGames,
        List<PlayerFieldingStatsByGame> fieldingStatsByGames) : base(Guid.NewGuid())
    {
        PlayerId = playerId;
        SeasonYear = seasonYear;
        _battingStatsByGames = battingStatsByGames;
        _pitchingStatsByGames = pitchingStatsByGames;
        _fieldingStatsByGames = fieldingStatsByGames;
    }

    public void LogBattingGame(PlayerBattingStatsByGame stats)
    {
        _battingStatsByGames.Add(stats);
        RaiseDomainEvent(new PlayerBattedInGameEvent(PlayerId, stats.GameDate));
    }

    public void LogPitchingGame(PlayerPitchingStatsByGame stats)
    {
        _pitchingStatsByGames.Add(stats);
        RaiseDomainEvent(new PlayerPitchedInGameEvent(PlayerId, stats.GameDate));
    }

    public void LogFieldingGame(PlayerFieldingStatsByGame stats)
    {
        _fieldingStatsByGames.Add(stats);
        RaiseDomainEvent(new PlayerFieldedInGameEvent(PlayerId, stats.GameDate));
    }

    public void AssessPerformanceToDate(DateTime comparisonDate, decimal percentImprovementThreshold = 20)
    {
        AssessBattingPerformance(comparisonDate, percentImprovementThreshold);
        AssessPitchingPerformance(comparisonDate, percentImprovementThreshold);
        AssessFieldingPerformance(comparisonDate, percentImprovementThreshold);
    }

    private void AssessBattingPerformance(DateTime comparisonDate, decimal percentImprovementThreshold)
    {
        var statsBefore = BattingStatsBeforeDate(comparisonDate);
        var statsSince = BattingStatsSinceDate(comparisonDate);
        var comparison = PlayerBattingPeriodComparison.Create(PlayerId, comparisonDate,
            plateAppearancesBeforeComparisonDate: statsBefore.PlateAppearances.Value,
            onBasePlusSluggingBeforeComparisonDate: statsBefore.OnBasePlusSlugging.Value,
            plateAppearancesSinceComparisonDate: statsSince.PlateAppearances.Value,
            onBasePlusSluggingSinceComparisonDate: statsSince.OnBasePlusSlugging.Value
        );

        if (comparison.PercentageChange >= percentImprovementThreshold)
        {
            RaiseDomainEvent(new BattingImprovementEvent(comparison));
        }
        else if (comparison.PercentageChange <= -percentImprovementThreshold)
        {
            RaiseDomainEvent(new BattingDeclineEvent(comparison));
        }

        // switch (comparison.PercentageChange)
        // {
        //     case > 20:
        //         RaiseDomainEvent(new BattingImprovementEvent(comparison));
        //         break;
        //     case < -20:
        //         RaiseDomainEvent(new BattingDeclineEvent(comparison));
        //         break;
        // }
    }

    private void AssessPitchingPerformance(DateTime comparisonDate, decimal percentImprovementThreshold)
    {
        var statsBefore = PitchingStatsBeforeDate(comparisonDate);
        var statsSince = PitchingStatsSinceDate(comparisonDate);
        var comparison = PlayerPitchingPeriodComparison.Create(PlayerId, comparisonDate,
            inningsPitchedBeforeComparisonDate: statsBefore.InningsPitched.Value,
            battersFacedBeforeComparisonDate: statsBefore.BattersFaced.Value,
            earnedRunAverageBeforeComparisonDate: statsBefore.EarnedRunAverage.Value,
            inningsPitchedSinceComparisonDate: statsSince.InningsPitched.Value,
            battersFacedSinceComparisonDate: statsSince.BattersFaced.Value,
            earnedRunAverageSinceComparisonDate: statsSince.EarnedRunAverage.Value
        );

        if (comparison.PercentageChange >= percentImprovementThreshold)
        {
            RaiseDomainEvent(new PitchingImprovementEvent(comparison));
        }
        else if (comparison.PercentageChange <= -percentImprovementThreshold)
        {
            RaiseDomainEvent(new PitchingDeclineEvent(comparison));
        }

        // switch (comparison.PercentageChange)
        // {
        //     case > 20:
        //         RaiseDomainEvent(new PitchingImprovementEvent(comparison));
        //         break;
        //     case < -20:
        //         RaiseDomainEvent(new PitchingDeclineEvent(comparison));
        //         break;
        // }
    }

    private void AssessFieldingPerformance(DateTime comparisonDate, decimal percentImprovementThreshold)
    {
        var statsBefore = FieldingStatsBeforeDate(comparisonDate);
        var statsSince = FieldingStatsSinceDate(comparisonDate);
        var comparison = PlayerFieldingPeriodComparison.Create(PlayerId, comparisonDate,
            totalChancesBeforeComparisonDate: (int)statsBefore.TotalChances.Value,
            fieldingPercentageBeforeComparisonDate: statsBefore.FieldingPercentage.Value,
            totalChancesSinceComparisonDate: (int)statsSince.TotalChances.Value,
            fieldingPercentageSinceComparisonDate: statsSince.FieldingPercentage.Value
        );

        if (comparison.PercentageChange >= percentImprovementThreshold)
        {
            RaiseDomainEvent(new FieldingImprovementEvent(comparison));
        }
        else if (comparison.PercentageChange <= -percentImprovementThreshold)
        {
            RaiseDomainEvent(new FieldingDeclineEvent(comparison));
        }
        // switch (comparison.PercentageChange)
        // {
        //     case > 20:
        //         RaiseDomainEvent(new FieldingImprovementEvent(comparison));
        //         break;
        //     case < -20:
        //         RaiseDomainEvent(new FieldingDeclineEvent(comparison));
        //         break;
        // }
    }

    private BattingStats BattingStatsBeforeDate(DateTime date) =>
        BattingStats.Create(_battingStatsByGames.Where(x => x.GameDate < date));

    private BattingStats BattingStatsSinceDate(DateTime date) =>
        BattingStats.Create(_battingStatsByGames.Where(x => x.GameDate >= date));

    private PitchingStats PitchingStatsBeforeDate(DateTime date) =>
        PitchingStats.Create(_pitchingStatsByGames.Where(x => x.GameDate < date));

    private PitchingStats PitchingStatsSinceDate(DateTime date) =>
        PitchingStats.Create(_pitchingStatsByGames.Where(x => x.GameDate >= date));

    private FieldingStats FieldingStatsBeforeDate(DateTime date) =>
        FieldingStats.Create(_fieldingStatsByGames.Where(x => x.GameDate < date));

    private FieldingStats FieldingStatsSinceDate(DateTime date) =>
        FieldingStats.Create(_fieldingStatsByGames.Where(x => x.GameDate >= date));

    public static PlayerStatsBySeason Create(MlbId playerId, SeasonYear seasonYear,
        List<PlayerBattingStatsByGame> battingStatsByGames, List<PlayerPitchingStatsByGame> pitchingStatsByGames,
        List<PlayerFieldingStatsByGame> fieldingStatsByGames)
    {
        return new PlayerStatsBySeason(playerId, seasonYear, battingStatsByGames, pitchingStatsByGames,
            fieldingStatsByGames);
    }
}