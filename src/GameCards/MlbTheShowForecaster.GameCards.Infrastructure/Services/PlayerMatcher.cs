using System.ComponentModel;
using System.Net;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.DomainApis.PlayerStatusApi;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services;

/// <summary>
/// Uses <see cref="IPlayerStatusApi"/> to match players in the MLB
/// </summary>
public sealed class PlayerMatcher : IPlayerMatcher
{
    /// <summary>
    /// Client for the PlayerStatus API
    /// </summary>
    private readonly IPlayerStatusApi _playerStatusApi;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerStatusApi">Client for the PlayerStatus API</param>
    public PlayerMatcher(IPlayerStatusApi playerStatusApi)
    {
        _playerStatusApi = playerStatusApi;
    }

    /// <inheritdoc />
    public async Task<Player?> GetPlayerByName(CardName name, TeamShortName teamShortName)
    {
        var response = await _playerStatusApi.FindPlayer(name.Value, teamShortName.Value);
        if (response.IsSuccessStatusCode)
        {
            return new Player(MlbId.Create(response.Content.MlbId),
                $"{response.Content.FirstName} {response.Content.LastName}",
                (Position)TypeDescriptor.GetConverter(typeof(Position)).ConvertFrom(response.Content.Position)!,
                teamShortName);
        }

        // Valid case, the server returns 404 when the query does not match a player
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        // Unexpected case, something may be wrong on the server side
        throw new PlayerCouldNotBeMatchedException(
            $"{nameof(PlayerMatcher)} could not be matched: {name.Value}, {teamShortName.Value} -- {response.StatusCode} -- {response.ReasonPhrase}");
    }
}