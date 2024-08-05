using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Repositories;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetImpactedPlayerCardForecasts;

/// <summary>
/// Handles a <see cref="GetImpactedPlayerCardForecastsQuery"/>
///
/// <para>Gets all <see cref="PlayerCardForecast"/>s that remain impacted on the specified date</para>
/// </summary>
internal sealed class
    GetImpactedPlayerCardForecastsQueryHandler : IQueryHandler<GetImpactedPlayerCardForecastsQuery,
    IEnumerable<PlayerCardForecast>>
{
    /// <summary>
    /// The <see cref="PlayerCardForecast"/> repository
    /// </summary>
    private readonly IForecastRepository _forecastRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="forecastRepository">The <see cref="PlayerCardForecast"/> repository</param>
    public GetImpactedPlayerCardForecastsQueryHandler(IForecastRepository forecastRepository)
    {
        _forecastRepository = forecastRepository;
    }

    /// <summary>
    /// Gets all <see cref="PlayerCardForecast"/>s that remain impacted on the specified date
    /// </summary>
    /// <param name="query">The query</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The <see cref="PlayerCardForecast"/>s that remain impacted on the specified date</returns>
    public async Task<IEnumerable<PlayerCardForecast>?> Handle(GetImpactedPlayerCardForecastsQuery query,
        CancellationToken cancellationToken)
    {
        return await _forecastRepository.GetImpactedForecasts(query.Date);
    }
}