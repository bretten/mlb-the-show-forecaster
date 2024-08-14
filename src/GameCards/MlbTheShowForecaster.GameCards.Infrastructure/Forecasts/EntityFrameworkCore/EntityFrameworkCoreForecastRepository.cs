using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Forecasts.EntityFrameworkCore;

/// <summary>
/// EF core repository for <see cref="PlayerCardForecast"/>
/// </summary>
public sealed class EntityFrameworkCoreForecastRepository : IForecastRepository
{
    /// <summary>
    /// The DB context for <see cref="PlayerCardForecast"/>
    /// </summary>
    private readonly ForecastsDbContext _dbContext;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbContext">The DB context for <see cref="PlayerCardForecast"/></param>
    public EntityFrameworkCoreForecastRepository(ForecastsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Adds a <see cref="PlayerCardForecast"/> to the repository
    /// </summary>
    /// <param name="playerCardForecast">The <see cref="PlayerCardForecast"/> to add</param>
    /// <returns>The completed task</returns>
    public async Task Add(PlayerCardForecast playerCardForecast)
    {
        await _dbContext.AddAsync(playerCardForecast);
    }

    /// <summary>
    /// Updates a <see cref="PlayerCardForecast"/> in the repository
    /// </summary>
    /// <param name="playerCardForecast">The <see cref="PlayerCardForecast"/> to update</param>
    /// <returns>The completed task</returns>
    public Task Update(PlayerCardForecast playerCardForecast)
    {
        _dbContext.Update(playerCardForecast);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Returns a <see cref="PlayerCardForecast"/> for the specified <see cref="SeasonYear"/> and <see cref="CardExternalId"/>
    /// </summary>
    /// <param name="seasonYear">The <see cref="SeasonYear"/> of the <see cref="PlayerCardForecast"/></param>
    /// <param name="cardExternalId">The <see cref="CardExternalId"/> of the <see cref="PlayerCardForecast"/></param>
    /// <returns>The corresponding <see cref="PlayerCardForecast"/></returns>
    public async Task<PlayerCardForecast?> GetBy(SeasonYear seasonYear, CardExternalId cardExternalId)
    {
        return await _dbContext.PlayerCardForecastsWithImpacts()
            .FirstOrDefaultAsync(x => x.CardExternalId == cardExternalId);
    }

    /// <summary>
    /// Returns a <see cref="PlayerCardForecast"/> for the specified <see cref="SeasonYear"/> and <see cref="MlbId"/>
    /// </summary>
    /// <param name="seasonYear">The <see cref="SeasonYear"/> of the <see cref="PlayerCardForecast"/></param>
    /// <param name="mlbId">The <see cref="MlbId"/> of the <see cref="PlayerCardForecast"/></param>
    /// <returns>The corresponding <see cref="PlayerCardForecast"/></returns>
    public async Task<PlayerCardForecast?> GetBy(SeasonYear seasonYear, MlbId mlbId)
    {
        return await _dbContext.PlayerCardForecastsWithImpacts()
            .FirstOrDefaultAsync(x => x.MlbId == mlbId);
    }

    /// <summary>
    /// Gets any <see cref="PlayerCardForecast"/>sethat are predicted to have demand changes on the specified date
    /// due to <see cref="ForecastImpact"/>s
    /// </summary>
    /// <param name="date">Date (inclusive) when any <see cref="PlayerCardForecast"/>s still remain influenced by <see cref="ForecastImpact"/>s</param>
    /// <returns>Impacted <see cref="PlayerCardForecast"/></returns>
    public async Task<IEnumerable<PlayerCardForecast>> GetImpactedForecasts(DateOnly date)
    {
        return await _dbContext.PlayerCardForecasts
            .Where(x => x.ForecastImpactsChronologically.Any(i => i.EndDate >= date))
            .ToListAsync();
    }
}