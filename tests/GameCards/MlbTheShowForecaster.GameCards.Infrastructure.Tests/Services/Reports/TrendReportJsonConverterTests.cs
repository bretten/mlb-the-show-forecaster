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
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
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
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
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
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new TrendReportJsonConverter() }
        });

        // Assert
        Assert.True(JsonNode.DeepEquals(JsonNode.Parse(expected), JsonNode.Parse(actual)));
    }

    private const string JsonMissingProperty = @"{
      ""year"": 2024,
      ""cardExternalId"": ""00000000-0000-0000-0000-000000000001"",
      ""mlbId"": 1,
      ""primaryPosition"": ""CF"",
      ""overallRating"": 99,
      ""metricsByDate"": [],
      ""impacts"": []
    }";

    private const string Json = @"{
      ""year"": 2024,
      ""cardExternalId"": ""00000000-0000-0000-0000-000000000001"",
      ""mlbId"": 1,
      ""cardName"": ""Dottie"",
      ""primaryPosition"": ""CF"",
      ""overallRating"": 99,
      ""isBoosted"": true,
      ""orders1H"": 5,
      ""orders24H"": 50,
      ""buyPrice"": 123,
      ""buyPriceChange24H"": 31.2,
      ""sellPrice"": 456,
      ""sellPriceChange24H"": 2.15,
      ""score"": 1,
      ""scoreChange2W"": 3.0,
      ""demand"": 2,
      ""metricsByDate"": [
        {
          ""date"": ""2024-10-05"",
          ""buyPrice"": 100,
          ""sellPrice"": 200,
          ""battingScore"": 0.1,
          ""significantBattingParticipation"": false,
          ""pitchingScore"": 0.2,
          ""significantPitchingParticipation"": false,
          ""fieldingScore"": 0.3,
          ""significantFieldingParticipation"": false,
          ""battingAverage"": 0.111,
          ""onBasePercentage"": 0.112,
          ""slugging"": 0.113,
          ""earnedRunAverage"": 0.114,
          ""opponentsBattingAverage"": 0.115,
          ""strikeoutsPer9"": 0.116,
          ""baseOnBallsPer9"": 0.117,
          ""homeRunsPer9"": 0.118,
          ""fieldingPercentage"": 0.119,
          ""demand"": 1,
          ""orderCount"": 10
        },
        {
          ""date"": ""2024-10-06"",
          ""buyPrice"": 100,
          ""sellPrice"": 200,
          ""battingScore"": 0.1,
          ""significantBattingParticipation"": false,
          ""pitchingScore"": 0.2,
          ""significantPitchingParticipation"": false,
          ""fieldingScore"": 0.3,
          ""significantFieldingParticipation"": false,
          ""battingAverage"": 0.111,
          ""onBasePercentage"": 0.112,
          ""slugging"": 0.113,
          ""earnedRunAverage"": 0.114,
          ""opponentsBattingAverage"": 0.115,
          ""strikeoutsPer9"": 0.116,
          ""baseOnBallsPer9"": 0.117,
          ""homeRunsPer9"": 0.118,
          ""fieldingPercentage"": 0.119,
          ""demand"": 2,
          ""orderCount"": 20
        }
      ],
      ""impacts"": [
        {
          ""start"": ""2024-10-08"",
          ""end"": ""2024-10-09"",
          ""description"": ""Trend impact description"",
          ""demand"": 1
        }
      ]
    }";

    private static readonly TrendReport TrendReport = Faker.FakeTrendReport(year: 2024,
        externalId: new Guid("00000000-0000-0000-0000-000000000001"),
        mlbId: 1,
        position: Position.CenterField,
        overallRating: 99,
        cardName: "Dottie",
        isBoosted: true,
        orders1H: 5,
        orders24H: 50,
        buyPrice: 123,
        buyPriceChange24H: 31.2m,
        sellPrice: 456,
        sellPriceChange24H: 2.15m,
        score: 1,
        scoreChange2W: 3.0m,
        demand: 2,
        metricsByDate: new List<TrendMetricsByDate>()
        {
            Faker.FakeTrendMetricsByDate(date: new DateOnly(2024, 10, 5), demand: 1, orderCount: 10),
            Faker.FakeTrendMetricsByDate(date: new DateOnly(2024, 10, 6), demand: 2, orderCount: 20),
        },
        impacts: new List<TrendImpact>()
        {
            Faker.FakeTrendImpact(startDate: new DateOnly(2024, 10, 8), new DateOnly(2024, 10, 9),
                description: "Trend impact description", demand: 1),
        }
    );
}