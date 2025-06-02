using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.RosterEntries;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Mapping.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Mapping;

/// <summary>
/// Should map MLB API data to application level DTOs
/// </summary>
public interface IMlbApiPlayerMapper
{
    /// <summary>
    /// Should map a player's roster entry from the MLB API to <see cref="RosterEntry"/>
    /// </summary>
    /// <param name="dto">A player's MLB API roster entry</param>
    /// <param name="rosterStatuses">The player's roster status history</param>
    /// <returns>The application level <see cref="RosterEntry"/></returns>
    RosterEntry Map(PlayerDto dto, IEnumerable<RosterEntryDto> rosterStatuses);

    /// <summary>
    /// Should map a position abbreviation from the MLB API to <see cref="Position"/>
    /// </summary>
    /// <param name="positionAbbreviation">The position abbreviation from the MLB API</param>
    /// <returns><see cref="Position"/></returns>
    /// <exception cref="InvalidMlbApiPositionException">Thrown if the position string value is invalid</exception>
    public Position MapPosition(string positionAbbreviation);

    /// <summary>
    /// Should map a bat side code from the MLB API to <see cref="BatSide"/>
    /// </summary>
    /// <param name="batSideCode">The bat side code from the MLB API</param>
    /// <returns><see cref="BatSide"/></returns>
    /// <exception cref="InvalidMlbApiBatSideCodeException">Thrown if the bat side code is invalid</exception>
    public BatSide MapBatSide(string batSideCode);

    /// <summary>
    /// Should map a throw arm code from the MLB API to <see cref="ThrowArm"/>
    /// </summary>
    /// <param name="throwArmCode">The throw arm code from the MLB API</param>
    /// <returns><see cref="ThrowArm"/></returns>
    /// <exception cref="InvalidMlbApiThrowArmCodeException">Thrown if the throw arm code is invalid</exception>
    public ThrowArm MapThrowArm(string throwArmCode);

    /// <summary>
    /// Should determine the player's active status by parsing through their roster status history
    /// </summary>
    /// <param name="statuses">The player's roster status history</param>
    /// <returns>True if the player is currently active, otherwise false</returns>
    public bool MapActiveStatus(IEnumerable<RosterEntryDto> statuses);
}