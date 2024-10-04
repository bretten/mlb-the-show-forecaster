﻿using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.FieldingStatsChange;

/// <summary>
/// Consumes a <see cref="FieldingStatsImprovementEvent"/>
///
/// <para>Applies a <see cref="StatsForecastImpact"/> to the specified <see cref="PlayerCardForecast"/></para>
/// </summary>
public sealed class
    FieldingStatsImprovementEventConsumer : BaseForecastImpactEventConsumer<FieldingStatsImprovementEvent>
{
    /// <inheritdoc />
    public FieldingStatsImprovementEventConsumer(ICommandSender commandSender, ICalendar calendar,
        ForecastImpactDuration duration) : base(commandSender, calendar, duration)
    {
    }

    /// <inheritdoc />
    protected override ForecastImpact CreateImpact(IForecastImpactEvent ev)
    {
        var e = (FieldingStatsImprovementEvent)ev;
        return new FieldingStatsForecastImpact(e.Comparison.ReferenceValue, e.Comparison.NewValue,
            Calendar.Today(), Calendar.Today().AddDays(Duration.FieldingStatsChange));
    }
}