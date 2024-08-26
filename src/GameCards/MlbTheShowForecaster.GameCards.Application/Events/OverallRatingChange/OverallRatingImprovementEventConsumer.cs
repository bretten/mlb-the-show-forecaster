using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.OverallRatingChange;

/// <summary>
/// Consumes a <see cref="OverallRatingImprovementEvent"/>
///
/// <para>Applies a <see cref="OverallRatingChangeForecastImpact"/> to the specified <see cref="PlayerCardForecast"/></para>
/// </summary>
public sealed class
    OverallRatingImprovementEventConsumer : BaseForecastImpactEventConsumer<OverallRatingImprovementEvent>
{
    /// <inheritdoc />
    public OverallRatingImprovementEventConsumer(ICommandSender commandSender, ICalendar calendar,
        ForecastImpactDuration duration, IForecastReportPublisher publisher) : base(commandSender, calendar, duration,
        publisher)
    {
    }

    /// <inheritdoc />
    protected override ForecastImpact CreateImpact(IForecastImpactEvent ev)
    {
        var e = (OverallRatingImprovementEvent)ev;
        return new OverallRatingChangeForecastImpact(oldRating: e.PreviousOverallRating, newRating: e.NewOverallRating,
            Calendar.Today().AddDays(Duration.OverallRatingChange));
    }
}