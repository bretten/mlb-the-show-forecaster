using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;

/// <summary>
/// A player's fielding stats for a single game
/// </summary>
/// <param name="PlayerMlbId">The MLB ID of the Player</param>
/// <param name="SeasonYear">The season</param>
/// <param name="GameDate">The date of the game</param>
/// <param name="GameMlbId">The MLB ID of the game</param>
/// <param name="TeamMlbId">The MLB ID of the team</param>
/// <param name="Position">The position the player is fielding</param>
/// <param name="GameStarted">The number of times the player started the game at this <see cref="Position"/></param>
/// <param name="InningsPlayed">The number of innings this player fielded at this <see cref="Position"/></param>
/// <param name="Assists">The number of outs on a play where the fielder touched the ball excluding when this player does the actual putout</param>
/// <param name="PutOuts">The number of times the fielder tags, forces, or appeals a runner and they are called out</param>
/// <param name="Errors">The number of times a fielder fails to make a play that is considered to be doable with common effort</param>
/// <param name="ThrowingErrors">The number of errors that were the result of a bad throw</param>
/// <param name="DoublePlays">The number of double plays where the fielder recorded a putout or an assist</param>
/// <param name="TriplePlays">The number of triple plays where the fielder recorded a putout or an assist</param>
/// <param name="CaughtStealing">Catcher stat: The number of times the catcher was able to throw out a base runner attempting to steal</param>
/// <param name="StolenBases">Catcher stat: The number of times a base runner successfully stole a base against the catcher</param>
/// <param name="PassedBalls">Catcher stat: The number of times the catcher dropped the ball and a runner was able to advance</param>
/// <param name="CatcherInterferences">Catcher stat: The number of times a catcher interfered with the batter's plate appearance</param>
/// <param name="WildPitches">Catcher stat: The number of wild pitches the catcher saw from the pitcher</param>
/// <param name="PickOffs">Catcher stat: The number of pick offs made by the pitcher while this catcher was behind the plate</param>
public readonly record struct PlayerGameFieldingStats(
    MlbId PlayerMlbId,
    SeasonYear SeasonYear,
    DateTime GameDate,
    MlbId GameMlbId,
    MlbId TeamMlbId,
    Position Position,
    bool GameStarted,
    InningsCount InningsPlayed,
    NaturalNumber Assists,
    NaturalNumber PutOuts,
    NaturalNumber Errors,
    NaturalNumber ThrowingErrors,
    NaturalNumber DoublePlays,
    NaturalNumber TriplePlays,
    NaturalNumber CaughtStealing,
    NaturalNumber StolenBases,
    NaturalNumber PassedBalls,
    NaturalNumber CatcherInterferences,
    NaturalNumber WildPitches,
    NaturalNumber PickOffs);