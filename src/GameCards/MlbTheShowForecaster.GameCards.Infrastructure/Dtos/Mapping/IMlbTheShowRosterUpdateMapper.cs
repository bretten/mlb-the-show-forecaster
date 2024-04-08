using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.RosterUpdates;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.RosterUpdates;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;

/// <summary>
/// Defines a mapper that maps roster updates from MLB The Show to application-level DTOs
/// </summary>
public interface IMlbTheShowRosterUpdateMapper
{
    /// <summary>
    /// Should map <see cref="RosterUpdateDto"/> and <see cref="GetRosterUpdateResponse"/> to <see cref="RosterUpdate"/>
    /// </summary>
    /// <param name="rosterUpdate">The roster update information</param>
    /// <param name="rosterUpdateChanges">The player changes in the roster update</param>
    /// <returns><see cref="RosterUpdate"/></returns>
    RosterUpdate Map(RosterUpdateDto rosterUpdate, GetRosterUpdateResponse rosterUpdateChanges);

    /// <summary>
    /// Should map <see cref="PlayerAttributeChangeDto"/> to <see cref="PlayerRatingChange"/>
    /// </summary>
    /// <param name="date">The date of the change</param>
    /// <param name="playerAttributeChange">The attribute changes for the player</param>
    /// <returns><see cref="PlayerRatingChange"/></returns>
    PlayerRatingChange Map(DateOnly date, PlayerAttributeChangeDto playerAttributeChange);

    /// <summary>
    /// Should map a collection of <see cref="AttributeChangeDto"/> to <see cref="MlbPlayerAttributeChanges"/>
    /// </summary>
    /// <param name="attributeChanges">The individual attribute changes</param>
    /// <returns><see cref="MlbPlayerAttributeChanges"/></returns>
    MlbPlayerAttributeChanges Map(IEnumerable<AttributeChangeDto> attributeChanges);

    /// <summary>
    /// Should map <see cref="PlayerPositionChangeDto"/> to <see cref="PlayerPositionChange"/>
    /// </summary>
    /// <param name="positionChange">The position change</param>
    /// <returns><see cref="PlayerPositionChange"/></returns>
    PlayerPositionChange Map(PlayerPositionChangeDto positionChange);

    /// <summary>
    /// Should map <see cref="NewlyAddedPlayerDto"/> to <see cref="PlayerAddition"/>
    /// </summary>
    /// <param name="newPlayer">The new player</param>
    /// <returns><see cref="PlayerAddition"/></returns>
    PlayerAddition Map(NewlyAddedPlayerDto newPlayer);
}