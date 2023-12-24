using com.brettnamba.MlbTheShowForecaster.Core.SeedWork;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Players.Entities;

/// <summary>
/// Represents a MLB Player
/// </summary>
public sealed class Player : AggregateRoot
{
    /// <summary>
    /// The MLB ID of the Player
    /// </summary>
    public MlbId MlbId { get; private set; }

    /// <summary>
    /// Player's first name
    /// </summary>
    public PersonName FirstName { get; private set; }

    /// <summary>
    /// Player's last name
    /// </summary>
    public PersonName LastName { get; private set; }

    /// <summary>
    /// Player's birthdate
    /// </summary>
    public DateTime Birthdate { get; private set; }

    /// <summary>
    /// Player's primary position
    /// </summary>
    public Position Position { get; private set; }

    /// <summary>
    /// The date the Player debuted in the MLB
    /// </summary>
    public DateTime MlbDebutDate { get; private set; }

    /// <summary>
    /// The side the Player bats on
    /// </summary>
    public BatSide BatSide { get; private set; }

    /// <summary>
    /// The arm the Player throws with
    /// </summary>
    public ThrowArm ThrowArm { get; private set; }

    /// <summary>
    /// The Player's Team
    /// </summary>
    public Team? Team { get; private set; }

    /// <summary>
    /// True if the Player is active in the MLB, otherwise false
    /// </summary>
    public bool Active { get; private set; }

    /// <summary>
    /// True if the Player is a free agent, otherwise false
    /// </summary>
    /// <returns></returns>
    public bool IsFreeAgent() => Team == null;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mlbId">MLB ID</param>
    /// <param name="firstName">First name</param>
    /// <param name="lastName">Last name</param>
    /// <param name="birthdate">Birthdate</param>
    /// <param name="position">Primary position</param>
    /// <param name="mlbDebutDate">MLB debut date</param>
    /// <param name="batSide">The side the Player bats on</param>
    /// <param name="throwArm">The arm the Player throws with</param>
    /// <param name="team">The Player's Team</param>
    /// <param name="active">True if the Player is active in the MLB, otherwise false</param>
    private Player(MlbId mlbId, PersonName firstName, PersonName lastName, DateTime birthdate,
        Position position, DateTime mlbDebutDate, BatSide batSide, ThrowArm throwArm, Team? team,
        bool active) : base(Guid.NewGuid())
    {
        MlbId = mlbId;
        FirstName = firstName;
        LastName = lastName;
        Birthdate = birthdate;
        Position = position;
        MlbDebutDate = mlbDebutDate;
        BatSide = batSide;
        ThrowArm = throwArm;
        Team = team;
        Active = active;
    }

    /// <summary>
    /// Signs the Player with the specified Team
    /// </summary>
    /// <param name="team">The Team to sign with</param>
    public void SignContractWithTeam(Team team)
    {
        if (!IsFreeAgent())
        {
            EndCurrentTeamContract();
        }

        Team = team;
    }

    /// <summary>
    /// Enters the Player into free agency
    /// </summary>
    public void EnterFreeAgency()
    {
        EndCurrentTeamContract();
    }

    /// <summary>
    /// Ends the Player's current Team contract
    /// </summary>
    private void EndCurrentTeamContract()
    {
        Team = null;
    }

    /// <summary>
    /// Creates a <see cref="Player"/>
    /// </summary>
    /// <param name="mlbId">MLB ID</param>
    /// <param name="firstName">First name</param>
    /// <param name="lastName">Last name</param>
    /// <param name="birthdate">Birthdate</param>
    /// <param name="position">Primary position</param>
    /// <param name="mlbDebutDate">MLB debut date</param>
    /// <param name="batSide">The side the Player bats on</param>
    /// <param name="throwArm">The arm the Player throws with</param>
    /// <param name="team">The Player's Team</param>
    /// <param name="active">True if the Player is active in the MLB, otherwise false</param>
    /// <returns>The created <see cref="Player"/></returns>
    public static Player Create(MlbId mlbId, PersonName firstName, PersonName lastName, DateTime birthdate,
        Position position, DateTime mlbDebutDate, BatSide batSide, ThrowArm throwArm, Team? team,
        bool active)
    {
        return new Player(mlbId, firstName, lastName, birthdate, position, mlbDebutDate, batSide, throwArm, team,
            active);
    }
}