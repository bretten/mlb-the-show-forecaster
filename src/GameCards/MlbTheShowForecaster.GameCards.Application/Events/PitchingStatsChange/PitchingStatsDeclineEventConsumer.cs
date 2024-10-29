using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PitchingStatsChange;

/// <summary>
/// Consumes a <see cref="PitchingStatsDeclineEvent"/>
///
/// <para>Applies a <see cref="StatsForecastImpact"/> to the specified <see cref="PlayerCardForecast"/></para>
/// </summary>
public sealed class PitchingStatsDeclineEventConsumer : BaseForecastImpactEventConsumer<PitchingStatsDeclineEvent>
{
    /// <inheritdoc />
    public PitchingStatsDeclineEventConsumer(ICommandSender commandSender, ICalendar calendar,
        ForecastImpactDuration duration) : base(commandSender, calendar, duration)
    {
    }

    /// <inheritdoc />
    protected override ForecastImpact CreateImpact(IForecastImpactEvent ev)
    {
        var e = (PitchingStatsDeclineEvent)ev;
        return new PitchingStatsForecastImpact(e.Comparison.ReferenceValue, e.Comparison.NewValue, ev.Date,
            ev.Date.AddDays(Duration.PitchingStatsChange));
    }
}