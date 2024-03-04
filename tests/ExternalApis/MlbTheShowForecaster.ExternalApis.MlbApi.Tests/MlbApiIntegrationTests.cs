using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Tests;

public class MlbApiIntegrationTests
{
    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetPlayersBySeason_2023RegularSeason_ReturnsAllPlayers()
    {
        // Arrange
        var request = new GetPlayersBySeasonRequest(2023, GameType.RegularSeason);
        var mlbApi = RestService.For<IMlbApi>(Constants.BaseUrl,
            new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(
                    new JsonSerializerOptions()
                    {
                        Converters = { new JsonStringEnumConverter() }
                    }
                )
            });

        // Act
        var actual = await mlbApi.GetPlayersBySeason(request);

        // Assert
        Assert.NotNull(actual.Players);
        Assert.Equal(1457, actual.Players?.Count);
        var actualPlayer = actual.Players!.First(x => x.Id == 677594);
        Assert.Equal("Julio", actualPlayer.FirstName);
        Assert.Equal("Rodríguez", actualPlayer.LastName);
        Assert.Equal(new DateTime(2000, 12, 29), actualPlayer.Birthdate);
        Assert.Equal(new PositionDto("Outfielder", "CF"), actualPlayer.Position);
        Assert.Equal(new DateTime(2022, 4, 8), actualPlayer.MlbDebutDate);
        Assert.Equal(new ArmSideDto("R", "Right"), actualPlayer.BatSide);
        Assert.Equal(new ArmSideDto("R", "Right"), actualPlayer.ThrowArm);
        Assert.Equal(new CurrentTeamDto(136), actualPlayer.CurrentTeam);
        Assert.True(actualPlayer.Active);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetPlayerSeasonStatsByGame_PlayerWithAllStatsAnd2023_ReturnsStatsForPlayer()
    {
        // Arrange
        var request = new GetPlayerSeasonStatsByGameRequest(660271, 2023);
        var mlbApi = RestService.For<IMlbApi>(Constants.BaseUrl,
            new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(
                    new JsonSerializerOptions()
                    {
                        Converters = { new JsonStringEnumConverter() }
                    }
                )
            });

        // Act
        var actual = await mlbApi.GetPlayerSeasonStatsByGame(request);

        // Assert
        Assert.NotNull(actual);
        Assert.NotNull(actual.People);
        Assert.Single(actual.People);
        Assert.Equal(660271, actual.People[0].Id);
        Assert.Equal("Shohei", actual.People[0].FirstName);
        Assert.Equal("Ohtani", actual.People[0].LastName);
        Assert.Equal(3, actual.People[0].Stats.Count);
    }
}