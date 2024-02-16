using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
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
    }

    public BattingStats SeasonBattingStats => BattingStats.Create(
        plateAppearances: _battingStatsByGames.Sum(x => x.PlateAppearances.Value),
        atBats: _battingStatsByGames.Sum(x => x.AtBats.Value),
        runs: _battingStatsByGames.Sum(x => x.Runs.Value),
        hits: _battingStatsByGames.Sum(x => x.Hits.Value),
        doubles: _battingStatsByGames.Sum(x => x.Doubles.Value),
        triples: _battingStatsByGames.Sum(x => x.Triples.Value),
        homeRuns: _battingStatsByGames.Sum(x => x.HomeRuns.Value),
        runsBattedIn: _battingStatsByGames.Sum(x => x.RunsBattedIn.Value),
        baseOnBalls: _battingStatsByGames.Sum(x => x.BaseOnBalls.Value),
        intentionalWalks: _battingStatsByGames.Sum(x => x.IntentionalWalks.Value),
        strikeouts: _battingStatsByGames.Sum(x => x.Strikeouts.Value),
        stolenBases: _battingStatsByGames.Sum(x => x.StolenBases.Value),
        caughtStealing: _battingStatsByGames.Sum(x => x.CaughtStealing.Value),
        hitByPitch: _battingStatsByGames.Sum(x => x.HitByPitch.Value),
        sacrificeBunts: _battingStatsByGames.Sum(x => x.SacrificeBunts.Value),
        sacrificeFlies: _battingStatsByGames.Sum(x => x.SacrificeFlies.Value),
        numberOfPitchesSeen: _battingStatsByGames.Sum(x => x.NumberOfPitchesSeen.Value),
        leftOnBase: _battingStatsByGames.Sum(x => x.LeftOnBase.Value),
        groundOuts: _battingStatsByGames.Sum(x => x.GroundOuts.Value),
        groundIntoDoublePlays: _battingStatsByGames.Sum(x => x.GroundIntoDoublePlays.Value),
        groundIntoTriplePlays: _battingStatsByGames.Sum(x => x.GroundIntoTriplePlays.Value),
        airOuts: _battingStatsByGames.Sum(x => x.AirOuts.Value),
        catchersInterference: _battingStatsByGames.Sum(x => x.CatchersInterference.Value)
    );

    public PitchingStats SeasonPitchingStats => PitchingStats.Create(wins: _pitchingStatsByGames.Sum(x => x.Wins.Value),
        losses: _pitchingStatsByGames.Sum(x => x.Losses.Value),
        gamesStarted: _pitchingStatsByGames.Sum(x => x.GamesStarted.Value),
        gamesFinished: _pitchingStatsByGames.Sum(x => x.GamesFinished.Value),
        completeGames: _pitchingStatsByGames.Sum(x => x.CompleteGames.Value),
        shutouts: _pitchingStatsByGames.Sum(x => x.Shutouts.Value),
        holds: _pitchingStatsByGames.Sum(x => x.Holds.Value),
        saves: _pitchingStatsByGames.Sum(x => x.Saves.Value),
        blownSaves: _pitchingStatsByGames.Sum(x => x.BlownSaves.Value),
        saveOpportunities: _pitchingStatsByGames.Sum(x => x.SaveOpportunities.Value),
        inningsPitched: _pitchingStatsByGames.Sum(x => x.InningsPitched.Value),
        hits: _pitchingStatsByGames.Sum(x => x.Hits.Value),
        doubles: _pitchingStatsByGames.Sum(x => x.Doubles.Value),
        triples: _pitchingStatsByGames.Sum(x => x.Triples.Value),
        homeRuns: _pitchingStatsByGames.Sum(x => x.HomeRuns.Value),
        runs: _pitchingStatsByGames.Sum(x => x.Runs.Value),
        earnedRuns: _pitchingStatsByGames.Sum(x => x.EarnedRuns.Value),
        strikeouts: _pitchingStatsByGames.Sum(x => x.Strikeouts.Value),
        baseOnBalls: _pitchingStatsByGames.Sum(x => x.BaseOnBalls.Value),
        intentionalWalks: _pitchingStatsByGames.Sum(x => x.IntentionalWalks.Value),
        hitBatsmen: _pitchingStatsByGames.Sum(x => x.HitBatsmen.Value),
        outs: _pitchingStatsByGames.Sum(x => x.Outs.Value),
        groundOuts: _pitchingStatsByGames.Sum(x => x.GroundOuts.Value),
        airOuts: _pitchingStatsByGames.Sum(x => x.AirOuts.Value),
        groundIntoDoublePlays: _pitchingStatsByGames.Sum(x => x.GroundIntoDoublePlays.Value),
        numberOfPitches: _pitchingStatsByGames.Sum(x => x.NumberOfPitches.Value),
        strikes: _pitchingStatsByGames.Sum(x => x.Strikes.Value),
        wildPitches: _pitchingStatsByGames.Sum(x => x.WildPitches.Value),
        balks: _pitchingStatsByGames.Sum(x => x.Balks.Value),
        battersFaced: _pitchingStatsByGames.Sum(x => x.BattersFaced.Value),
        atBats: _pitchingStatsByGames.Sum(x => x.AtBats.Value),
        stolenBases: _pitchingStatsByGames.Sum(x => x.StolenBases.Value),
        caughtStealing: _pitchingStatsByGames.Sum(x => x.CaughtStealing.Value),
        pickOffs: _pitchingStatsByGames.Sum(x => x.PickOffs.Value),
        inheritedRunners: _pitchingStatsByGames.Sum(x => x.InheritedRunners.Value),
        inheritedRunnersScored: _pitchingStatsByGames.Sum(x => x.InheritedRunnersScored.Value),
        catchersInterferences: _pitchingStatsByGames.Sum(x => x.CatchersInterferences.Value),
        sacrificeBunts: _pitchingStatsByGames.Sum(x => x.SacrificeBunts.Value),
        sacrificeFlies: _pitchingStatsByGames.Sum(x => x.SacrificeFlies.Value)
    );

    public FieldingStats SeasonAggregateFieldingStats => FieldingStats.Create(
        position: _fieldingStatsByGames.First().Position,
        gamesStarted: _fieldingStatsByGames.Sum(x => x.GamesStarted.Value),
        inningsPlayed: _fieldingStatsByGames.Sum(x => x.InningsPlayed.Value),
        assists: _fieldingStatsByGames.Sum(x => x.Assists.Value),
        putOuts: _fieldingStatsByGames.Sum(x => x.PutOuts.Value),
        errors: _fieldingStatsByGames.Sum(x => x.Errors.Value),
        throwingErrors: _fieldingStatsByGames.Sum(x => x.ThrowingErrors.Value),
        doublePlays: _fieldingStatsByGames.Sum(x => x.DoublePlays.Value),
        triplePlays: _fieldingStatsByGames.Sum(x => x.TriplePlays.Value),
        caughtStealing: _fieldingStatsByGames.Sum(x => x.CaughtStealing.Value),
        stolenBases: _fieldingStatsByGames.Sum(x => x.StolenBases.Value),
        passedBalls: _fieldingStatsByGames.Sum(x => x.PassedBalls.Value),
        catchersInterference: _fieldingStatsByGames.Sum(x => x.CatchersInterference.Value),
        wildPitches: _fieldingStatsByGames.Sum(x => x.WildPitches.Value),
        pickOffs: _fieldingStatsByGames.Sum(x => x.PickOffs.Value)
    );

    public Dictionary<Position, FieldingStats> SeasonFieldingStatsByPosition => _fieldingStatsByGames
        .GroupBy(x => x.Position)
        .ToDictionary(x => x.Key, y => FieldingStats.Create(
                position: y.First().Position,
                gamesStarted: y.Sum(x => x.GamesStarted.Value),
                inningsPlayed: y.Sum(x => x.InningsPlayed.Value),
                assists: y.Sum(x => x.Assists.Value),
                putOuts: y.Sum(x => x.PutOuts.Value),
                errors: y.Sum(x => x.Errors.Value),
                throwingErrors: y.Sum(x => x.ThrowingErrors.Value),
                doublePlays: y.Sum(x => x.DoublePlays.Value),
                triplePlays: y.Sum(x => x.TriplePlays.Value),
                caughtStealing: y.Sum(x => x.CaughtStealing.Value),
                stolenBases: y.Sum(x => x.StolenBases.Value),
                passedBalls: y.Sum(x => x.PassedBalls.Value),
                catchersInterference: y.Sum(x => x.CatchersInterference.Value),
                wildPitches: y.Sum(x => x.WildPitches.Value),
                pickOffs: y.Sum(x => x.PickOffs.Value)
            )
        );

    public static PlayerStatsBySeason Create(MlbId playerId, SeasonYear seasonYear,
        List<PlayerBattingStatsByGame> battingStatsByGames, List<PlayerPitchingStatsByGame> pitchingStatsByGames,
        List<PlayerFieldingStatsByGame> fieldingStatsByGames)
    {
        return new PlayerStatsBySeason(playerId, seasonYear, battingStatsByGames, pitchingStatsByGames,
            fieldingStatsByGames);
    }
}