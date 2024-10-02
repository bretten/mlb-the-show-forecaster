using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;

/// <summary>
/// Maps <see cref="PlayerSeason"/> to other objects
/// </summary>
public sealed class PlayerSeasonMapper : IPlayerSeasonMapper
{
    /// <summary>
    /// Assesses performance
    /// </summary>
    private readonly IPerformanceAssessor _performanceAssessor;

    /// <summary>
    /// Assesses participation
    /// </summary>
    private readonly IParticipationAssessor _participationAssessor;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="performanceAssessor">Assesses performance</param>
    /// <param name="participationAssessor">Assesses participation</param>
    public PlayerSeasonMapper(IPerformanceAssessor performanceAssessor, IParticipationAssessor participationAssessor)
    {
        _performanceAssessor = performanceAssessor;
        _participationAssessor = participationAssessor;
    }

    /// <summary>
    /// Maps <see cref="PlayerSeason"/> to <see cref="PlayerStatsBySeason"/>
    /// </summary>
    /// <param name="playerSeason">The <see cref="PlayerSeason"/> to map</param>
    /// <returns>The mapped <see cref="PlayerStatsBySeason"/></returns>
    public PlayerStatsBySeason Map(PlayerSeason playerSeason)
    {
        return PlayerStatsBySeason.Create(playerSeason.PlayerMlbId, playerSeason.SeasonYear, PerformanceScore.Zero(),
            PerformanceScore.Zero(), PerformanceScore.Zero(), new List<PlayerBattingStatsByGame>(),
            new List<PlayerPitchingStatsByGame>(), new List<PlayerFieldingStatsByGame>());
    }

    /// <summary>
    /// Maps a collection of <see cref="PlayerGameBattingStats"/> to <see cref="PlayerBattingStatsByGame"/>
    /// </summary>
    /// <param name="stats">The collection of <see cref="PlayerGameBattingStats"/> to map</param>
    /// <returns>The mapped collection of <see cref="PlayerBattingStatsByGame"/></returns>
    public IEnumerable<PlayerBattingStatsByGame> MapBattingGames(IEnumerable<PlayerGameBattingStats> stats)
    {
        return stats
            .Select(x => PlayerBattingStatsByGame.Create(x.PlayerMlbId, x.SeasonYear, x.GameDate, x.GameMlbId,
                    x.TeamMlbId,
                    plateAppearances: x.PlateAppearances.Value,
                    atBats: x.AtBats.Value,
                    runs: x.Runs.Value,
                    hits: x.Hits.Value,
                    doubles: x.Doubles.Value,
                    triples: x.Triples.Value,
                    homeRuns: x.HomeRuns.Value,
                    runsBattedIn: x.RunsBattedIn.Value,
                    baseOnBalls: x.BaseOnBalls.Value,
                    intentionalWalks: x.IntentionalWalks.Value,
                    strikeouts: x.Strikeouts.Value,
                    stolenBases: x.StolenBases.Value,
                    caughtStealing: x.CaughtStealing.Value,
                    hitByPitch: x.HitByPitch.Value,
                    sacrificeBunts: x.SacrificeBunts.Value,
                    sacrificeFlies: x.SacrificeFlies.Value,
                    numberOfPitchesSeen: x.NumberOfPitchesSeen.Value,
                    leftOnBase: x.LeftOnBase.Value,
                    groundOuts: x.GroundOuts.Value,
                    groundIntoDoublePlays: x.GroundIntoDoublePlays.Value,
                    groundIntoTriplePlays: x.GroundIntoTriplePlays.Value,
                    airOuts: x.AirOuts.Value,
                    catcherInterferences: x.CatcherInterferences.Value
                )
            ).ToList();
    }

    /// <summary>
    /// Maps a collection of <see cref="PlayerGamePitchingStats"/> to <see cref="PlayerPitchingStatsByGame"/>
    /// </summary>
    /// <param name="stats">The collection of <see cref="PlayerGamePitchingStats"/> to map</param>
    /// <returns>The mapped collection of <see cref="PlayerPitchingStatsByGame"/></returns>
    public IEnumerable<PlayerPitchingStatsByGame> MapPitchingGames(IEnumerable<PlayerGamePitchingStats> stats)
    {
        return stats
            .Select(x => PlayerPitchingStatsByGame.Create(x.PlayerMlbId, x.SeasonYear, x.GameDate, x.GameMlbId,
                    x.TeamMlbId,
                    win: x.Win,
                    loss: x.Loss,
                    gameStarted: x.GameStarted,
                    gameFinished: x.GameFinished,
                    completeGame: x.CompleteGame,
                    shutout: x.Shutout,
                    hold: x.Hold,
                    save: x.Save,
                    blownSave: x.BlownSave,
                    saveOpportunity: x.SaveOpportunity,
                    inningsPitched: x.InningsPitched.Value,
                    hits: x.Hits.Value,
                    doubles: x.Doubles.Value,
                    triples: x.Triples.Value,
                    homeRuns: x.HomeRuns.Value,
                    runs: x.Runs.Value,
                    earnedRuns: x.EarnedRuns.Value,
                    strikeouts: x.Strikeouts.Value,
                    baseOnBalls: x.BaseOnBalls.Value,
                    intentionalWalks: x.IntentionalWalks.Value,
                    hitBatsmen: x.HitBatsmen.Value,
                    outs: x.Outs.Value,
                    groundOuts: x.GroundOuts.Value,
                    airOuts: x.AirOuts.Value,
                    groundIntoDoublePlays: x.GroundIntoDoublePlays.Value,
                    numberOfPitches: x.NumberOfPitches.Value,
                    strikes: x.Strikes.Value,
                    wildPitches: x.WildPitches.Value,
                    balks: x.Balks.Value,
                    battersFaced: x.BattersFaced.Value,
                    atBats: x.AtBats.Value,
                    stolenBases: x.StolenBases.Value,
                    caughtStealing: x.CaughtStealing.Value,
                    pickoffs: x.Pickoffs.Value,
                    inheritedRunners: x.InheritedRunners.Value,
                    inheritedRunnersScored: x.InheritedRunnersScored.Value,
                    catcherInterferences: x.CatcherInterferences.Value,
                    sacrificeBunts: x.SacrificeBunts.Value,
                    sacrificeFlies: x.SacrificeFlies.Value
                )
            ).ToList();
    }

    /// <summary>
    /// Maps a collection of <see cref="PlayerGameFieldingStats"/> to <see cref="PlayerFieldingStatsByGame"/>
    /// </summary>
    /// <param name="stats">The collection of <see cref="PlayerGameFieldingStats"/> to map</param>
    /// <returns>The mapped collection of <see cref="PlayerFieldingStatsByGame"/></returns>
    public IEnumerable<PlayerFieldingStatsByGame> MapFieldingGames(IEnumerable<PlayerGameFieldingStats> stats)
    {
        return stats
            .Select(x => PlayerFieldingStatsByGame.Create(x.PlayerMlbId, x.SeasonYear, x.GameDate, x.GameMlbId,
                    x.TeamMlbId,
                    position: x.Position,
                    gameStarted: x.GameStarted,
                    inningsPlayed: x.InningsPlayed.Value,
                    assists: x.Assists.Value,
                    putouts: x.Putouts.Value,
                    errors: x.Errors.Value,
                    throwingErrors: x.ThrowingErrors.Value,
                    doublePlays: x.DoublePlays.Value,
                    triplePlays: x.TriplePlays.Value,
                    caughtStealing: x.CaughtStealing.Value,
                    stolenBases: x.StolenBases.Value,
                    passedBalls: x.PassedBalls.Value,
                    catcherInterferences: x.CatcherInterferences.Value,
                    wildPitches: x.WildPitches.Value,
                    pickoffs: x.Pickoffs.Value
                )
            ).ToList();
    }

    /// <summary>
    /// Maps a <see cref="PlayerStatsBySeason"/> to <see cref="PlayerSeasonPerformanceMetrics"/> using
    /// stats from the specified time period
    /// </summary>
    /// <param name="playerStatsBySeason"><see cref="PlayerStatsBySeason"/></param>
    /// <param name="start">The start date of the time period</param>
    /// <param name="end">The end date of the time period</param>
    /// <returns><see cref="PlayerSeasonPerformanceMetrics"/></returns>
    public PlayerSeasonPerformanceMetrics MapToPlayerSeasonPerformanceMetrics(PlayerStatsBySeason playerStatsBySeason,
        DateOnly start, DateOnly end)
    {
        var metricsByDate = new List<PerformanceMetricsByDate>();

        // Calculate cumulative stats for each date in the time period
        var i = start;
        while (i <= end)
        {
            var battingStats = playerStatsBySeason.BattingStatsFor(start, i);
            var battingScore = _performanceAssessor.AssessBatting(battingStats);
            var significantBattingParticipation = _participationAssessor.AssessBatting(start, i, battingStats);

            var pitchingStats = playerStatsBySeason.PitchingStatsFor(start, i);
            var pitchingScore = _performanceAssessor.AssessPitching(pitchingStats);
            var significantPitchingParticipation = _participationAssessor.AssessPitching(start, i, pitchingStats);

            var fieldingStats = playerStatsBySeason.FieldingStatsFor(start, i);
            var fieldingScore = _performanceAssessor.AssessFielding(fieldingStats);
            var significantFieldingParticipation = _participationAssessor.AssessFielding(start, i, fieldingStats);

            metricsByDate.Add(new PerformanceMetricsByDate(
                i, BattingScore: battingScore.Value, SignificantBattingParticipation: significantBattingParticipation,
                PitchingScore: pitchingScore.Value, SignificantPitchingParticipation: significantPitchingParticipation,
                FieldingScore: fieldingScore.Value, SignificantFieldingParticipation: significantFieldingParticipation,
                BattingAverage: battingStats.BattingAverage.Value,
                OnBasePercentage: battingStats.OnBasePercentage.Value,
                Slugging: battingStats.Slugging.Value,
                EarnedRunAverage: pitchingStats.EarnedRunAverage.Value,
                OpponentsBattingAverage: pitchingStats.OpponentsBattingAverage.Value,
                StrikeoutsPer9: pitchingStats.StrikeoutsPer9.Value,
                BaseOnBallsPer9: pitchingStats.BaseOnBallsPer9.Value,
                HomeRunsPer9: pitchingStats.HomeRunsPer9.Value,
                FieldingPercentage: fieldingStats.FieldingPercentage.Value));

            i = i.AddDays(1);
        }

        return new PlayerSeasonPerformanceMetrics(playerStatsBySeason.SeasonYear, playerStatsBySeason.PlayerMlbId,
            metricsByDate);
    }
}