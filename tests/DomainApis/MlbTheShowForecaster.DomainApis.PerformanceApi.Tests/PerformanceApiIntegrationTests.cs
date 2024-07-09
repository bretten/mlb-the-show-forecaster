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
        var startQuery = new DateOnly(2024, 7, 1).ToString("yyyy-MM-dd");
        var endQuery = new DateOnly(2024, 7, 8).ToString("yyyy-MM-dd");
        var client = GetClient();

        // Act
        var actual = await client.GetPlayerSeasonPerformance(seasonQuery, playerMlbIdQuery, startQuery, endQuery);

        // Assert
        Assert.True(actual.IsSuccessStatusCode);
        Assert.Equal(2024, actual.Content.Season);
        Assert.Equal(20, actual.Content.MlbId);
        Assert.Equal(0.5600m, actual.Content.BattingScore);
        Assert.True(actual.Content.HadSignificantBattingParticipation);
        Assert.Equal(0m, actual.Content.PitchingScore);
        Assert.False(actual.Content.HadSignificantPitchingParticipation);
        Assert.Equal(0.97m, actual.Content.FieldingScore);
        Assert.True(actual.Content.HadSignificantFieldingParticipation);
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
                        ""2024-07-01""
                    ],
                    ""end"": [
                        ""2024-07-08""
                    ]
                }
            },
            ""httpResponse"": {
                ""body"": {
                    ""season"": 2024,
                    ""mlbId"": 20,
                    ""battingScore"": 0.5600,
                    ""hadSignificantBattingParticipation"": true,
                    ""pitchingScore"": 0,
                    ""hadSignificantPitchingParticipation"": false,
                    ""fieldingScore"": 0.97,
                    ""hadSignificantFieldingParticipation"": true
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