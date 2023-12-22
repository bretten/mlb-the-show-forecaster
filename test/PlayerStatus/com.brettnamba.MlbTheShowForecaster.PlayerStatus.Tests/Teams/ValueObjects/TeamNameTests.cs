using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Tests.Teams.ValueObjects;

public class TeamNameTests
{
    [Fact]
    public void Create_EmptyName_ThrowsException()
    {
        // Arrange
        var action = () => TeamName.Create("");

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<EmptyTeamNameException>(actual);
    }

    [Fact]
    public void Create_ValidName_Created()
    {
        // Arrange
        var name = "Seattle Mariners";

        // Act
        var actual = TeamName.Create(name);

        // Assert
        Assert.Equal("Seattle Mariners", actual.Value);
    }
}