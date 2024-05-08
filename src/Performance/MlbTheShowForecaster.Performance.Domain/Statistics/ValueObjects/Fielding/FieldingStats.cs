using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.BaseRunning;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared.Extensions;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;

/// <summary>
/// Statistics for fielding
/// </summary>
public class FieldingStats : ValueObject
{
    /// <summary>
    /// The position the player is fielding
    /// </summary>
    public Position Position { get; }

    /// <summary>
    /// The number of times the player started the game at this <see cref="Position"/>
    /// </summary>
    public NaturalNumber GamesStarted { get; }

    /// <summary>
    /// The number of innings this player fielded at this <see cref="Position"/>
    /// </summary>
    public InningsCount InningsPlayed { get; }

    /// <summary>
    /// The number of outs on a play where the fielder touched the ball excluding when this player does the actual putout
    /// </summary>
    public NaturalNumber Assists { get; }

    /// <summary>
    /// The number of times the fielder tags, forces, or appeals a runner and they are called out
    /// </summary>
    public NaturalNumber Putouts { get; }

    /// <summary>
    /// The number of times a fielder fails to make a play that is considered to be doable with common effort
    /// </summary>
    public NaturalNumber Errors { get; }

    /// <summary>
    /// The number of errors that were the result of a bad throw
    /// </summary>
    public NaturalNumber ThrowingErrors { get; }

    /// <summary>
    /// The number of double plays where the fielder recorded a putout or an assist
    /// </summary>
    public NaturalNumber DoublePlays { get; }

    /// <summary>
    /// The number of triple plays where the fielder recorded a putout or an assist
    /// </summary>
    public NaturalNumber TriplePlays { get; }

    /// <summary>
    /// Fielding percentage
    /// </summary>
    public FieldingPercentage FieldingPercentage =>
        FieldingPercentage.Create(Assists.Value, Putouts.Value, Errors.Value);

    /// <summary>
    /// Total chances
    /// </summary>
    public TotalChances TotalChances => TotalChances.Create(Assists.Value, Putouts.Value, Errors.Value);

    /// <summary>
    /// Range factor per 9 innings
    /// </summary>
    public RangeFactorPerNine RangeFactorPer9 =>
        RangeFactorPerNine.Create(Assists.Value, Putouts.Value, InningsPlayed.Value);

    /// <summary>
    /// Catcher stat: The number of times the catcher was able to throw out a base runner attempting to steal
    /// </summary>
    public NaturalNumber CaughtStealing { get; }

    /// <summary>
    /// Catcher stat: The number of times a base runner successfully stole a base against the catcher
    /// </summary>
    public NaturalNumber StolenBases { get; }

    /// <summary>
    /// Catcher stat: The number of times the catcher dropped the ball and a runner was able to advance
    /// </summary>
    public NaturalNumber PassedBalls { get; }

    /// <summary>
    /// Catcher stat: The number of times a catcher interfered with the batter's plate appearance
    /// </summary>
    public NaturalNumber CatcherInterferences { get; }

    /// <summary>
    /// Catcher stat: The number of wild pitches the catcher saw from the pitcher
    /// </summary>
    public NaturalNumber WildPitches { get; }

    /// <summary>
    /// Catcher stat: The number of pick offs made by the pitcher while this catcher was behind the plate
    /// </summary>
    public NaturalNumber Pickoffs { get; }

    /// <summary>
    /// Catcher stat: Stolen base percentage
    /// </summary>
    public StolenBasePercentage StolenBasePercentage =>
        StolenBasePercentage.Create(StolenBases.Value, CaughtStealing.Value);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="position">The position the player is fielding</param>
    /// <param name="gamesStarted">The number of times the player started the game at this <see cref="Position"/></param>
    /// <param name="inningsPlayed">The number of innings this player fielded at this <see cref="Position"/></param>
    /// <param name="assists">The number of outs on a play where the fielder touched the ball excluding when this player does the actual putout</param>
    /// <param name="putouts">The number of times the fielder tags, forces, or appeals a runner and they are called out</param>
    /// <param name="errors">The number of times a fielder fails to make a play that is considered to be doable with common effort</param>
    /// <param name="throwingErrors">The number of errors that were the result of a bad throw</param>
    /// <param name="doublePlays">The number of double plays where the fielder recorded a putout or an assist</param>
    /// <param name="triplePlays">The number of triple plays where the fielder recorded a putout or an assist</param>
    /// <param name="caughtStealing">Catcher stat: The number of times the catcher was able to throw out a base runner attempting to steal</param>
    /// <param name="stolenBases">Catcher stat: The number of times a base runner successfully stole a base against the catcher</param>
    /// <param name="passedBalls">Catcher stat: The number of times the catcher dropped the ball and a runner was able to advance</param>
    /// <param name="catcherInterferences">Catcher stat: The number of times a catcher interfered with the batter's plate appearance</param>
    /// <param name="wildPitches">Catcher stat: The number of wild pitches the catcher saw from the pitcher</param>
    /// <param name="pickoffs">Catcher stat: The number of pick offs made by the pitcher while this catcher was behind the plate</param>
    protected FieldingStats(Position position, NaturalNumber gamesStarted, InningsCount inningsPlayed,
        NaturalNumber assists, NaturalNumber putouts, NaturalNumber errors, NaturalNumber throwingErrors,
        NaturalNumber doublePlays, NaturalNumber triplePlays, NaturalNumber caughtStealing, NaturalNumber stolenBases,
        NaturalNumber passedBalls, NaturalNumber catcherInterferences, NaturalNumber wildPitches,
        NaturalNumber pickoffs)
    {
        Position = position;
        GamesStarted = gamesStarted;
        InningsPlayed = inningsPlayed;
        Assists = assists;
        Putouts = putouts;
        Errors = errors;
        ThrowingErrors = throwingErrors;
        DoublePlays = doublePlays;
        TriplePlays = triplePlays;
        CaughtStealing = caughtStealing;
        StolenBases = stolenBases;
        PassedBalls = passedBalls;
        CatcherInterferences = catcherInterferences;
        WildPitches = wildPitches;
        Pickoffs = pickoffs;
    }

    /// <summary>
    /// Creates <see cref="FieldingStats"/>
    /// </summary>
    /// <param name="position">The position the player is fielding</param>
    /// <param name="gamesStarted">The number of times the player started the game at this <see cref="Position"/></param>
    /// <param name="inningsPlayed">The number of innings this player fielded at this <see cref="Position"/></param>
    /// <param name="assists">The number of outs on a play where the fielder touched the ball excluding when this player does the actual putout</param>
    /// <param name="putouts">The number of times the fielder tags, forces, or appeals a runner and they are called out</param>
    /// <param name="errors">The number of times a fielder fails to make a play that is considered to be doable with common effort</param>
    /// <param name="throwingErrors">The number of errors that were the result of a bad throw</param>
    /// <param name="doublePlays">The number of double plays where the fielder recorded a putout or an assist</param>
    /// <param name="triplePlays">The number of triple plays where the fielder recorded a putout or an assist</param>
    /// <param name="caughtStealing">Catcher stat: The number of times the catcher was able to throw out a base runner attempting to steal</param>
    /// <param name="stolenBases">Catcher stat: The number of times a base runner successfully stole a base against the catcher</param>
    /// <param name="passedBalls">Catcher stat: The number of times the catcher dropped the ball and a runner was able to advance</param>
    /// <param name="catcherInterferences">Catcher stat: The number of times a catcher interfered with the batter's plate appearance</param>
    /// <param name="wildPitches">Catcher stat: The number of wild pitches the catcher saw from the pitcher</param>
    /// <param name="pickoffs">Catcher stat: The number of pick offs made by the pitcher while this catcher was behind the plate</param>
    /// <returns><see cref="FieldingStats"/></returns>
    public static FieldingStats Create(Position position, int gamesStarted, decimal inningsPlayed, int assists,
        int putouts, int errors, int throwingErrors, int doublePlays, int triplePlays, int caughtStealing,
        int stolenBases, int passedBalls, int catcherInterferences, int wildPitches, int pickoffs)
    {
        var gs = NaturalNumber.Create(gamesStarted);
        var inn = InningsCount.Create(inningsPlayed);
        var a = NaturalNumber.Create(assists);
        var po = NaturalNumber.Create(putouts);
        var e = NaturalNumber.Create(errors);
        var te = NaturalNumber.Create(throwingErrors);
        var dp = NaturalNumber.Create(doublePlays);
        var tp = NaturalNumber.Create(triplePlays);
        var cs = NaturalNumber.Create(caughtStealing);
        var sb = NaturalNumber.Create(stolenBases);
        var pb = NaturalNumber.Create(passedBalls);
        var ci = NaturalNumber.Create(catcherInterferences);
        var wp = NaturalNumber.Create(wildPitches);
        var pk = NaturalNumber.Create(pickoffs);
        return new FieldingStats(position,
            gamesStarted: gs,
            inningsPlayed: inn,
            assists: a,
            putouts: po,
            errors: e,
            throwingErrors: te,
            doublePlays: dp,
            triplePlays: tp,
            caughtStealing: cs,
            stolenBases: sb,
            passedBalls: pb,
            catcherInterferences: ci,
            wildPitches: wp,
            pickoffs: pk);
    }

    /// <summary>
    /// Creates an aggregate <see cref="FieldingStats"/>
    /// </summary>
    /// <param name="fieldingStatsCollection">A collection of fielding stats</param>
    /// <returns>Aggregated <see cref="FieldingStats"/></returns>
    public static FieldingStats Create(IEnumerable<FieldingStats> fieldingStatsCollection)
    {
        var fieldingStatsArray = fieldingStatsCollection as FieldingStats[] ?? fieldingStatsCollection.ToArray();
        return Create(
            position: fieldingStatsArray.First().Position,
            gamesStarted: fieldingStatsArray.Sum(x => x.GamesStarted.Value),
            inningsPlayed: fieldingStatsArray.Select(x => x.InningsPlayed).SumInnings().Value,
            assists: fieldingStatsArray.Sum(x => x.Assists.Value),
            putouts: fieldingStatsArray.Sum(x => x.Putouts.Value),
            errors: fieldingStatsArray.Sum(x => x.Errors.Value),
            throwingErrors: fieldingStatsArray.Sum(x => x.ThrowingErrors.Value),
            doublePlays: fieldingStatsArray.Sum(x => x.DoublePlays.Value),
            triplePlays: fieldingStatsArray.Sum(x => x.TriplePlays.Value),
            caughtStealing: fieldingStatsArray.Sum(x => x.CaughtStealing.Value),
            stolenBases: fieldingStatsArray.Sum(x => x.StolenBases.Value),
            passedBalls: fieldingStatsArray.Sum(x => x.PassedBalls.Value),
            catcherInterferences: fieldingStatsArray.Sum(x => x.CatcherInterferences.Value),
            wildPitches: fieldingStatsArray.Sum(x => x.WildPitches.Value),
            pickoffs: fieldingStatsArray.Sum(x => x.Pickoffs.Value)
        );
    }
}