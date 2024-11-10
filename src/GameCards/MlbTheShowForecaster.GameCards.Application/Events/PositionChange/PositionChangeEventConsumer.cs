using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PositionChange;

/// <summary>
/// Consumes a <see cref="PositionChangeEvent"/>
///
/// <para>Applies a <see cref="PositionChangeForecastImpact"/> to the specified <see cref="PlayerCardForecast"/></para>
/// </summary>
public sealed class PositionChangeEventConsumer : BaseForecastImpactEventConsumer<PositionChangeEvent>
{
    /// <inheritdoc />
    public PositionChangeEventConsumer(ICommandSender commandSender, ITrendReporter trendReporter,
        ForecastImpactDuration duration) : base(commandSender, trendReporter, duration)
    {
    }

    /// <inheritdoc />
    protected override ForecastImpact CreateImpact(IForecastImpactEvent ev)
    {
        var e = (PositionChangeEvent)ev;
        return new PositionChangeForecastImpact(oldPosition: e.OldPosition, newPosition: e.NewPosition,
            startDate: ev.Date, ev.Date.AddDays(Duration.PositionChange));
    }
}