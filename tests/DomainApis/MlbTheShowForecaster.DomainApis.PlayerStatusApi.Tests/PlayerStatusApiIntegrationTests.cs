using System.Text.Json;
using System.Text.Json.Serialization;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.DomainApis.PlayerStatusApi.Tests;

public class PlayerStatusApiIntegrationTests : IAsyncLifetime
{
    private readonly IContainer _container;
    private const int Port = 1080;

    private int HostPort => _container.GetMappedPublicPort(Port);

    /// <summary>
    /// Configures the container options that will be used for each test
    /// </summary>
    /// <exception cref="DockerNotRunningException">Thrown if Docker is not running</exception>
    public PlayerStatusApiIntegrationTests()
    {
        try
        {
            _container = new ContainerBuilder()
                .WithImage("mockserver/mockserver")
                .WithEnvironment("MOCKSERVER_LOG_LEVEL", "TRACE")
                .WithName(GetType().Name + Guid.NewGuid())
                .WithPortBinding(Port, true)
                .WithWaitStrategy(Wait.ForUnixContainer()
                    .UntilPortIsAvailable(Port, o => o.WithTimeout(TimeSpan.FromMinutes(1)))
                )
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
    public async Task FindPlayer_NameAndTeam_ReturnsPlayer()
    {
        // Arrange
        await Task.Delay(TimeSpan.FromSeconds(2)); // Wait for mockserver
        await MockEndpoint(ReturnsPlayerEndpoint);
        const string playerQuery = "dot spot";
        const string teamQuery = "SEA";
        var client = GetClient();

        // Act
        var actual = await client.FindPlayer(playerQuery, teamQuery);

        // Assert
        Assert.True(actual.IsSuccessStatusCode);
        Assert.Equal(20, actual.Content.MlbId);
        Assert.Equal("Dot", actual.Content.FirstName);
        Assert.Equal("Spot", actual.Content.LastName);
        Assert.Equal("2B", actual.Content.Position);
        Assert.Equal("SEA", actual.Content.Team);
    }

    private async Task MockEndpoint(string endpoint)
    {
        using var httpClient = new HttpClient();
        await httpClient.PutAsync($"http://localhost:{HostPort}/mockserver/expectation", new StringContent(endpoint));
    }

    private const string ReturnsPlayerEndpoint = @"
    [
        {
            ""httpRequest"": {
                ""method"": ""GET"",
                ""path"": ""/players"",
                ""queryStringParameters"": {
                    ""name"": [
                        ""dot spot""
                    ],
                    ""team"": [
                        ""SEA""
                    ]
                }
            },
            ""httpResponse"": {
                ""body"": {
                    ""mlbId"": 20,
                    ""firstName"": ""Dot"",
                    ""lastName"": ""Spot"",
                    ""position"": ""2B"",
                    ""team"": ""SEA""
                }
            }
        }
    ]";

    private IPlayerStatusApi GetClient()
    {
        return RestService.For<IPlayerStatusApi>($"http://localhost:{HostPort}",
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

    public async Task DisposeAsync() => await _container.DisposeAsync();

    private sealed class DockerNotRunningException : Exception
    {
        public DockerNotRunningException(string? message) : base(message)
        {
        }
    }
}