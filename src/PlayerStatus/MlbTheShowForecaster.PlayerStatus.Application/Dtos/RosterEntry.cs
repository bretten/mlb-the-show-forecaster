using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos;

/// <summary>
/// Represents a <see cref="Player"/> and their information on the MLB roster
/// </summary>
/// <param name="MlbId">The MLB issued ID</param>
/// <param name="FirstName">The player's first name</param>
/// <param name="LastName">The player's last name</param>
/// <param name="Birthdate">The player's birthdate</param>
/// <param name="Position">The player's position</param>
/// <param name="MlbDebutDate">The player's MLB debut date</param>
/// <param name="BatSide">The side the player bats on</param>
/// <param name="ThrowArm">The arm the player throws with</param>
/// <param name="CurrentTeamMlbId">The MLB issued ID of the player's current team</param>
/// <param name="Active">True if the player is active, otherwise false</param>
public readonly record struct RosterEntry(MlbId MlbId, PersonName FirstName, PersonName LastName, DateTime Birthdate,
    Position Position, DateTime MlbDebutDate, BatSide BatSide, ThrowArm ThrowArm, MlbId CurrentTeamMlbId, bool Active);