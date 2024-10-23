using com.brettnamba.MlbTheShowForecaster.Apps.Gateway.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Tests.SignalR;

public class SignalRMultiplexerIntegrationTests
{
    [Fact]
    [Trait("Category", "Integration")]
    public async Task ExecuteAsync_Hubs_ConnectsToHubs()
    {
        /*
         * Arrange
         */
        const string url = "http://localhost:5123";
        const string hubUrl = url + "/hub";
        // Mocks
        var mockHubContext = new Mock<IHubContext<GatewayHub>>();
        var mockLogger = new Mock<ILogger<SignalRMultiplexer>>();
        var interval = TimeSpan.FromMilliseconds(10);
        var hubs = new HashSet<RelayedHub>()
        {
            new RelayedHub(hubUrl, new HashSet<string>() { "Method" }),
            new RelayedHub("ForceFailedConnection", new HashSet<string>() { "Method2" })
        };
        var options = new SignalRMultiplexer.Options(interval, hubs, new TestRetryPolicy());

        // Create the app
        var builder = WebApplication.CreateBuilder([]);
        builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);
        builder.Services.AddSignalR();
        builder.Services.AddHostedService<SignalRMultiplexer>(_ =>
            new SignalRMultiplexer(mockHubContext.Object, options, mockLogger.Object));
        var app = builder.Build();
        app.Urls.Add(url);
        app.UseRouting();
        app.MapHub<GatewayHub>("/hub");

        // Get the hub to send messages
        var hub = app.Services.GetRequiredService<IHubContext<GatewayHub>>();

        /*
         * Act
         */
        // Cancellation token to stop the program
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        // Start the host
        _ = app.RunAsync();
        // Send a test message
        await hub.Clients.All.SendAsync("Method", "test", cts.Token);
        // Let it do some work
        await Task.Delay(TimeSpan.FromSeconds(2), cts.Token);

        /*
         * Assert
         */
        // The hub should have connected
        mockLogger.Verify(x => x.Log(LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.StartsWith($"Hub connected: {hubUrl}")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }

    private sealed class TestRetryPolicy : IRetryPolicy
    {
        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            return TimeSpan.Zero;
        }
    }
}