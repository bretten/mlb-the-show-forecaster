using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.ValueObjects;

public class CardNameTests
{
    [Fact]
    public void Create_EmptyName_ThrowsException()
    {
        // Arrange
        const string name = "";
        var action = () => CardName.Create(name);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<EmptyCardNameException>(actual);
    }

    [Fact]
    public void Create_ValidName_ReturnsName()
    {
        // Arrange
        const string name = "Dot";

        // Act
        var actual = CardName.Create(name);

        // Assert
        Assert.Equal("Dot", actual.Value);
    }
}