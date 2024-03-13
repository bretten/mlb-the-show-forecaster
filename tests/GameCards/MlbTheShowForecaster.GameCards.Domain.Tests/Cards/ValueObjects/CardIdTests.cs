using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.ValueObjects;

public class CardIdTests
{
    [Fact]
    public void Create_EmptyCardId_ThrowsException()
    {
        // Arrange
        const string cardId = "";
        var action = () => CardId.Create(cardId);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<EmptyCardIdException>(actual);
    }

    [Fact]
    public void Create_ValidCardId_ReturnsCardId()
    {
        // Arrange
        const string cardId = "id1";

        // Act
        var actual = CardId.Create(cardId);

        // Assert
        Assert.Equal("id1", actual.Value);
    }
}