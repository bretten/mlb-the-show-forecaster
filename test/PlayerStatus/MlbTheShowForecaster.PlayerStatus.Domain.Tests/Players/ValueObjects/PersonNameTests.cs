using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Players.ValueObjects;

public class PersonNameTests
{
    [Fact]
    public void Create_EmptyName_ThrowsException()
    {
        // Arrange
        var action = () => PersonName.Create("");

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<EmptyPersonNameException>(actual);
    }

    [Fact]
    public void Create_ValidName_Created()
    {
        // Arrange
        var name = "Brett";

        // Act
        var actual = PersonName.Create(name);

        // Assert
        Assert.Equal("Brett", actual.Value);
    }
}