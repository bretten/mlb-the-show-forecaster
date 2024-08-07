﻿using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Repositories;

/// <summary>
/// Defines a repository for <see cref="PlayerCardForecast"/>
/// </summary>
public interface IForecastRepository
{
    /// <summary>
    /// Should add a <see cref="PlayerCardForecast"/>
    /// </summary>
    /// <param name="playerCardForecast">The <see cref="PlayerCardForecast"/> to add</param>
    /// <returns>The completed task</returns>
    Task Add(PlayerCardForecast playerCardForecast);

    /// <summary>
    /// Should update a <see cref="PlayerCardForecast"/>
    /// </summary>
    /// <param name="playerCardForecast">The <see cref="PlayerCardForecast"/> to update</param>
    /// <returns>The completed task</returns>
    Task Update(PlayerCardForecast playerCardForecast);

    /// <summary>
    /// Should return a <see cref="PlayerCardForecast"/> for the specified <see cref="SeasonYear"/> and <see cref="CardExternalId"/>
    /// </summary>
    /// <param name="seasonYear">The <see cref="SeasonYear"/> of the <see cref="PlayerCardForecast"/></param>
    /// <param name="cardExternalId">The <see cref="CardExternalId"/> of the <see cref="PlayerCardForecast"/></param>
    /// <returns>The corresponding <see cref="PlayerCardForecast"/></returns>
    Task<PlayerCardForecast?> GetBy(SeasonYear seasonYear, CardExternalId cardExternalId);

    /// <summary>
    /// Should return a <see cref="PlayerCardForecast"/> for the specified <see cref="SeasonYear"/> and <see cref="MlbId"/>
    /// </summary>
    /// <param name="seasonYear">The <see cref="SeasonYear"/> of the <see cref="PlayerCardForecast"/></param>
    /// <param name="mlbId">The <see cref="MlbId"/> of the <see cref="PlayerCardForecast"/></param>
    /// <returns>The corresponding <see cref="PlayerCardForecast"/></returns>
    Task<PlayerCardForecast?> GetBy(SeasonYear seasonYear, MlbId mlbId);

    /// <summary>
    /// Gets any <see cref="PlayerCardForecast"/>s that are predicted to have demand changes on the specified date
    /// due to <see cref="ForecastImpact"/>s
    /// </summary>
    /// <param name="date">Date (inclusive) when any <see cref="PlayerCardForecast"/>s still remain influenced by <see cref="ForecastImpact"/>s</param>
    /// <returns>Impacted <see cref="PlayerCardForecast"/></returns>
    Task<IEnumerable<PlayerCardForecast>> GetImpactedForecasts(DateOnly date);
}