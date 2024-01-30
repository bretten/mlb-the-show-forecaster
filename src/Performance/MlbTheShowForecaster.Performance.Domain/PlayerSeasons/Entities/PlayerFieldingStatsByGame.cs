using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;

public sealed class PlayerFieldingStatsByGame : Entity
{
    /// <summary>
    /// The MLB ID of the Player
    /// </summary>
    public MlbId MlbId { get; private set; }

    /// <summary>
    /// The season
    /// </summary>
    public int SeasonYear { get; private set; }

    /// <summary>
    /// The date of the game
    /// </summary>
    public DateTime GameDate { get; private set; }

    /// <summary>
    /// The MLB ID of the game
    /// </summary>
    public MlbId GameId { get; private set; }

    /// <summary>
    /// The MLB ID of the team
    /// </summary>
    public MlbId TeamId { get; private set; }

    /// <summary>
    /// The position the player is fielding
    /// </summary>
    public Position Position { get; private set; }

    /// <summary>
    /// True if the player started the game at this position
    /// </summary>
    public bool GameStarted { get; private set; }

    /// <summary>
    /// The number of outs on a play where the fielder touched the ball excluding when this player does the actual putout
    /// </summary>
    public int Assists { get; private set; }

    /// <summary>
    /// The number of times the fielder tags, forces, or appeals a runner and they are called out
    /// </summary>
    public int PutOuts { get; private set; }

    /// <summary>
    /// The number of times a fielder fails to make a play that is considered to be doable with common effort
    /// </summary>
    public int Errors { get; private set; }

    /// <summary>
    /// The number of innings this player fielded at this position
    /// </summary>
    public int Innings { get; private set; }

    /// <summary>
    /// The number of double plays where the fielder recorded a putout or an assist
    /// </summary>
    public int DoublePlays { get; private set; }

    /// <summary>
    /// The number of triple plays where the fielder recorded a putout or an assist
    /// </summary>
    public int TriplePlays { get; private set; }

    /// <summary>
    /// An error that was a result of a bad throw
    /// </summary>
    public int ThrowingErrors { get; private set; }

    /// <summary>
    /// Fielding percentage = PO + A / (PO + A + E)
    /// </summary>
    public float FieldingPercentage => (float)(PutOuts + Assists) / (PutOuts + Assists + Errors);

    /// <summary>
    /// Total chances = PO + A + E
    /// </summary>
    public int TotalChances => PutOuts + Assists + Errors;

    public int RangeFactorPerNineInnings => 9 * (PutOuts + Assists) / Innings;

    /// <summary>
    /// Catcher stat: The catcher was able to throw out a base runner attempting to steal
    /// </summary>
    public int CaughtStealing { get; private set; }

    /// <summary>
    /// Catcher stat: The number of times a base runner successfully stole a base against the catcher
    /// </summary>
    public int StolenBases { get; private set; }

    /// <summary>
    /// Catcher stat: The number of times the catcher dropped the ball and a runner is able to advance
    /// </summary>
    public int PassedBalls { get; private set; }

    /// <summary>
    /// Catcher stat: The number of times a catcher interfered with the batter's plate appearance
    /// </summary>
    public int CatchersInterference { get; private set; }

    /// <summary>
    /// Catcher stat: The number of wild pitches the catcher saw from the pitcher
    /// </summary>
    public int WildPitches { get; private set; }

    /// <summary>
    /// Catcher stat: The number of pick offs made by the pitcher while this catcher was behind the plate
    /// </summary>
    public int PickOffs { get; private set; }

    /// <summary>
    /// Catcher stat: Stolen base percentage = SB / (SB + CS)
    /// </summary>
    public float StolenBasePercentage => (float)(StolenBases) / (StolenBases + CaughtStealing);

    /// <summary>
    /// Catcher stat: Catcher's ERA = 9 * (ER / IP)
    /// </summary>
    public float CatcherEarnedRunAverage { get; private set; }

    public PlayerFieldingStatsByGame(Guid id) : base(id)
    {
    }
}