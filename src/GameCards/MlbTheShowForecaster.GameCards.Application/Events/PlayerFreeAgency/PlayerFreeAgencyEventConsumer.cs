﻿using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PlayerFreeAgency;

/// <summary>
/// Consumes a <see cref="PlayerFreeAgencyEvent"/>
///
/// <para>Applies a <see cref="PlayerFreeAgencyForecastImpact"/> to the specified <see cref="PlayerCardForecast"/></para>
/// </summary>
public sealed class PlayerFreeAgencyEventConsumer : BaseForecastImpactEventConsumer<PlayerFreeAgencyEvent>
{
    /// <inheritdoc />
    public PlayerFreeAgencyEventConsumer(ICommandSender commandSender, ITrendReporter trendReporter,
        ForecastImpactDuration duration) : base(commandSender, trendReporter, duration)
    {
    }

    /// <inheritdoc />
    protected override ForecastImpact CreateImpact(IForecastImpactEvent ev)
    {
        return new PlayerFreeAgencyForecastImpact(ev.Date, ev.Date.AddDays(Duration.PlayerFreeAgency));
    }
}