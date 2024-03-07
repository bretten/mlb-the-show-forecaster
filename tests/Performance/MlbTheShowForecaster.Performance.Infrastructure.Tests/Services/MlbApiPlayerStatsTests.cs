using Castle.Components.DictionaryAdapter;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Responses;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Services.Exceptions;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Tests.Services;

public class MlbApiPlayerStatsTests
{
    [Fact]
    public async Task GetPlayerSeason_UnknownPlayerMlbIdAndSeason_ThrowsException()
    {
        // Arrange
        var playerMlbId = MlbId.Create(1);
        var season = SeasonYear.Create(2024);
        var request = new GetPlayerSeasonStatsByGameRequest(playerMlbId.Value, season.Value);
        var stubMlbAPi = new Mock<IMlbApi>();
        stubMlbAPi.Setup(x => x.GetPlayerSeasonStatsByGame(request))
            .ReturnsAsync(new GetPlayerSeasonStatsByGameResponse(new List<PlayerSeasonStatsByGameDto>()));
        var mockMapper = Mock.Of<IMlbApiPlayerStatsMapper>();

        var service = new MlbApiPlayerStats(stubMlbAPi.Object, mockMapper);
        var action = async () => await service.GetPlayerSeason(playerMlbId, season);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<PlayerSeasonNotFoundException>(actual);
    }

    [Fact]
    public async Task GetPlayerSeason_KnownPlayerMlbIdAndSeason_ReturnsPlayerSeason()
    {
        // Arrange
        var playerMlbId = MlbId.Create(1);
        var season = SeasonYear.Create(2024);
        var request = new GetPlayerSeasonStatsByGameRequest(playerMlbId.Value, season.Value);
        var playerSeasonStats =
            new PlayerSeasonStatsByGameDto(playerMlbId.Value, "First", "Last", new List<StatsDto>());

        var stubMlbAPi = new Mock<IMlbApi>();
        stubMlbAPi.Setup(x => x.GetPlayerSeasonStatsByGame(request))
            .ReturnsAsync(new GetPlayerSeasonStatsByGameResponse(new List<PlayerSeasonStatsByGameDto>()
            {
                playerSeasonStats
            }));

        var playerSeason = new PlayerSeason(playerMlbId, season, new List<PlayerGameBattingStats>(),
            new List<PlayerGamePitchingStats>(), new EditableList<PlayerGameFieldingStats>());
        var stubMapper = new Mock<IMlbApiPlayerStatsMapper>();
        stubMapper.Setup(x => x.Map(playerSeasonStats))
            .Returns(playerSeason);

        var service = new MlbApiPlayerStats(stubMlbAPi.Object, stubMapper.Object);

        // Act
        var actual = await service.GetPlayerSeason(playerMlbId, season);

        // Assert
        Assert.Equal(1, actual.PlayerMlbId.Value);
        Assert.Equal(2024, actual.SeasonYear.Value);
        Assert.Empty(actual.GameBattingStats);
        Assert.Empty(actual.GamePitchingStats);
        Assert.Empty(actual.GameFieldingStats);
    }
}