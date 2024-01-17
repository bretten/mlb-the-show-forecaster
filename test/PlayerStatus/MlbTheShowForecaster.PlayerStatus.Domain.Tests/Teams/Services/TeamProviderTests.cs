using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Teams.Services;

public class TeamProviderTests
{
    [Fact]
    public void GetBy_UnknownMlbId_ThrowsException()
    {
        // Arrange
        var provider = new TeamProvider();
        var mlbId = MlbId.Create(1);
        var action = () => provider.GetBy(mlbId);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<UnknownTeamMlbIdException>(actual);
    }

    [Fact]
    public void GetBy_ValidMlbId_ReturnsTeam()
    {
        // Arrange
        var provider = new TeamProvider();
        var mlbId = MlbId.Create((int)TeamInfo.SEA);

        // Act
        var actual = provider.GetBy(mlbId);

        // Assert
        Assert.Equal(136, actual.MlbId.Value);
        Assert.Equal("Seattle Mariners", actual.Name.Value);
        Assert.Equal("SEA", actual.Abbreviation.Value);
    }

    [Fact]
    public void GetBy_UnknownTeamAbbreviation_ThrowsException()
    {
        // Arrange
        var provider = new TeamProvider();
        var abbreviation = TeamAbbreviation.Create("DOG");
        var action = () => provider.GetBy(abbreviation);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<UnknownTeamAbbreviationException>(actual);
    }

    [Fact]
    public void GetBy_ValidTeamAbbreviation_ReturnsTeam()
    {
        // Arrange
        var provider = new TeamProvider();
        var abbreviation = TeamAbbreviation.Create(TeamInfo.SEA.ToString());

        // Act
        var actual = provider.GetBy(abbreviation);

        // Assert
        Assert.Equal(136, actual.MlbId.Value);
        Assert.Equal("Seattle Mariners", actual.Name.Value);
        Assert.Equal("SEA", actual.Abbreviation.Value);
    }
}