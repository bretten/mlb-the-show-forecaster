using System.Collections.Concurrent;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Responses;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Tests.Dtos.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Tests.Services;

public class MlbApiPlayerStatsTests
{
    [Fact]
    public async Task GetAllPlayerStatsFor_NoStatsInSeason_ThrowsException()
    {
        // Arrange
        var season = SeasonYear.Create(2024);

        var fakeMlbApiRequest = new GetPlayersBySeasonRequest(season.Value, GameType.RegularSeason);
        var fakeMlbApiResponse = new GetPlayersBySeasonResponse(new List<PlayerDto>());
        var stubMlbApi = new Mock<IMlbApi>();
        stubMlbApi.Setup(x => x.GetPlayersBySeason(fakeMlbApiRequest))
            .ReturnsAsync(fakeMlbApiResponse);

        var mockMapper = Mock.Of<IMlbApiPlayerStatsMapper>();

        var service = new MlbApiPlayerStats(stubMlbApi.Object, mockMapper);

        var action = async () =>
        {
            await foreach (var p in service.GetAllPlayerStatsFor(season, CancellationToken.None))
            {
            }
        };

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<NoPlayerSeasonsFoundException>(actual);
    }

    [Fact]
    public async Task GetAllPlayerStatsFor_UnknownPlayerMlbIdAndSeason_ThrowsException()
    {
        // Arrange
        var fakePlayer = Faker.FakePlayerDto(1);
        var season = SeasonYear.Create(2024);

        var getPlayersRequest = new GetPlayersBySeasonRequest(season.Value, GameType.RegularSeason);

        var getStatsRequest = new GetPlayerSeasonStatsByGameRequest(fakePlayer.Id, season.Value);

        var stubMlbApi = new Mock<IMlbApi>();
        stubMlbApi.Setup(x => x.GetPlayersBySeason(getPlayersRequest))
            .ReturnsAsync(new GetPlayersBySeasonResponse(new List<PlayerDto>() { fakePlayer }));
        stubMlbApi.Setup(x => x.GetPlayerSeasonStatsByGame(getStatsRequest))
            .ReturnsAsync(new GetPlayerSeasonStatsByGameResponse(new List<PlayerSeasonStatsByGameDto>()
            {
                new()
            }));
        var mockMapper = Mock.Of<IMlbApiPlayerStatsMapper>();

        var service = new MlbApiPlayerStats(stubMlbApi.Object, mockMapper);
        var action = async () =>
        {
            await foreach (var p in service.GetAllPlayerStatsFor(season, CancellationToken.None))
            {
            }
        };

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<PlayerSeasonNotFoundException>(actual);
    }

    [Fact]
    public async Task GetAllPlayerStatsFor_Season_ReturnsPlayerStatsForSeason()
    {
        // Arrange
        var season = SeasonYear.Create(2024);

        var fakePlayer1 = Faker.FakePlayerDto(1);
        var fakePlayer2 = Faker.FakePlayerDto(2);
        var fakePlayer3 = Faker.FakePlayerDto(3);
        var players = new List<PlayerDto>() { fakePlayer1, fakePlayer2, fakePlayer3 };

        var fakePlayer1Season = Application.Tests.Dtos.TestClasses.Faker.FakePlayerSeason(fakePlayer1.Id);
        var fakePlayer2Season = Application.Tests.Dtos.TestClasses.Faker.FakePlayerSeason(fakePlayer2.Id);
        var fakePlayer3Season = Application.Tests.Dtos.TestClasses.Faker.FakePlayerSeason(fakePlayer3.Id);

        var getPlayersRequest = new GetPlayersBySeasonRequest(season.Value, GameType.RegularSeason);

        var getPlayer1StatsRequest = new GetPlayerSeasonStatsByGameRequest(fakePlayer1.Id, season.Value);
        var getPlayer2StatsRequest = new GetPlayerSeasonStatsByGameRequest(fakePlayer2.Id, season.Value);
        var getPlayer3StatsRequest = new GetPlayerSeasonStatsByGameRequest(fakePlayer3.Id, season.Value);

        var stubMlbApi = new Mock<IMlbApi>();
        stubMlbApi.Setup(x => x.GetPlayersBySeason(getPlayersRequest))
            .ReturnsAsync(new GetPlayersBySeasonResponse(players));
        stubMlbApi.Setup(x => x.GetPlayerSeasonStatsByGame(getPlayer1StatsRequest))
            .ReturnsAsync(new GetPlayerSeasonStatsByGameResponse(new List<PlayerSeasonStatsByGameDto>()
            {
                new(fakePlayer1.Id, "First1", "Last1", new List<StatsDto>())
            }));
        stubMlbApi.Setup(x => x.GetPlayerSeasonStatsByGame(getPlayer2StatsRequest))
            .ReturnsAsync(new GetPlayerSeasonStatsByGameResponse(new List<PlayerSeasonStatsByGameDto>()
            {
                new(fakePlayer2.Id, "First2", "Last2", new List<StatsDto>())
            }));
        stubMlbApi.Setup(x => x.GetPlayerSeasonStatsByGame(getPlayer3StatsRequest))
            .ReturnsAsync(new GetPlayerSeasonStatsByGameResponse(new List<PlayerSeasonStatsByGameDto>()
            {
                new(fakePlayer3.Id, "First3", "Last3", new List<StatsDto>())
            }));

        var stubMapper = new Mock<IMlbApiPlayerStatsMapper>();
        stubMapper.Setup(x => x.Map(It.Is<PlayerSeasonStatsByGameDto>(p => p.Id == fakePlayer1.Id)))
            .Returns(fakePlayer1Season);
        stubMapper.Setup(x => x.Map(It.Is<PlayerSeasonStatsByGameDto>(p => p.Id == fakePlayer2.Id)))
            .Returns(fakePlayer2Season);
        stubMapper.Setup(x => x.Map(It.Is<PlayerSeasonStatsByGameDto>(p => p.Id == fakePlayer3.Id)))
            .Returns(fakePlayer3Season);

        var service = new MlbApiPlayerStats(stubMlbApi.Object, stubMapper.Object);

        // Act
        var actual = new ConcurrentBag<PlayerSeason>();
        await foreach (var p in service.GetAllPlayerStatsFor(season, CancellationToken.None))
        {
            actual.Add(p);
        }

        // Assert
        Assert.Contains(fakePlayer1Season, actual);
        Assert.Contains(fakePlayer2Season, actual);
        Assert.Contains(fakePlayer3Season, actual);
    }
}