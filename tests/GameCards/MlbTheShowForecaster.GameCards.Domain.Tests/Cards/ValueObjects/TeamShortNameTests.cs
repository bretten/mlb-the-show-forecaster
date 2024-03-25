using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.ValueObjects;

public class TeamShortNameTests
{
    [Fact]
    public void Create_EmptyTeamShortName_ThrowsException()
    {
        // Arrange
        const string teamShortName = "";
        var action = () => TeamShortName.Create(teamShortName);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<EmptyTeamShortNameException>(actual);
    }

    [Fact]
    public void Create_ValidTeamShortName_ReturnsTeamShortName()
    {
        // Arrange
        const string teamShortName = "SEA";

        // Act
        var actual = TeamShortName.Create(teamShortName);

        // Assert
        Assert.Equal("SEA", actual.Value);
    }
}