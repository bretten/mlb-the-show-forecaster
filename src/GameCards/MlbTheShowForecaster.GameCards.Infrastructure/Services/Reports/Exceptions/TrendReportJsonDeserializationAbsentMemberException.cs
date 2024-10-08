using System.Text;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Reports;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Reports.Exceptions;

/// <summary>
/// Thrown when JSON cannot be deserialized as a <see cref="TrendReport"/> due to missing data
/// </summary>
public sealed class TrendReportJsonDeserializationAbsentMemberException : Exception
{
    public TrendReportJsonDeserializationAbsentMemberException(int? year = null, string? cardExternalId = null,
        int? mlbId = null, string? cardName = null, string? position = null, int? overallRating = null,
        List<TrendMetricsByDate>? metrics = null, List<TrendImpact>? impacts = null)
    {
        var b = new StringBuilder(
            $"JSON could not be deserialized to {nameof(TrendReport)}. The following properties were missing: ");
        if (year == null) b.Append($"{nameof(TrendReport.Year)} ");
        if (cardExternalId == null) b.Append($"{nameof(TrendReport.CardExternalId)} ");
        if (mlbId == null) b.Append($"{nameof(TrendReport.MlbId)} ");
        if (cardName == null) b.Append($"{nameof(TrendReport.CardName)} ");
        if (position == null) b.Append($"{nameof(TrendReport.PrimaryPosition)} ");
        if (overallRating == null) b.Append($"{nameof(TrendReport.OverallRating)} ");
        if (metrics == null) b.Append($"{nameof(TrendReport.MetricsByDate)} ");
        if (impacts == null) b.Append($"{nameof(TrendReport.Impacts)} ");
        Message = b.ToString().TrimEnd();
    }

    public override string Message { get; }
}