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
    public async Task GetPlayersBySeason_2023RegularSeason_RequestsAllPlayers()
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
        var actualPlayer = actual.Players![1094];
        Assert.Equal(677594, actualPlayer.Id);
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
}