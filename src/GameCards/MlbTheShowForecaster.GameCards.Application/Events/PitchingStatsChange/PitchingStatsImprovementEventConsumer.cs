using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PitchingStatsChange;

/// <summary>
/// Consumes a <see cref="PitchingStatsImprovementEvent"/>
///
/// <para>Applies a <see cref="StatsForecastImpact"/> to the specified <see cref="PlayerCardForecast"/></para>
/// </summary>
public sealed class
    PitchingStatsImprovementEventConsumer : BaseForecastImpactEventConsumer<PitchingStatsImprovementEvent>
{
    /// <inheritdoc />
    public PitchingStatsImprovementEventConsumer(ICommandSender commandSender, ICalendar calendar,
        ForecastImpactDuration duration, IForecastReportPublisher publisher) : base(commandSender, calendar, duration,
        publisher)
    {
    }

    /// <inheritdoc />
    protected override ForecastImpact CreateImpact(IForecastImpactEvent ev)
    {
        var e = (PitchingStatsImprovementEvent)ev;
        return new PitchingStatsForecastImpact(e.Comparison.ReferenceValue, e.Comparison.NewValue,
            Calendar.Today().AddDays(Duration.PitchingStatsChange));
    }
}