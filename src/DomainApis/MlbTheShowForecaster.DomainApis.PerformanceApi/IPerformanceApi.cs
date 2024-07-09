using com.brettnamba.MlbTheShowForecaster.DomainApis.PerformanceApi.Responses;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.DomainApis.PerformanceApi;

/// <summary>
/// Represents a client for the API of the Performance domain
/// </summary>
public interface IPerformanceApi
{
    /// <summary>
    /// Request to get a player's season performance
    /// </summary>
    /// <param name="season">The season</param>
    /// <param name="playerMlbId">The player's MLB ID</param>
    /// <param name="start">The start date (yyyy-MM-dd) of the performance period</param>
    /// <param name="end">The end date (yyyy-MM-dd) of the performance period</param>
    /// <returns><see cref="PlayerSeasonPerformanceResponse"/></returns>
    [Get("/performance")]
    Task<ApiResponse<PlayerSeasonPerformanceResponse>> GetPlayerSeasonPerformance([Query] ushort season,
        [Query] int playerMlbId, [Query] string start, [Query] string end);
}