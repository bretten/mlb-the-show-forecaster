using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
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

    [Theory]
    [InlineData("AZ", TeamInfo.AZ)]
    [InlineData("ATL", TeamInfo.ATL)]
    [InlineData("BAL", TeamInfo.BAL)]
    [InlineData("BOS", TeamInfo.BOS)]
    [InlineData("CHC", TeamInfo.CHC)]
    [InlineData("CIN", TeamInfo.CIN)]
    [InlineData("CLE", TeamInfo.CLE)]
    [InlineData("COL", TeamInfo.COL)]
    [InlineData("CWS", TeamInfo.CWS)]
    [InlineData("DET", TeamInfo.DET)]
    [InlineData("HOU", TeamInfo.HOU)]
    [InlineData("KC", TeamInfo.KC)]
    [InlineData("LAA", TeamInfo.LAA)]
    [InlineData("LAD", TeamInfo.LAD)]
    [InlineData("MIA", TeamInfo.MIA)]
    [InlineData("MIL", TeamInfo.MIL)]
    [InlineData("MIN", TeamInfo.MIN)]
    [InlineData("NYM", TeamInfo.NYM)]
    [InlineData("NYY", TeamInfo.NYY)]
    [InlineData("OAK", TeamInfo.OAK)]
    [InlineData("PHI", TeamInfo.PHI)]
    [InlineData("PIT", TeamInfo.PIT)]
    [InlineData("SD", TeamInfo.SD)]
    [InlineData("SEA", TeamInfo.SEA)]
    [InlineData("SF", TeamInfo.SF)]
    [InlineData("STL", TeamInfo.STL)]
    [InlineData("TB", TeamInfo.TB)]
    [InlineData("TEX", TeamInfo.TEX)]
    [InlineData("TOR", TeamInfo.TOR)]
    [InlineData("WSH", TeamInfo.WSH)]
    public void GetBy_Name_ReturnsTeam(string team, TeamInfo expectedResult)
    {
        // Arrange
        var provider = new TeamProvider();
        var expectedTeam = Team.Create(expectedResult);

        // Act
        var actual = provider.GetBy(team);

        // Assert
        Assert.Equal(expectedTeam, actual);
    }

    [Fact]
    public void GetBy_UnknownTeam_ReturnsNull()
    {
        // Arrange
        var provider = new TeamProvider();
        const string unknownTeam = "team";

        // Act
        var actual = provider.GetBy(unknownTeam);

        // Assert
        Assert.Null(actual);
    }
}