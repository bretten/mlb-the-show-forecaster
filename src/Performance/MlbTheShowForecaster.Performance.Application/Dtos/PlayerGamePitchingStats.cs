using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;

/// <summary>
/// A player's pitching stats for a single game
/// </summary>
/// <param name="PlayerMlbId">The MLB ID of the Player</param>
/// <param name="SeasonYear">The season</param>
/// <param name="GameDate">The date of the game</param>
/// <param name="GameMlbId">The MLB ID of the game</param>
/// <param name="TeamMlbId">The MLB ID of the team</param>
/// <param name="Win">True if the pitcher got the win for this game</param>
/// <param name="Loss">True if the pitcher got the loss for this game</param>
/// <param name="GameStarted">True if the pitcher started this game</param>
/// <param name="GameFinished">True if the pitcher was the last pitcher in the game as a relief pitcher</param>
/// <param name="CompleteGame">True if the pitcher pitched the whole game</param>
/// <param name="Shutout">True if the pitcher pitched a shutout</param>
/// <param name="Hold">True if the pitcher earned a hold</param>
/// <param name="Save">True if the pitcher earned a save</param>
/// <param name="BlownSave">True if the pitcher failed to earn a save</param>
/// <param name="SaveOpportunity">True if this game was a save opportunity for the pitcher</param>
/// <param name="InningsPitched">The number of innings pitched</param>
/// <param name="Hits">The number of hits given up</param>
/// <param name="Doubles">The number of doubles given up</param>
/// <param name="Triples">The number of triples given up</param>
/// <param name="HomeRuns">The number of home runs given up</param>
/// <param name="Runs">The number of runs given up</param>
/// <param name="EarnedRuns">The number of earned runs given up (runs that were a result of this pitcher giving up a hit)</param>
/// <param name="Strikeouts">The number of strikeouts</param>
/// <param name="BaseOnBalls">The number of times the pitcher walked the batter</param>
/// <param name="IntentionalWalks">The number of times the pitcher intentionally walked the batter</param>
/// <param name="HitBatsmen">The number of times the pitcher hit a batter with a pitch</param>
/// <param name="Outs">The number of outs made by the team while this pitcher was active</param>
/// <param name="GroundOuts">The number of times a pitch resulted in a ground out</param>
/// <param name="AirOuts">The number of times a pitch resulted in a air/fly out</param>
/// <param name="GroundIntoDoublePlays">The number of double play ground outs induced</param>
/// <param name="NumberOfPitches">The number of pitches thrown this game</param>
/// <param name="Strikes">The number of strikes thrown by the pitcher</param>
/// <param name="WildPitches">The number of wild pitches thrown</param>
/// <param name="Balks">The number of balks</param>
/// <param name="BattersFaced">The number of batters faced, pitcher version of plate appearance</param>
/// <param name="AtBats">The number of at-bats</param>
/// <param name="StolenBases">The number of bases stolen against this pitcher</param>
/// <param name="CaughtStealing">The number of times a runner was caught stealing against this pitcher</param>
/// <param name="Pickoffs">The number of pick offs made by this pitcher</param>
/// <param name="InheritedRunners">The number of runners on base when the pitcher enters the game</param>
/// <param name="InheritedRunnersScored">The number of inherited runners allowed to score</param>
/// <param name="CatcherInterferences">The number of times a catcher interfered with the batter's plate appearance</param>
/// <param name="SacrificeBunts">The number of sacrifice bunts made against the pitcher</param>
/// <param name="SacrificeFlies">The number of sacrifice flies made against the pitcher</param>
public readonly record struct PlayerGamePitchingStats(
    MlbId PlayerMlbId,
    SeasonYear SeasonYear,
    DateTime GameDate,
    MlbId GameMlbId,
    MlbId TeamMlbId,
    bool Win,
    bool Loss,
    bool GameStarted,
    bool GameFinished,
    bool CompleteGame,
    bool Shutout,
    bool Hold,
    bool Save,
    bool BlownSave,
    bool SaveOpportunity,
    InningsCount InningsPitched,
    NaturalNumber Hits,
    NaturalNumber Doubles,
    NaturalNumber Triples,
    NaturalNumber HomeRuns,
    NaturalNumber Runs,
    NaturalNumber EarnedRuns,
    NaturalNumber Strikeouts,
    NaturalNumber BaseOnBalls,
    NaturalNumber IntentionalWalks,
    NaturalNumber HitBatsmen,
    NaturalNumber Outs,
    NaturalNumber GroundOuts,
    NaturalNumber AirOuts,
    NaturalNumber GroundIntoDoublePlays,
    NaturalNumber NumberOfPitches,
    NaturalNumber Strikes,
    NaturalNumber WildPitches,
    NaturalNumber Balks,
    NaturalNumber BattersFaced,
    NaturalNumber AtBats,
    NaturalNumber StolenBases,
    NaturalNumber CaughtStealing,
    NaturalNumber Pickoffs,
    NaturalNumber InheritedRunners,
    NaturalNumber InheritedRunnersScored,
    NaturalNumber CatcherInterferences,
    NaturalNumber SacrificeBunts,
    NaturalNumber SacrificeFlies);