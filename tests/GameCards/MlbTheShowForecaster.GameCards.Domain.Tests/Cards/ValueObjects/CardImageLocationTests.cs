using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.ValueObjects;

public class CardImageLocationTests
{
    [Fact]
    public void Create_EmptyCardImage_ThrowsException()
    {
        // Arrange
        const string cardImage = "";
        var action = () => CardImageLocation.Create(cardImage);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<EmptyCardImageException>(actual);
    }

    [Fact]
    public void Create_ValidCardImage_ReturnsCardImage()
    {
        // Arrange
        const string cardImage = "/assets/main.jpg";

        // Act
        var actual = CardImageLocation.Create(cardImage);

        // Assert
        Assert.Equal("/assets/main.jpg", actual.Value.OriginalString);
    }
}