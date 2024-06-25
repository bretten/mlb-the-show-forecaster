using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Repositories;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Players.TestClasses;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Teams.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Players.Services;

public class PlayerSearchServiceTests
{
    [Fact]
    public async Task FindPlayer_NameAndTeamWithMultipleMatches_ThrowsException()
    {
        // Arrange
        const string name = "Dot Spot Jr. III";
        var team = TeamFaker.Fake();

        var match1 = PlayerFaker.Fake(firstName: "Dot Spot", lastName: "Jr. III", team: team);
        var match2 = PlayerFaker.Fake(firstName: "Dot", lastName: "Spot Jr. III", team: team);

        var stubPlayerRepository = new Mock<IPlayerRepository>();
        stubPlayerRepository.Setup(x => x.GetAllByName(match1.FirstName, match1.LastName))
            .ReturnsAsync(new List<Player>() { match1 });
        stubPlayerRepository.Setup(x => x.GetAllByName(match2.FirstName, match2.LastName))
            .ReturnsAsync(new List<Player>() { match2 });

        var service = new PlayerSearchService(stubPlayerRepository.Object);
        var action = () => service.FindPlayer(name, team);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<PlayerSearchCouldNotBeRefinedException>(actual);
    }

    [Fact]
    public async Task FindPlayer_NameAndTeam_ReturnsMatchedPlayer()
    {
        // Arrange
        const string name = "Dot Spot Jr. III";
        var team = TeamFaker.Fake();

        var match = PlayerFaker.Fake(firstName: "Dot Spot", lastName: "Jr. III", team: team);

        var stubPlayerRepository = new Mock<IPlayerRepository>();
        stubPlayerRepository.Setup(x => x.GetAllByName(match.FirstName, match.LastName))
            .ReturnsAsync(new List<Player>() { match });

        var service = new PlayerSearchService(stubPlayerRepository.Object);

        // Act
        var actual = await service.FindPlayer(name, team);

        // Assert
        Assert.Equal(match, actual);
        stubPlayerRepository.Verify(x => x.GetAllByName(PersonName.Create("Dot"), PersonName.Create("Spot Jr. III")),
            Times.Once);
        stubPlayerRepository.Verify(x => x.GetAllByName(PersonName.Create("Dot Spot"), PersonName.Create("Jr. III")),
            Times.Once);
        stubPlayerRepository.Verify(x => x.GetAllByName(PersonName.Create("Dot Spot Jr."), PersonName.Create("III")),
            Times.Once);
    }
}