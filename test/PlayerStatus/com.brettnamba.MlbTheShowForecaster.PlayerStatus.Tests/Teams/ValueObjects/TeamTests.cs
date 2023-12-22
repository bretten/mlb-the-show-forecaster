using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Tests.Teams.ValueObjects;

public class TeamTests
{
    [Fact]
    public void Create_ValidValues_Created()
    {
        // Arrange
        var mlbId = MlbId.Create(1);
        var teamName = TeamName.Create("Seattle Mariners");
        var abbreviation = TeamAbbreviation.Create("SEA");

        // Act
        var actual = Team.Create(mlbId, teamName, abbreviation);

        // Assert
        Assert.Equal(1, actual.MlbId.Value);
        Assert.Equal("Seattle Mariners", actual.Name.Value);
        Assert.Equal("SEA", actual.Abbreviation.Value);
    }
}