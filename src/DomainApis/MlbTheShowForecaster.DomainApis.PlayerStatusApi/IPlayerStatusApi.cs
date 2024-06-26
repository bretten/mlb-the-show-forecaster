using com.brettnamba.MlbTheShowForecaster.DomainApis.PlayerStatusApi.Responses;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.DomainApis.PlayerStatusApi;

/// <summary>
/// Represents a client for the API of the PlayerStatus domain
/// </summary>
public interface IPlayerStatusApi
{
    /// <summary>
    /// Finds a player by their name and team
    /// </summary>
    /// <param name="name">The player's name</param>
    /// <param name="team">The player's team</param>
    /// <returns><see cref="PlayerResponse"/> or 404 if no matching player is found</returns>
    [Get("/players")]
    Task<ApiResponse<PlayerResponse>> FindPlayer([Query] string name, [Query] string team);
}