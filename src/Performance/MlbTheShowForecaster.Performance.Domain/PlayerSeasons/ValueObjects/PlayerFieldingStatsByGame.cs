using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

/// <summary>
/// A player's fielding statistics for a single game
/// </summary>
public sealed class PlayerFieldingStatsByGame : FieldingStats
{
    /// <summary>
    /// The MLB ID of the Player
    /// </summary>
    public MlbId PlayerId { get; }

    /// <summary>
    /// The season
    /// </summary>
    public SeasonYear SeasonYear { get; }

    /// <summary>
    /// The date of the game
    /// </summary>
    public DateTime GameDate { get; }

    /// <summary>
    /// The MLB ID of the game
    /// </summary>
    public MlbId GameId { get; }

    /// <summary>
    /// The MLB ID of the team
    /// </summary>
    public MlbId TeamId { get; }

    /// <summary>
    /// Determines the properties that are used in equality
    /// </summary>
    /// <returns>The values of the properties that are used in equality</returns>
    protected override IEnumerable<object?> GetNestedValues()
    {
        yield return PlayerId.Value;
        yield return SeasonYear.Value;
        yield return GameDate;
        yield return GameId.Value;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerId">The MLB ID of the Player</param>
    /// <param name="seasonYear">The season</param>
    /// <param name="gameDate">The date of the game</param>
    /// <param name="gameId">The MLB ID of the game</param>
    /// <param name="teamId">The MLB ID of the team</param>
    /// <param name="position">The position the player is fielding</param>
    /// <param name="gamesStarted">The number of times the player started the game at this <see cref="Position"/></param>
    /// <param name="inningsPlayed">The number of innings this player fielded at this <see cref="Position"/></param>
    /// <param name="assists">The number of outs on a play where the fielder touched the ball excluding when this player does the actual putout</param>
    /// <param name="putOuts">The number of times the fielder tags, forces, or appeals a runner and they are called out</param>
    /// <param name="errors">The number of times a fielder fails to make a play that is considered to be doable with common effort</param>
    /// <param name="throwingErrors">The number of errors that were the result of a bad throw</param>
    /// <param name="doublePlays">The number of double plays where the fielder recorded a putout or an assist</param>
    /// <param name="triplePlays">The number of triple plays where the fielder recorded a putout or an assist</param>
    /// <param name="caughtStealing">Catcher stat: The number of times the catcher was able to throw out a base runner attempting to steal</param>
    /// <param name="stolenBases">Catcher stat: The number of times a base runner successfully stole a base against the catcher</param>
    /// <param name="passedBalls">Catcher stat: The number of times the catcher dropped the ball and a runner was able to advance</param>
    /// <param name="catchersInterference">Catcher stat: The number of times a catcher interfered with the batter's plate appearance</param>
    /// <param name="wildPitches">Catcher stat: The number of wild pitches the catcher saw from the pitcher</param>
    /// <param name="pickOffs">Catcher stat: The number of pick offs made by the pitcher while this catcher was behind the plate</param>
    private PlayerFieldingStatsByGame(MlbId playerId, SeasonYear seasonYear, DateTime gameDate, MlbId gameId,
        MlbId teamId, Position position, NaturalNumber gamesStarted, InningsCount inningsPlayed, NaturalNumber assists,
        NaturalNumber putOuts, NaturalNumber errors, NaturalNumber throwingErrors, NaturalNumber doublePlays,
        NaturalNumber triplePlays, NaturalNumber caughtStealing, NaturalNumber stolenBases, NaturalNumber passedBalls,
        NaturalNumber catchersInterference, NaturalNumber wildPitches, NaturalNumber pickOffs) : base(position,
        gamesStarted, inningsPlayed, assists, putOuts, errors, throwingErrors,
        doublePlays, triplePlays, caughtStealing, stolenBases, passedBalls, catchersInterference, wildPitches, pickOffs)
    {
        PlayerId = playerId;
        SeasonYear = seasonYear;
        GameDate = gameDate;
        GameId = gameId;
        TeamId = teamId;
    }

    /// <summary>
    /// Creates <see cref="PlayerFieldingStatsByGame"/>
    /// </summary>
    /// <param name="playerId">The MLB ID of the Player</param>
    /// <param name="seasonYear">The season</param>
    /// <param name="gameDate">The date of the game</param>
    /// <param name="gameId">The MLB ID of the game</param>
    /// <param name="teamId">The MLB ID of the team</param>
    /// <param name="position">The position the player is fielding</param>
    /// <param name="gameStarted">True if the player started the game at this position</param>
    /// <param name="inningsPlayed">The number of innings this player fielded at this position</param>
    /// <param name="assists">The number of outs on a play where the fielder touched the ball excluding when this player does the actual putout</param>
    /// <param name="putOuts">The number of times the fielder tags, forces, or appeals a runner and they are called out</param>
    /// <param name="errors">The number of times a fielder fails to make a play that is considered to be doable with common effort</param>
    /// <param name="throwingErrors">The number of errors that were the result of a bad throw</param>
    /// <param name="doublePlays">The number of double plays where the fielder recorded a putout or an assist</param>
    /// <param name="triplePlays">The number of triple plays where the fielder recorded a putout or an assist</param>
    /// <param name="caughtStealing">Catcher stat: The number of times the catcher was able to throw out a base runner attempting to steal</param>
    /// <param name="stolenBases">Catcher stat: The number of times a base runner successfully stole a base against the catcher</param>
    /// <param name="passedBalls">Catcher stat: The number of times the catcher dropped the ball and a runner was able to advance</param>
    /// <param name="catchersInterference">Catcher stat: The number of times a catcher interfered with the batter's plate appearance</param>
    /// <param name="wildPitches">Catcher stat: The number of wild pitches the catcher saw from the pitcher</param>
    /// <param name="pickOffs">Catcher stat: The number of pick offs made by the pitcher while this catcher was behind the plate</param>
    /// <returns><see cref="PlayerFieldingStatsByGame"/></returns>
    public static PlayerFieldingStatsByGame Create(MlbId playerId, SeasonYear seasonYear, DateTime gameDate,
        MlbId gameId, MlbId teamId, Position position, bool gameStarted, decimal inningsPlayed, int assists,
        int putOuts, int errors, int throwingErrors, int doublePlays, int triplePlays, int caughtStealing,
        int stolenBases, int passedBalls, int catchersInterference, int wildPitches, int pickOffs)
    {
        var gs = gameStarted ? NaturalNumber.Create(1) : NaturalNumber.Create(0);
        var inn = InningsCount.Create(inningsPlayed);
        var a = NaturalNumber.Create(assists);
        var po = NaturalNumber.Create(putOuts);
        var e = NaturalNumber.Create(errors);
        var te = NaturalNumber.Create(throwingErrors);
        var dp = NaturalNumber.Create(doublePlays);
        var tp = NaturalNumber.Create(triplePlays);
        var cs = NaturalNumber.Create(caughtStealing);
        var sb = NaturalNumber.Create(stolenBases);
        var pb = NaturalNumber.Create(passedBalls);
        var ci = NaturalNumber.Create(catchersInterference);
        var wp = NaturalNumber.Create(wildPitches);
        var pk = NaturalNumber.Create(pickOffs);
        return new PlayerFieldingStatsByGame(playerId, seasonYear, gameDate, gameId, teamId, position, gamesStarted: gs,
            inningsPlayed: inn, assists: a, putOuts: po, errors: e, throwingErrors: te, doublePlays: dp,
            triplePlays: tp, caughtStealing: cs, stolenBases: sb, passedBalls: pb, catchersInterference: ci,
            wildPitches: wp, pickOffs: pk);
    }
}