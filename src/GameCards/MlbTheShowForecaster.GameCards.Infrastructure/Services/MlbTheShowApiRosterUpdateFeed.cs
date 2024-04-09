using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.RosterUpdates;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;
using Microsoft.Extensions.Caching.Memory;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services;

/// <summary>
/// Service that tracks which roster updates are available from MLB The Show and which have yet to be applied
/// to the domain
/// </summary>
public sealed class MlbTheShowApiRosterUpdateFeed : IRosterUpdateFeed
{
    /// <summary>
    /// Client factory for <see cref="IMlbTheShowApi"/>. Provides a client based on a season year
    /// </summary>
    private readonly IMlbTheShowApiFactory _mlbTheShowApiFactory;

    /// <summary>
    /// Maps MLB The Show DTOs to application-layer DTOs
    /// </summary>
    private readonly IMlbTheShowRosterUpdateMapper _rosterUpdateMapper;

    /// <summary>
    /// Ephemeral storage for keeping track of which roster updates have been applied. If storage is lost,
    /// roster updates will not be duplicated because each player card keeps track of all changes
    /// </summary>
    private readonly IMemoryCache _memoryCache;

    /// <summary>
    /// Memory cache options
    /// </summary>
    private readonly MemoryCacheEntryOptions _entryOptions = new() { Priority = CacheItemPriority.NeverRemove };

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mlbTheShowApiFactory">Client factory for <see cref="IMlbTheShowApi"/>. Provides a client based on a season year</param>
    /// <param name="rosterUpdateMapper">Maps MLB The Show DTOs to application-layer DTOs</param>
    /// <param name="memoryCache">Ephemeral storage for keeping track of which roster updates have been applied</param>
    public MlbTheShowApiRosterUpdateFeed(IMlbTheShowApiFactory mlbTheShowApiFactory,
        IMlbTheShowRosterUpdateMapper rosterUpdateMapper, IMemoryCache memoryCache)
    {
        _mlbTheShowApiFactory = mlbTheShowApiFactory;
        _rosterUpdateMapper = rosterUpdateMapper;
        _memoryCache = memoryCache;
    }

    /// <summary>
    /// Retrieves roster updates from MLB The Show and returns those that have not been applied to the domain yet
    /// </summary>
    /// <param name="seasonYear">The season to check for roster updates</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>New roster updates</returns>
    public async Task<IEnumerable<RosterUpdate>> GetNewRosterUpdates(SeasonYear seasonYear,
        CancellationToken cancellationToken = default)
    {
        // Get the client for the specified year
        var mlbTheShowApiForSeason = _mlbTheShowApiFactory.GetClient((Year)seasonYear.Value);

        // Get the external roster updates from MLB The Show
        var rosterUpdatesResponse = await mlbTheShowApiForSeason.GetRosterUpdates();

        // Will hold the application-level roster updates
        var rosterUpdates = new List<RosterUpdate>();

        // Map each external roster update to the application-level
        foreach (var rosterUpdateId in rosterUpdatesResponse.RosterUpdates)
        {
            if (IsRosterUpdateCompleted(rosterUpdateId.Date))
            {
                continue;
            }

            // Get the external roster update data from MLB The Show
            var externalRosterUpdate =
                await mlbTheShowApiForSeason.GetRosterUpdate(new GetRosterUpdateRequest(rosterUpdateId.Id));

            // Map the roster update data to the application-level DTO and add it to the collection of uncompleted roster updates
            rosterUpdates.Add(_rosterUpdateMapper.Map(rosterUpdateId, externalRosterUpdate));
        }

        return rosterUpdates.OrderBy(x => x.Date);
    }

    /// <summary>
    /// Marks a roster update as complete so it will no longer be considered a new roster update
    /// </summary>
    /// <param name="rosterUpdate">The roster update to mark as complete</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The completed task</returns>
    public Task CompleteRosterUpdate(RosterUpdate rosterUpdate, CancellationToken cancellationToken = default)
    {
        _memoryCache.Set(rosterUpdate.Date, true, _entryOptions);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Checks if a roster update has been completed for the specified date
    /// </summary>
    /// <param name="dateOfRosterUpdate">The date of the roster update to check</param>
    /// <returns>True if the roster update is complete, otherwise false</returns>
    private bool IsRosterUpdateCompleted(DateOnly dateOfRosterUpdate)
    {
        var keyExists = _memoryCache.TryGetValue(dateOfRosterUpdate, out var completed);
        return keyExists && completed is true;
    }
}