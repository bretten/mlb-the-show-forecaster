using com.brettnamba.MlbTheShowForecaster.Common.Application.Pagination;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;

/// <summary>
/// Defines a service that reports on a player's stat trends
/// </summary>
public interface ITrendReporter
{
    /// <summary>
    /// Updates a player's trend report by season and card external ID
    /// </summary>
    /// <param name="year">The season</param>
    /// <param name="cardExternalId">The card external ID</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The completed task</returns>
    Task UpdateTrendReport(SeasonYear year, CardExternalId cardExternalId, CancellationToken cancellationToken);

    /// <summary>
    /// Updates a player's trend report by season and MLB ID
    /// </summary>
    /// <param name="year">The season</param>
    /// <param name="mlbId">The player's MLB ID</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The completed task</returns>
    Task UpdateTrendReport(SeasonYear year, MlbId mlbId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a collection of <see cref="TrendReport"/>s based on the specified criteria
    /// </summary>
    /// <param name="year">The season</param>
    /// <param name="page">The page</param>
    /// <param name="pageSize">The page size</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort direction</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns><see cref="TrendReport"/> collection</returns>
    Task<PaginationResult<TrendReport>> GetTrendReports(SeasonYear year, int page, int pageSize, string? sortField,
        SortOrder? sortOrder, CancellationToken cancellationToken);

    /// <summary>
    /// Sort order
    /// </summary>
    public enum SortOrder
    {
        Asc,
        Desc
    }
}