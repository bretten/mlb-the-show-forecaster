using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.Api.Responses;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;
using Microsoft.AspNetCore.Mvc;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.Api.Controllers;

/// <summary>
/// Exposes player related endpoints
/// </summary>
[ApiController]
public class PlayerController : Controller
{
    /// <summary>
    /// Service that searches for a player by certain criteria
    /// </summary>
    private readonly IPlayerSearchService _playerSearchService;

    /// <summary>
    /// Service that provides team information
    /// </summary>
    private readonly ITeamProvider _teamProvider;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerSearchService">Service that searches for a player by certain criteria</param>
    /// <param name="teamProvider">Service that provides team information</param>
    public PlayerController(IPlayerSearchService playerSearchService, ITeamProvider teamProvider)
    {
        _playerSearchService = playerSearchService;
        _teamProvider = teamProvider;
    }

    /// <summary>
    /// Finds a player by their name and team
    /// </summary>
    /// <param name="name">The player's name</param>
    /// <param name="team">the player's team</param>
    /// <returns>The player, or returns a 404 status code if not found</returns>
    [HttpGet("players/name/{name}/{team}")]
    public async Task<ActionResult<PlayerResponse>> FindPlayer(string name, string team)
    {
        var correspondingTeam = _teamProvider.GetBy(team);
        if (correspondingTeam == null)
        {
            return new NotFoundResult();
        }

        var player = await _playerSearchService.FindPlayer(name, correspondingTeam);
        if (player == null)
        {
            return new NotFoundResult();
        }

        return new ActionResult<PlayerResponse>(PlayerResponse.From(player));
    }
}