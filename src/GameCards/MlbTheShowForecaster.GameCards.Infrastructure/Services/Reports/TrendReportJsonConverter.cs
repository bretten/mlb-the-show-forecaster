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
        bool isBoosted = false;
        int? orders1H = null;
        int? orders24H = null;
        int? buyPrice = null;
        decimal? buyPriceChange24H = null;
        int? sellPrice = null;
        decimal? sellPriceChange24H = null;
        decimal? score = null;
        decimal? scoreChange2W = null;
        int? demand = null;

        // Read property names and their values
        while (reader.Read())
        {
            if (reader.TokenType != JsonTokenType.PropertyName) continue;
            var propertyName = reader.GetString();

            switch (propertyName)
            {
                case var _ when GetPropertyName(nameof(TrendReport.Year), options) == propertyName:
                    reader.Read();
                    year = reader.GetInt32();
                    break;
                case var _ when GetPropertyName(nameof(TrendReport.CardExternalId), options) == propertyName:
                    reader.Read();
                    cardExternalId = reader.GetString();
                    break;
                case var _ when GetPropertyName(nameof(TrendReport.MlbId), options) == propertyName:
                    reader.Read();
                    mlbId = reader.GetInt32();
                    break;
                case var _ when GetPropertyName(nameof(TrendReport.CardName), options) == propertyName:
                    reader.Read();
                    cardName = reader.GetString();
                    break;
                case var _ when GetPropertyName(nameof(TrendReport.PrimaryPosition), options) == propertyName:
                    reader.Read();
                    position = reader.GetString();
                    break;
                case var _ when GetPropertyName(nameof(TrendReport.OverallRating), options) == propertyName:
                    reader.Read();
                    overallRating = reader.GetInt32();
                    break;
                case var _ when GetPropertyName(nameof(TrendReport.MetricsByDate), options) == propertyName:
                    reader.Read();

                    if (reader.TokenType != JsonTokenType.StartArray)
                    {
                        break;
                    }

                    // Extract the array JSON element
                    var metricsArray = JsonDocument.ParseValue(ref reader).RootElement;
                    // Use default deserialization
                    metrics = metricsArray.Deserialize<List<TrendMetricsByDate>>(options);

                    break;
                case var _ when GetPropertyName(nameof(TrendReport.Impacts), options) == propertyName:
                    reader.Read();

                    if (reader.TokenType != JsonTokenType.StartArray)
                    {
                        break;
                    }

                    // Extract the array JSON element
                    var impactsArray = JsonDocument.ParseValue(ref reader).RootElement;
                    // Use default deserialization
                    impacts = impactsArray.Deserialize<List<TrendImpact>>(options);

                    break;
                case var _ when GetPropertyName(nameof(TrendReport.IsBoosted), options) == propertyName:
                    reader.Read();
                    isBoosted = reader.GetBoolean();
                    break;
                case var _ when GetPropertyName(nameof(TrendReport.Orders1H), options) == propertyName:
                    reader.Read();
                    orders1H = reader.GetInt32();
                    break;
                case var _ when GetPropertyName(nameof(TrendReport.Orders24H), options) == propertyName:
                    reader.Read();
                    orders24H = reader.GetInt32();
                    break;
                case var _ when GetPropertyName(nameof(TrendReport.BuyPrice), options) == propertyName:
                    reader.Read();
                    buyPrice = reader.GetInt32();
                    break;
                case var _ when GetPropertyName(nameof(TrendReport.BuyPriceChange24H), options) == propertyName:
                    reader.Read();
                    buyPriceChange24H = reader.GetDecimal();
                    break;
                case var _ when GetPropertyName(nameof(TrendReport.SellPrice), options) == propertyName:
                    reader.Read();
                    sellPrice = reader.GetInt32();
                    break;
                case var _ when GetPropertyName(nameof(TrendReport.SellPriceChange24H), options) == propertyName:
                    reader.Read();
                    sellPriceChange24H = reader.GetDecimal();
                    break;
                case var _ when GetPropertyName(nameof(TrendReport.Score), options) == propertyName:
                    reader.Read();
                    score = reader.GetDecimal();
                    break;
                case var _ when GetPropertyName(nameof(TrendReport.ScoreChange2W), options) == propertyName:
                    reader.Read();
                    scoreChange2W = reader.GetDecimal();
                    break;
                case var _ when GetPropertyName(nameof(TrendReport.Demand), options) == propertyName:
                    reader.Read();
                    demand = reader.GetInt32();
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
            Impacts: impacts,
            IsBoosted: isBoosted,
            Orders1H: orders1H!.Value,
            Orders24H: orders24H!.Value,
            BuyPrice: buyPrice!.Value,
            BuyPriceChange24H: buyPriceChange24H!.Value,
            SellPrice: sellPrice!.Value,
            SellPriceChange24H: sellPriceChange24H!.Value,
            Score: score!.Value,
            ScoreChange2W: scoreChange2W!.Value,
            Demand: demand!.Value
        );
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TrendReport value, JsonSerializerOptions options)
    {
        // Start the whole JSON object
        writer.WriteStartObject();

        // Top-level properties
        writer.WriteNumber(GetEncodedPropertyName(nameof(TrendReport.Year), options), value.Year.Value);
        writer.WriteString(GetEncodedPropertyName(nameof(TrendReport.CardExternalId), options),
            value.CardExternalId.Value.ToString());
        writer.WriteNumber(GetEncodedPropertyName(nameof(TrendReport.MlbId), options), value.MlbId.Value);
        writer.WriteString(GetEncodedPropertyName(nameof(TrendReport.CardName), options), value.CardName.Value);
        writer.WriteString(GetEncodedPropertyName(nameof(TrendReport.PrimaryPosition), options),
            value.PrimaryPosition.GetDisplayName());
        writer.WriteNumber(GetEncodedPropertyName(nameof(TrendReport.OverallRating), options),
            value.OverallRating.Value);

        // Metrics by date
        writer.WritePropertyName(GetEncodedPropertyName(nameof(TrendReport.MetricsByDate), options));
        writer.WriteRawValue(JsonSerializer.Serialize(value.MetricsByDate, options));

        // Trend impacts
        writer.WritePropertyName(GetEncodedPropertyName(nameof(TrendReport.Impacts), options));
        writer.WriteRawValue(JsonSerializer.Serialize(value.Impacts, options));

        // Boost
        writer.WriteBoolean(GetEncodedPropertyName(nameof(TrendReport.IsBoosted), options), value.IsBoosted);

        // Orders
        writer.WriteNumber(GetEncodedPropertyName(nameof(TrendReport.Orders1H), options), value.Orders1H);
        writer.WriteNumber(GetEncodedPropertyName(nameof(TrendReport.Orders24H), options), value.Orders24H);

        // Prices
        writer.WriteNumber(GetEncodedPropertyName(nameof(TrendReport.BuyPrice), options), value.BuyPrice);
        writer.WriteNumber(GetEncodedPropertyName(nameof(TrendReport.BuyPriceChange24H), options),
            value.BuyPriceChange24H);
        writer.WriteNumber(GetEncodedPropertyName(nameof(TrendReport.SellPrice), options), value.SellPrice);
        writer.WriteNumber(GetEncodedPropertyName(nameof(TrendReport.SellPriceChange24H), options),
            value.SellPriceChange24H);

        // Scores
        writer.WriteNumber(GetEncodedPropertyName(nameof(TrendReport.Score), options), value.Score);
        writer.WriteNumber(GetEncodedPropertyName(nameof(TrendReport.ScoreChange2W), options), value.ScoreChange2W);

        // Demand
        writer.WriteNumber(GetEncodedPropertyName(nameof(TrendReport.Demand), options), value.Demand);

        // End the whole JSON object
        writer.WriteEndObject();
    }

    /// <summary>
    /// Gets the property name converted using <see cref="JsonNamingPolicy"/>
    /// </summary>
    /// <param name="propertyName">The property name</param>
    /// <param name="options"><see cref="JsonSerializerOptions"/></param>
    /// <returns>The converted name</returns>
    private static string GetPropertyName(string propertyName, JsonSerializerOptions options)
    {
        var namingPolicy = options.PropertyNamingPolicy ?? JsonNamingPolicy.CamelCase;

        return namingPolicy.ConvertName(propertyName);
    }

    /// <summary>
    /// Gets the property name converted using <see cref="JsonNamingPolicy"/> and then encoded using <see cref="JsonEncodedText"/>
    /// </summary>
    /// <param name="propertyName">The property name</param>
    /// <param name="options"><see cref="JsonSerializerOptions"/></param>
    /// <returns>The converted name</returns>
    private static JsonEncodedText GetEncodedPropertyName(string propertyName, JsonSerializerOptions options)
    {
        return JsonEncodedText.Encode(GetPropertyName(propertyName, options));
    }
}