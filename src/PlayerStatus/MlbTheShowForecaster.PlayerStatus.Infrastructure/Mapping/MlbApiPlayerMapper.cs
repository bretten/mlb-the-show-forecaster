using System.ComponentModel;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.RosterEntries;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Mapping.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Mapping;

/// <summary>
/// Maps MLB API data to application level DTOs
/// </summary>
public sealed class MlbApiPlayerMapper : IMlbApiPlayerMapper
{
    /// <summary>
    /// Checks if the roster status team is a MLB team or MiLB team
    /// </summary>
    private readonly ITeamProvider _teamProvider;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="teamProvider">Checks if the roster status team is a MLB team or MiLB team</param>
    public MlbApiPlayerMapper(ITeamProvider teamProvider)
    {
        _teamProvider = teamProvider;
    }

    /// <summary>
    /// Maps a player's roster entry from the MLB API to <see cref="RosterEntry"/>
    /// </summary>
    /// <param name="dto">A player's MLB API roster entry</param>
    /// <param name="rosterStatuses">The player's roster status history</param>
    /// <returns>The application level <see cref="RosterEntry"/></returns>
    public RosterEntry Map(PlayerDto dto, IEnumerable<RosterEntryDto> rosterStatuses)
    {
        return new RosterEntry(MlbId: MlbId.Create(dto.Id),
            FirstName: PersonName.Create(dto.FirstName),
            LastName: PersonName.Create(dto.LastName),
            Birthdate: dto.Birthdate,
            Position: MapPosition(dto.Position.Abbreviation),
            MlbDebutDate: dto.MlbDebutDate,
            BatSide: MapBatSide(dto.BatSide.Code),
            ThrowArm: MapThrowArm(dto.ThrowArm.Code),
            CurrentTeamMlbId: MlbId.Create(dto.CurrentTeam.Id),
            Active: MapActiveStatus(rosterStatuses)
        );
    }

    /// <summary>
    /// Maps a position abbreviation from the MLB API to <see cref="Position"/>
    /// </summary>
    /// <param name="positionAbbreviation">The position abbreviation from the MLB API</param>
    /// <returns><see cref="Position"/></returns>
    /// <exception cref="InvalidMlbApiPositionException">Thrown if the position string value is invalid</exception>
    public Position MapPosition(string positionAbbreviation)
    {
        switch (positionAbbreviation)
        {
            case "P":
            case "C":
            case "1B":
            case "2B":
            case "3B":
            case "SS":
            case "LF":
            case "CF":
            case "RF":
            case "OF":
            case "DH":
            case "PH":
            case "PR":
            case "TWP":
            case "IF":
                return (Position)TypeDescriptor.GetConverter(typeof(Position)).ConvertFrom(positionAbbreviation)!;
            default:
                throw new InvalidMlbApiPositionException(
                    $"Could not map position from the MLB API: {positionAbbreviation}");
        }
    }

    /// <summary>
    /// Maps a bat side code from the MLB API to <see cref="BatSide"/>
    /// </summary>
    /// <param name="batSideCode">The bat side code from the MLB API</param>
    /// <returns><see cref="BatSide"/></returns>
    /// <exception cref="InvalidMlbApiBatSideCodeException">Thrown if the bat side code is invalid</exception>
    public BatSide MapBatSide(string batSideCode)
    {
        switch (batSideCode)
        {
            case "R":
            case "L":
            case "S":
                return (BatSide)TypeDescriptor.GetConverter(typeof(BatSide)).ConvertFrom(batSideCode)!;
            default:
                throw new InvalidMlbApiBatSideCodeException(
                    $"Could not map bat side code from the MLB API: {batSideCode}");
        }
    }

    /// <summary>
    /// Maps a throw arm code from the MLB API to <see cref="ThrowArm"/>
    /// </summary>
    /// <param name="throwArmCode">The throw arm code from the MLB API</param>
    /// <returns><see cref="ThrowArm"/></returns>
    /// <exception cref="InvalidMlbApiThrowArmCodeException">Thrown if the throw arm code is invalid</exception>
    public ThrowArm MapThrowArm(string throwArmCode)
    {
        switch (throwArmCode)
        {
            case "R":
            case "L":
            case "S":
                return (ThrowArm)TypeDescriptor.GetConverter(typeof(ThrowArm)).ConvertFrom(throwArmCode)!;
            default:
                throw new InvalidMlbApiThrowArmCodeException(
                    $"Could not map throw arm code from the MLB API: {throwArmCode}");
        }
    }

    /// <summary>
    /// Determines the player's active status by parsing through their roster status history
    /// </summary>
    /// <param name="statuses">The player's roster status history</param>
    /// <returns>True if the player is currently active, otherwise false</returns>
    public bool MapActiveStatus(IEnumerable<RosterEntryDto> statuses)
    {
        var list = statuses.ToList();
        if (list.Count == 0)
        {
            return false;
        }

        var mostRecent = list.OrderByDescending(x => x.StatusDate).First();

        // Check if the team is MLB or MiLB
        try
        {
            _teamProvider.GetBy(MlbId.Create(mostRecent.Team.Id));
        }
        catch (UnknownTeamMlbIdException)
        {
            // The player is currently on a MiLB team
            return false;
        }

        // If their most recent state is active, return true
        return mostRecent.Status.Code == "A"
               && mostRecent.IsActive
               && mostRecent.IsActiveFortyMan;
    }
}