using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Extensions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Reports.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Reports;

/// <summary>
/// JSON conversion for <see cref="TrendReport"/>
/// </summary>
public sealed class TrendReportJsonConverter : JsonConverter<TrendReport>
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(TrendReport);

    /// <inheritdoc />
    public override TrendReport Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Read the properties in no particular order, meaning we can serialize in no particular order
        int? year = null;
        string? cardExternalId = null;
        int? mlbId = null;
        string? cardName = null;
        string? position = null;
        int? overallRating = null;
        List<TrendMetricsByDate>? metrics = null;
        List<TrendImpact>? impacts = null;

        // Read property names and their values
        while (reader.Read())
        {
            if (reader.TokenType != JsonTokenType.PropertyName) continue;
            var propertyName = reader.GetString();

            switch (propertyName)
            {
                case nameof(TrendReport.Year):
                    reader.Read();
                    year = reader.GetInt32();
                    break;
                case nameof(TrendReport.CardExternalId):
                    reader.Read();
                    cardExternalId = reader.GetString();
                    break;
                case nameof(TrendReport.MlbId):
                    reader.Read();
                    mlbId = reader.GetInt32();
                    break;
                case nameof(TrendReport.CardName):
                    reader.Read();
                    cardName = reader.GetString();
                    break;
                case nameof(TrendReport.PrimaryPosition):
                    reader.Read();
                    position = reader.GetString();
                    break;
                case nameof(TrendReport.OverallRating):
                    reader.Read();
                    overallRating = reader.GetInt32();
                    break;
                case nameof(TrendReport.MetricsByDate):
                    reader.Read();

                    if (reader.TokenType != JsonTokenType.StartArray)
                    {
                        break;
                    }

                    // Extract the array JSON element
                    var metricsArray = JsonDocument.ParseValue(ref reader).RootElement;
                    // Use default deserialization
                    metrics = metricsArray.Deserialize<List<TrendMetricsByDate>>();

                    break;
                case nameof(TrendReport.Impacts):
                    reader.Read();

                    if (reader.TokenType != JsonTokenType.StartArray)
                    {
                        break;
                    }

                    // Extract the array JSON element
                    var impactsArray = JsonDocument.ParseValue(ref reader).RootElement;
                    // Use default deserialization
                    impacts = impactsArray.Deserialize<List<TrendImpact>>();

                    break;
            }
        }

        if (year == null || cardExternalId == null || mlbId == null || cardName == null || position == null ||
            overallRating == null || metrics == null || impacts == null)
        {
            throw new TrendReportJsonDeserializationAbsentMemberException(year, cardExternalId, mlbId, cardName,
                position, overallRating, metrics, impacts);
        }

        return new TrendReport(
            Year: SeasonYear.Create((ushort)year),
            CardExternalId: CardExternalId.Create(new Guid(cardExternalId)),
            MlbId: MlbId.Create(mlbId.Value),
            CardName: CardName.Create(cardName),
            PrimaryPosition: (Position)TypeDescriptor.GetConverter(typeof(Position)).ConvertFrom(position)!,
            OverallRating: OverallRating.Create(overallRating.Value),
            MetricsByDate: metrics,
            Impacts: impacts
        );
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TrendReport value, JsonSerializerOptions options)
    {
        // Start the whole JSON object
        writer.WriteStartObject();

        // Top-level properties
        writer.WriteNumber(Constants.Year, value.Year.Value);
        writer.WriteString(Constants.CardExternalId, value.CardExternalId.Value.ToString());
        writer.WriteNumber(Constants.MlbId, value.MlbId.Value);
        writer.WriteString(Constants.CardName, value.CardName.Value);
        writer.WriteString(Constants.PrimaryPosition, value.PrimaryPosition.GetDisplayName());
        writer.WriteNumber(Constants.OverallRating, value.OverallRating.Value);

        // Metrics by date
        writer.WritePropertyName(nameof(Constants.MetricsByDate));
        writer.WriteRawValue(JsonSerializer.Serialize(value.MetricsByDate, options));

        // Trend impacts
        writer.WritePropertyName(nameof(Constants.Impacts));
        writer.WriteRawValue(JsonSerializer.Serialize(value.Impacts, options));

        // End the whole JSON object
        writer.WriteEndObject();
    }

    /// <summary>
    /// Constants for writing JSON property names
    /// </summary>
    private static class Constants
    {
        public static readonly JsonEncodedText Year = JsonEncodedText.Encode(nameof(TrendReport.Year));

        public static readonly JsonEncodedText CardExternalId =
            JsonEncodedText.Encode(nameof(TrendReport.CardExternalId));

        public static readonly JsonEncodedText MlbId = JsonEncodedText.Encode(nameof(TrendReport.MlbId));
        public static readonly JsonEncodedText CardName = JsonEncodedText.Encode(nameof(TrendReport.CardName));

        public static readonly JsonEncodedText PrimaryPosition =
            JsonEncodedText.Encode(nameof(TrendReport.PrimaryPosition));

        public static readonly JsonEncodedText
            OverallRating = JsonEncodedText.Encode(nameof(TrendReport.OverallRating));

        public static readonly JsonEncodedText
            MetricsByDate = JsonEncodedText.Encode(nameof(TrendReport.MetricsByDate));

        public static readonly JsonEncodedText Impacts = JsonEncodedText.Encode(nameof(TrendReport.Impacts));
    }
}