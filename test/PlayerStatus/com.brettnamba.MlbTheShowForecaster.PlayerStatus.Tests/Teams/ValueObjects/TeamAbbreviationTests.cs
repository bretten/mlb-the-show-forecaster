using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Tests.Teams.ValueObjects;

public class TeamAbbreviationTests
{
    [Fact]
    public void Create_LessThanTwoCharacters_ThrowsException()
    {
        // Arrange
        var action = () => TeamAbbreviation.Create("A");

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidTeamAbbreviationException>(actual);
    }

    [Fact]
    public void Create_GreaterThanThreeCharacters_ThrowsException()
    {
        // Arrange
        var action = () => TeamAbbreviation.Create("ABCD");

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidTeamAbbreviationException>(actual);
    }

    [Fact]
    public void Create_BetweenTwoAndThreeCharacters_Created()
    {
        // Arrange
        string expected = "SEA";

        // Act
        var actual = TeamAbbreviation.Create("SEA");

        // Assert
        Assert.Equal(expected, actual.Value);
    }
}