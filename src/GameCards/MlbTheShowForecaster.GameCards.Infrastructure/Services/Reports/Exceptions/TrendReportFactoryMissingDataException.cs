using System.Text;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Reports.Exceptions;

/// <summary>
/// Thrown when <see cref="TrendReportFactory"/> cannot create a report due to missing data
/// </summary>
public sealed class TrendReportFactoryMissingDataException : Exception
{
    public TrendReportFactoryMissingDataException(PlayerCard? card, Listing? listing, PlayerCardForecast? forecast,
        MlbId? mlbId, SeasonYear year, CardExternalId cardExternalId)
    {
        var b = new StringBuilder($"{nameof(TrendReportFactory)} could not build report. Card data was missing for");
        b.Append($"{year.Value} and {cardExternalId.Value.ToString()}: ");
        if (card == null) b.Append($"{nameof(PlayerCard)} ");
        if (listing == null) b.Append($"{nameof(Listing)} ");
        if (forecast == null) b.Append($"{nameof(PlayerCardForecast)} ");
        if (mlbId == null) b.Append($"{nameof(MlbId)} ");
        Message = b.ToString().TrimEnd();
    }

    public TrendReportFactoryMissingDataException(SeasonYear year, MlbId mlbId)
    {
        Message = $"Forecast required to retrieve data, but could not be found for {year.Value} and {mlbId.Value}";
    }

    public override string Message { get; }
}