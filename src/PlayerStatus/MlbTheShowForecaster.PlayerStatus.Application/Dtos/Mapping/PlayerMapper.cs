using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos.Mapping;

/// <summary>
/// Maps the Application-level <see cref="RosterEntry"/> to the Domain's <see cref="Player"/>
/// </summary>
public sealed class PlayerMapper : IPlayerMapper
{
    /// <summary>
    /// Maps <see cref="RosterEntry"/> to <see cref="Player"/>
    /// </summary>
    /// <param name="rosterEntry">The roster entry to map</param>
    /// <returns><see cref="Player"/></returns>
    public Player Map(RosterEntry rosterEntry)
    {
        return Player.Create(rosterEntry.MlbId,
            firstName: rosterEntry.FirstName,
            lastName: rosterEntry.LastName,
            birthdate: rosterEntry.Birthdate,
            position: rosterEntry.Position,
            mlbDebutDate: rosterEntry.MlbDebutDate,
            batSide: rosterEntry.BatSide,
            throwArm: rosterEntry.ThrowArm,
            team: Team.Create((TeamInfo)rosterEntry.CurrentTeamMlbId.Value),
            active: rosterEntry.Active
        );
    }
}