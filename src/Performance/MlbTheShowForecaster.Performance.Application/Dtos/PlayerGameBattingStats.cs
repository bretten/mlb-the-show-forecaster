using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;

/// <summary>
/// A player's batting stats for a single game
/// </summary>
/// <param name="PlayerMlbId">The MLB ID of the Player</param>
/// <param name="SeasonYear">The season</param>
/// <param name="GameDate">The date of the game</param>
/// <param name="GameMlbId">The MLB ID of the game</param>
/// <param name="TeamMlbId">The MLB ID of the team</param>
/// <param name="PlateAppearances">The number of plate appearances</param>
/// <param name="AtBats">The number of at bats</param>
/// <param name="Runs">The number of runs scored</param>
/// <param name="Hits">The number of hits</param>
/// <param name="Doubles">The number of doubles</param>
/// <param name="Triples">The number of triples</param>
/// <param name="HomeRuns">The number of home runs</param>
/// <param name="RunsBattedIn">The number of runs batted in</param>
/// <param name="BaseOnBalls">The number of walks</param>
/// <param name="IntentionalWalks">The number of intentional walks</param>
/// <param name="Strikeouts">The number of strikeouts</param>
/// <param name="StolenBases">The number of stolen bases</param>
/// <param name="CaughtStealing">The number of times caught stealing</param>
/// <param name="HitByPitch">The number of times the player was hit by a pitch</param>
/// <param name="SacrificeBunts">The number of sacrifice bunts</param>
/// <param name="SacrificeFlies">The number of sacrifice flies</param>
/// <param name="NumberOfPitchesSeen">The number of pitches the player saw as a batter</param>
/// <param name="LeftOnBase">The number of runners the player did not advance when batting and their out results in the end of the inning</param>
/// <param name="GroundOuts">The number of times the batter grounded out</param>
/// <param name="GroundIntoDoublePlays">The number of times the batter grounded into a double play</param>
/// <param name="GroundIntoTriplePlays">The number of times the batter grounded into a triple play</param>
/// <param name="AirOuts">The number of times the batter hit a fly ball that led to an out</param>
/// <param name="CatcherInterferences">The number of times a catcher interfered with the batter's plate appearance</param>
public readonly record struct PlayerGameBattingStats(
    MlbId PlayerMlbId,
    SeasonYear SeasonYear,
    DateOnly GameDate,
    MlbId GameMlbId,
    MlbId TeamMlbId,
    NaturalNumber PlateAppearances,
    NaturalNumber AtBats,
    NaturalNumber Runs,
    NaturalNumber Hits,
    NaturalNumber Doubles,
    NaturalNumber Triples,
    NaturalNumber HomeRuns,
    NaturalNumber RunsBattedIn,
    NaturalNumber BaseOnBalls,
    NaturalNumber IntentionalWalks,
    NaturalNumber Strikeouts,
    NaturalNumber StolenBases,
    NaturalNumber CaughtStealing,
    NaturalNumber HitByPitch,
    NaturalNumber SacrificeBunts,
    NaturalNumber SacrificeFlies,
    NaturalNumber NumberOfPitchesSeen,
    NaturalNumber LeftOnBase,
    NaturalNumber GroundOuts,
    NaturalNumber GroundIntoDoublePlays,
    NaturalNumber GroundIntoTriplePlays,
    NaturalNumber AirOuts,
    NaturalNumber CatcherInterferences);