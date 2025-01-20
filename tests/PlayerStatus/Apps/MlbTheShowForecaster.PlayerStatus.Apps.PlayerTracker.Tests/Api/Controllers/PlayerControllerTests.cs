using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.Api.Controllers;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Teams.TestClasses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.Tests.Api.Controllers;

public class PlayerControllerTests
{
    [Fact]
    public async Task FindPlayer_UnknownTeam_Returns404()
    {
        // Arrange
        const string teamQuery = "team";

        var stubTeamProvider = new Mock<ITeamProvider>();
        stubTeamProvider.Setup(x => x.GetBy(teamQuery))
            .Returns((Team?)null);

        var mockPlayerSearchService = Mock.Of<IPlayerSearchService>();

        var controller = new PlayerController(mockPlayerSearchService, stubTeamProvider.Object);

        // Act
        var actual = await controller.FindPlayer("Dot Spot", teamQuery);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<NotFoundResult>(actual);
    }

    [Fact]
    public async Task FindPlayer_UnknownPlayer_Returns404()
    {
        // Arrange
        const string playerQuery = "Dot Spot";
        var team = Faker.FakeTeam();

        var stubTeamProvider = new Mock<ITeamProvider>();
        stubTeamProvider.Setup(x => x.GetBy(team.Abbreviation.Value))
            .Returns(team);

        var stubPlayerSearchService = new Mock<IPlayerSearchService>();
        stubPlayerSearchService.Setup(x => x.FindPlayer(playerQuery, team))
            .ReturnsAsync((Player?)null);

        var controller = new PlayerController(stubPlayerSearchService.Object, stubTeamProvider.Object);

        // Act
        var actual = await controller.FindPlayer(playerQuery, team.Abbreviation.Value);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<NotFoundResult>(actual);
    }

    [Fact]
    public async Task FindPlayer_KnownPlayer_ReturnsPlayer()
    {
        // Arrange
        const string playerQuery = "Dot Spot";
        var player = Domain.Tests.Players.TestClasses.Faker.FakePlayer(1, firstName: "Dot", lastName: "Spot");
        var team = Faker.FakeTeam();

        var stubTeamProvider = new Mock<ITeamProvider>();
        stubTeamProvider.Setup(x => x.GetBy(team.Abbreviation.Value))
            .Returns(team);

        var stubPlayerSearchService = new Mock<IPlayerSearchService>();
        stubPlayerSearchService.Setup(x => x.FindPlayer(playerQuery, team))
            .ReturnsAsync(player);

        var controller = new PlayerController(stubPlayerSearchService.Object, stubTeamProvider.Object);

        // Act
        var actual = await controller.FindPlayer(playerQuery, team.Abbreviation.Value);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<JsonResult>(actual);
    }
}