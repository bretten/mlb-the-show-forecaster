using System.Text.Json;
using System.Text.Json.Serialization;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.DomainApis.PerformanceApi.Tests;

public class PerformanceApiIntegrationTests : IAsyncLifetime
{
    private readonly IContainer _container;
    private const int Port = 10802;

    /// <summary>
    /// Configures the container options that will be used for each test
    /// </summary>
    /// <exception cref="DockerNotRunningException">Thrown if Docker is not running</exception>
    public PerformanceApiIntegrationTests()
    {
        try
        {
            _container = new ContainerBuilder()
                .WithImage("mockserver/mockserver")
                .WithEnvironment("MOCKSERVER_LOG_LEVEL", "TRACE")
                .WithName(GetType().Name + Guid.NewGuid())
                .WithPortBinding(Port, 1080)
                .Build();
        }
        catch (ArgumentException e)
        {
            if (!e.Message.Contains("Docker is either not running or misconfigured"))
            {
                throw;
            }

            throw new DockerNotRunningException($"Docker is required to run tests for {GetType().Name}");
        }
    }

    [Fact]
    public async Task GetPlayerSeasonPerformance_PlayerMlbIdAndTimePeriod_ReturnsPlayerSeasonPerformance()
    {
        // Arrange
        await Task.Delay(TimeSpan.FromSeconds(2)); // Wait for mockserver
        await MockEndpoint(ReturnsPlayerSeasonEndpoint);
        const ushort seasonQuery = 2024;
        const int playerMlbIdQuery = 20;
        var startQuery = new DateOnly(2024, 10, 1).ToString("yyyy-MM-dd");
        var endQuery = new DateOnly(2024, 10, 2).ToString("yyyy-MM-dd");
        var client = GetClient();

        // Act
        var actual = await client.GetPlayerSeasonPerformance(seasonQuery, playerMlbIdQuery, startQuery, endQuery);

        // Assert
        Assert.True(actual.IsSuccessStatusCode);
        Assert.Equal(2024, actual.Content.Season);
        Assert.Equal(20, actual.Content.MlbId);

        Assert.Equal(new DateOnly(2024, 10, 1), actual.Content.MetricsByDate[0].Date);
        Assert.Equal(0.1m, actual.Content.MetricsByDate[0].BattingScore);
        Assert.False(actual.Content.MetricsByDate[0].SignificantBattingParticipation);
        Assert.Equal(0.2m, actual.Content.MetricsByDate[0].PitchingScore);
        Assert.False(actual.Content.MetricsByDate[0].SignificantPitchingParticipation);
        Assert.Equal(0.3m, actual.Content.MetricsByDate[0].FieldingScore);
        Assert.False(actual.Content.MetricsByDate[0].SignificantFieldingParticipation);
        Assert.Equal(1.1m, actual.Content.MetricsByDate[0].BattingAverage);
        Assert.Equal(1.2m, actual.Content.MetricsByDate[0].OnBasePercentage);
        Assert.Equal(1.3m, actual.Content.MetricsByDate[0].Slugging);
        Assert.Equal(1.4m, actual.Content.MetricsByDate[0].EarnedRunAverage);
        Assert.Equal(1.5m, actual.Content.MetricsByDate[0].OpponentsBattingAverage);
        Assert.Equal(1.6m, actual.Content.MetricsByDate[0].StrikeoutsPer9);
        Assert.Equal(1.7m, actual.Content.MetricsByDate[0].BaseOnBallsPer9);
        Assert.Equal(1.8m, actual.Content.MetricsByDate[0].HomeRunsPer9);
        Assert.Equal(1.9m, actual.Content.MetricsByDate[0].FieldingPercentage);

        Assert.Equal(new DateOnly(2024, 10, 2), actual.Content.MetricsByDate[1].Date);
        Assert.Equal(0.4m, actual.Content.MetricsByDate[1].BattingScore);
        Assert.True(actual.Content.MetricsByDate[1].SignificantBattingParticipation);
        Assert.Equal(0.5m, actual.Content.MetricsByDate[1].PitchingScore);
        Assert.True(actual.Content.MetricsByDate[1].SignificantPitchingParticipation);
        Assert.Equal(0.6m, actual.Content.MetricsByDate[1].FieldingScore);
        Assert.True(actual.Content.MetricsByDate[1].SignificantFieldingParticipation);
        Assert.Equal(2.1m, actual.Content.MetricsByDate[1].BattingAverage);
        Assert.Equal(2.2m, actual.Content.MetricsByDate[1].OnBasePercentage);
        Assert.Equal(2.3m, actual.Content.MetricsByDate[1].Slugging);
        Assert.Equal(2.4m, actual.Content.MetricsByDate[1].EarnedRunAverage);
        Assert.Equal(2.5m, actual.Content.MetricsByDate[1].OpponentsBattingAverage);
        Assert.Equal(2.6m, actual.Content.MetricsByDate[1].StrikeoutsPer9);
        Assert.Equal(2.7m, actual.Content.MetricsByDate[1].BaseOnBallsPer9);
        Assert.Equal(2.8m, actual.Content.MetricsByDate[1].HomeRunsPer9);
        Assert.Equal(2.9m, actual.Content.MetricsByDate[1].FieldingPercentage);
    }

    private static async Task MockEndpoint(string endpoint)
    {
        using var httpClient = new HttpClient();
        await httpClient.PutAsync($"http://localhost:{Port}/mockserver/expectation", new StringContent(endpoint));
    }

    private const string ReturnsPlayerSeasonEndpoint = @"
    [
        {
            ""httpRequest"": {
                ""method"": ""GET"",
                ""path"": ""/performance"",
                ""queryStringParameters"": {
                    ""season"": [
                        ""2024""
                    ],
                    ""playerMlbId"": [
                        ""20""
                    ],
                    ""start"": [
                        ""2024-10-01""
                    ],
                    ""end"": [
                        ""2024-10-02""
                    ]
                }
            },
            ""httpResponse"": {
                ""body"": {
                    ""season"": 2024,
                    ""mlbId"": 20,
                    ""metricsByDate"": [
                        {
                            ""date"": ""2024-10-01"",
                            ""battingScore"": 0.1,
                            ""significantBattingParticipation"": false,
                            ""pitchingScore"": 0.2,
                            ""significantPitchingParticipation"": false,
                            ""fieldingScore"": 0.3,
                            ""significantFieldingParticipation"": false,
                            ""battingAverage"": 1.1,
                            ""onBasePercentage"": 1.2,
                            ""slugging"": 1.3,
                            ""earnedRunAverage"": 1.4,
                            ""opponentsBattingAverage"": 1.5,
                            ""strikeoutsPer9"": 1.6,
                            ""baseOnBallsPer9"": 1.7,
                            ""homeRunsPer9"": 1.8,
                            ""fieldingPercentage"": 1.9
                        },
                        {
                            ""date"": ""2024-10-02"",
                            ""battingScore"": 0.4,
                            ""significantBattingParticipation"": true,
                            ""pitchingScore"": 0.5,
                            ""significantPitchingParticipation"": true,
                            ""fieldingScore"": 0.6,
                            ""significantFieldingParticipation"": true,
                            ""battingAverage"": 2.1,
                            ""onBasePercentage"": 2.2,
                            ""slugging"": 2.3,
                            ""earnedRunAverage"": 2.4,
                            ""opponentsBattingAverage"": 2.5,
                            ""strikeoutsPer9"": 2.6,
                            ""baseOnBallsPer9"": 2.7,
                            ""homeRunsPer9"": 2.8,
                            ""fieldingPercentage"": 2.9
                        }
                    ]
                }
            }
        }
    ]";

    private static IPerformanceApi GetClient()
    {
        return RestService.For<IPerformanceApi>($"http://localhost:{Port}",
            new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(
                    new JsonSerializerOptions()
                    {
                        Converters = { new JsonStringEnumConverter() }
                    }
                )
            });
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public async Task DisposeAsync() => await _container.StopAsync();

    private sealed class DockerNotRunningException : Exception
    {
        public DockerNotRunningException(string? message) : base(message)
        {
        }
    }
}