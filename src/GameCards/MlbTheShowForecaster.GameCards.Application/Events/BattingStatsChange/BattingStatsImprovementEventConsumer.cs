using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.BattingStatsChange;

/// <summary>
/// Consumes a <see cref="BattingStatsImprovementEvent"/>
///
/// <para>Applies a <see cref="StatsForecastImpact"/> to the specified <see cref="PlayerCardForecast"/></para>
/// </summary>
public sealed class BattingStatsImprovementEventConsumer : BaseForecastImpactEventConsumer<BattingStatsImprovementEvent>
{
    /// <inheritdoc />
    public BattingStatsImprovementEventConsumer(ICommandSender commandSender, ITrendReporter trendReporter,
        ForecastImpactDuration duration) : base(commandSender, trendReporter, duration)
    {
    }

    /// <inheritdoc />
    protected override ForecastImpact CreateImpact(IForecastImpactEvent ev)
    {
        var e = (BattingStatsImprovementEvent)ev;
        return new BattingStatsForecastImpact(e.Comparison.ReferenceValue, e.Comparison.NewValue, ev.Date,
            ev.Date.AddDays(Duration.BattingStatsChange));
    }
}