using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Responses;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Mapping;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Tests.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Tests.Services;

public class MlbApiPlayerRosterTests
{
    [Fact]
    public async Task GetRosterEntries_SeasonWithNoPlayers_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2023);
        var fakeMlbApiRequest = new GetPlayersBySeasonRequest(seasonYear.Value, GameType.RegularSeason);
        var fakeMlbApiResponse = new GetPlayersBySeasonResponse(new List<PlayerDto>());
        var stubMlbApi = Mock.Of<IMlbApi>(x =>
            x.GetPlayersBySeason(fakeMlbApiRequest) == Task.FromResult(fakeMlbApiResponse));
        var mockPlayerMapper = Mock.Of<IMlbApiPlayerMapper>();
        var roster = new MlbApiPlayerRoster(stubMlbApi, mockPlayerMapper);
        var action = async () => await roster.GetRosterEntries(seasonYear, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Mock.Get(stubMlbApi).Verify(x => x.GetPlayersBySeason(fakeMlbApiRequest), Times.Once);
        Assert.NotNull(actual);
        Assert.IsType<EmptyRosterException>(actual);
    }

    [Fact]
    public async Task GetRosterEntries_SeasonHasPlayers_ReturnsPlayers()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2023);

        // Fake roster entries to be returned from the service under test
        var fakeRosterEntry1 = Faker.FakeRosterEntry(1);
        var fakeRosterEntry2 = Faker.FakeRosterEntry(2);
        var fakeRosterEntry3 = Faker.FakeRosterEntry(3);
        var expectedRosterEntries = new List<RosterEntry>() { fakeRosterEntry1, fakeRosterEntry2, fakeRosterEntry3 };

        // Fake player DTOs from the MLB API
        var fakePlayer1 = Faker.FakePlayerDto(1);
        var fakePlayer2 = Faker.FakePlayerDto(2);
        var fakePlayer3 = Faker.FakePlayerDto(3);
        var players = new List<PlayerDto>() { fakePlayer1, fakePlayer2, fakePlayer3 };

        // Mock MLB API behavior to return the players
        var fakeMlbApiRequest = new GetPlayersBySeasonRequest(seasonYear.Value, GameType.RegularSeason);
        var fakeMlbApiResponse = new GetPlayersBySeasonResponse(players);
        var stubMlbApi = Mock.Of<IMlbApi>(x =>
            x.GetPlayersBySeason(fakeMlbApiRequest) == Task.FromResult(fakeMlbApiResponse));

        // Mock mapping of player to roster entry
        var stubPlayerMapper = new Mock<IMlbApiPlayerMapper>();
        stubPlayerMapper.Setup(x => x.Map(fakePlayer1)).Returns(fakeRosterEntry1);
        stubPlayerMapper.Setup(x => x.Map(fakePlayer2)).Returns(fakeRosterEntry2);
        stubPlayerMapper.Setup(x => x.Map(fakePlayer3)).Returns(fakeRosterEntry3);

        // Service under test
        var roster = new MlbApiPlayerRoster(stubMlbApi, stubPlayerMapper.Object);

        /*
         * Act
         */
        var actual = await roster.GetRosterEntries(seasonYear, cToken);

        /*
         * Assert
         */
        // Verify the MLB API was invoked
        Mock.Get(stubMlbApi).Verify(x => x.GetPlayersBySeason(fakeMlbApiRequest), Times.Once);
        // Verify mapping took place
        stubPlayerMapper.Verify(x => x.Map(fakePlayer1), Times.Once);
        stubPlayerMapper.Verify(x => x.Map(fakePlayer2), Times.Once);
        stubPlayerMapper.Verify(x => x.Map(fakePlayer3), Times.Once);
        // Verify the roster entries were returned
        Assert.NotNull(actual);
        Assert.Equal(expectedRosterEntries, actual);
    }
}