using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos.Mapping;

/// <summary>
/// Defines a mapper that maps the Application-level <see cref="RosterEntry"/> to the Domain's <see cref="Player"/>
/// </summary>
public interface IPlayerMapper
{
    /// <summary>
    /// Should map <see cref="RosterEntry"/> to <see cref="Player"/>
    /// </summary>
    /// <param name="rosterEntry">The roster entry to map</param>
    /// <returns><see cref="Player"/></returns>
    Player Map(RosterEntry rosterEntry);
}