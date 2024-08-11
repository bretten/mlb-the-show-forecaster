using System.Net;
using com.brettnamba.MlbTheShowForecaster.DomainApis.PlayerStatusApi;
using com.brettnamba.MlbTheShowForecaster.DomainApis.PlayerStatusApi.Responses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Exceptions;
using Moq;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Services;

public class PlayerMatcherTests
{
    [Fact]
    public async Task GetPlayerByName_PlayerDoesNotExist_ReturnsNull()
    {
        // Arrange
        const string name = "dot spot";
        const string team = "SEA";

        var stubPlayerStatusApi = new Mock<IPlayerStatusApi>();
        stubPlayerStatusApi.Setup(x => x.FindPlayer(name, team))
            .ReturnsAsync(new ApiResponse<PlayerResponse>(new HttpResponseMessage(HttpStatusCode.NotFound),
                content: new PlayerResponse(), new RefitSettings()));

        var playerMatcher = new PlayerMatcher(stubPlayerStatusApi.Object);

        // Act
        var actual = await playerMatcher.GetPlayerByName(CardName.Create(name), TeamShortName.Create(team));

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async Task GetPlayerByName_PlayerExists_ReturnsMlbId()
    {
        // Arrange
        const string name = "dot spot";
        const string team = "SEA";

        var playerResponse = new PlayerResponse(1, "dot", "spot", "RF", "SEA");

        var stubPlayerStatusApi = new Mock<IPlayerStatusApi>();
        stubPlayerStatusApi.Setup(x => x.FindPlayer(name, team))
            .ReturnsAsync(new ApiResponse<PlayerResponse>(new HttpResponseMessage(HttpStatusCode.OK), playerResponse,
                new RefitSettings()));

        var playerMatcher = new PlayerMatcher(stubPlayerStatusApi.Object);

        // Act
        var actual = await playerMatcher.GetPlayerByName(CardName.Create(name), TeamShortName.Create(team));

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(1, actual.Value);
    }

    [Fact]
    public async Task GetPlayerByName_ServerError_ThrowsException()
    {
        // Arrange
        const string name = "dot spot";
        const string team = "SEA";

        var playerResponse = new PlayerResponse();

        var stubPlayerStatusApi = new Mock<IPlayerStatusApi>();
        stubPlayerStatusApi.Setup(x => x.FindPlayer(name, team))
            .ReturnsAsync(new ApiResponse<PlayerResponse>(new HttpResponseMessage(HttpStatusCode.InternalServerError),
                playerResponse, new RefitSettings()));

        var playerMatcher = new PlayerMatcher(stubPlayerStatusApi.Object);

        var action = async () => await playerMatcher.GetPlayerByName(CardName.Create(name), TeamShortName.Create(team));

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<PlayerCouldNotBeMatchedException>(actual);
    }
}