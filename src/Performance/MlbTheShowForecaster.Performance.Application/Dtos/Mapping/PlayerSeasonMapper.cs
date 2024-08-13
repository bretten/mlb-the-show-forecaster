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
}