using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.OverallRatingChange;

/// <summary>
/// Consumes a <see cref="OverallRatingDeclineEvent"/>
///
/// <para>Applies a <see cref="OverallRatingChangeForecastImpact"/> to the specified <see cref="PlayerCardForecast"/></para>
/// </summary>
public sealed class OverallRatingDeclineEventConsumer : BaseForecastImpactEventConsumer<OverallRatingDeclineEvent>
{
    /// <inheritdoc />
    public OverallRatingDeclineEventConsumer(ICommandSender commandSender, ICalendar calendar,
        ForecastImpactDuration duration) : base(commandSender, calendar, duration)
    {
    }

    /// <inheritdoc />
    protected override ForecastImpact CreateImpact(IForecastImpactEvent ev)
    {
        var e = (OverallRatingDeclineEvent)ev;
        return new OverallRatingChangeForecastImpact(oldRating: e.PreviousOverallRating, newRating: e.NewOverallRating,
            ev.Date, ev.Date.AddDays(Duration.OverallRatingChange));
    }
}