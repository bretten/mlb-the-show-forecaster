using System.Text.Json;
using System.Text.Json.Nodes;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Reports.Exceptions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Services.Reports;

public class TrendReportJsonConverterTests
{
    [Fact]
    public void Read_JsonWithMissingProperty_ThrowsException()
    {
        // Arrange
        var json = JsonMissingProperty;
        Action action = () => JsonSerializer.Deserialize<TrendReport>(json, new JsonSerializerOptions()
        {
            Converters = { new TrendReportJsonConverter() }
        });

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<TrendReportJsonDeserializationAbsentMemberException>(actual);
    }

    [Fact]
    public void Read_Json_ReturnsParsedTrendReport()
    {
        // Arrange
        const string json = Json;
        var expected = TrendReport;

        // Act
        var actual = JsonSerializer.Deserialize<TrendReport>(json, new JsonSerializerOptions()
        {
            Converters = { new TrendReportJsonConverter() }
        });

        // Assert
        Assert.Equal(expected.Year, actual.Year);
        Assert.Equal(expected.CardExternalId, actual.CardExternalId);
        Assert.Equal(expected.MlbId, actual.MlbId);
        Assert.Equal(expected.PrimaryPosition, actual.PrimaryPosition);
        Assert.Equal(expected.OverallRating, actual.OverallRating);
        Assert.Equal(expected.CardName, actual.CardName);
        Assert.Equal(expected.MetricsByDate, actual.MetricsByDate);
        Assert.Equal(expected.Impacts, actual.Impacts);
    }

    [Fact]
    public void Write_TrendReport_ReturnsJson()
    {
        // Arrange
        var trendReport = TrendReport;
        const string expected = Json;

        // Act
        var actual = JsonSerializer.Serialize(trendReport, new JsonSerializerOptions()
        {
            Converters = { new TrendReportJsonConverter() }
        });

        // Assert
        Assert.True(JsonNode.DeepEquals(JsonNode.Parse(expected), JsonNode.Parse(actual)));
    }

    private const string JsonMissingProperty = @"{
      ""Year"": 2024,
      ""CardExternalId"": ""00000000-0000-0000-0000-000000000001"",
      ""MlbId"": 1,
      ""PrimaryPosition"": ""CF"",
      ""OverallRating"": 99,
      ""MetricsByDate"": [],
      ""Impacts"": []
    }";

    private const string Json = @"{
      ""Year"": 2024,
      ""CardExternalId"": ""00000000-0000-0000-0000-000000000001"",
      ""MlbId"": 1,
      ""CardName"": ""Dottie"",
      ""PrimaryPosition"": ""CF"",
      ""OverallRating"": 99,
      ""MetricsByDate"": [
        {
          ""Date"": ""2024-10-05"",
          ""BuyPrice"": 100,
          ""SellPrice"": 200,
          ""BattingScore"": 0.1,
          ""SignificantBattingParticipation"": false,
          ""PitchingScore"": 0.2,
          ""SignificantPitchingParticipation"": false,
          ""FieldingScore"": 0.3,
          ""SignificantFieldingParticipation"": false,
          ""BattingAverage"": 0.111,
          ""OnBasePercentage"": 0.112,
          ""Slugging"": 0.113,
          ""EarnedRunAverage"": 0.114,
          ""OpponentsBattingAverage"": 0.115,
          ""StrikeoutsPer9"": 0.116,
          ""BaseOnBallsPer9"": 117,
          ""HomeRunsPer9"": 0.118,
          ""FieldingPercentage"": 0.119
        },
        {
          ""Date"": ""2024-10-06"",
          ""BuyPrice"": 100,
          ""SellPrice"": 200,
          ""BattingScore"": 0.1,
          ""SignificantBattingParticipation"": false,
          ""PitchingScore"": 0.2,
          ""SignificantPitchingParticipation"": false,
          ""FieldingScore"": 0.3,
          ""SignificantFieldingParticipation"": false,
          ""BattingAverage"": 0.111,
          ""OnBasePercentage"": 0.112,
          ""Slugging"": 0.113,
          ""EarnedRunAverage"": 0.114,
          ""OpponentsBattingAverage"": 0.115,
          ""StrikeoutsPer9"": 0.116,
          ""BaseOnBallsPer9"": 117,
          ""HomeRunsPer9"": 0.118,
          ""FieldingPercentage"": 0.119
        }
      ],
      ""Impacts"": [
        {
          ""Start"": ""2024-10-08"",
          ""End"": ""2024-10-09"",
          ""Description"": ""Trend impact description""
        }
      ]
    }";

    private static readonly TrendReport TrendReport = Faker.FakeTrendReport(year: 2024,
        externalId: new Guid("00000000-0000-0000-0000-000000000001"),
        mlbId: 1,
        position: Position.CenterField,
        overallRating: 99,
        cardName: "Dottie",
        new List<TrendMetricsByDate>()
        {
            Faker.FakeTrendMetricsByDate(date: new DateOnly(2024, 10, 5)),
            Faker.FakeTrendMetricsByDate(date: new DateOnly(2024, 10, 6)),
        },
        impacts: new List<TrendImpact>()
        {
            Faker.FakeTrendImpact(startDate: new DateOnly(2024, 10, 8), new DateOnly(2024, 10, 9),
                description: "Trend impact description"),
        }
    );
}