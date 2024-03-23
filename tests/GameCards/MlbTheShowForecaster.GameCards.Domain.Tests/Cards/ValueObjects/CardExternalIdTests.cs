using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.ValueObjects;

public class CardExternalIdTests
{
    [Fact]
    public void Create_EmptyExternalId_ThrowsException()
    {
        // Arrange
        const string externalId = "";
        var action = () => CardExternalId.Create(externalId);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<EmptyCardExternalIdException>(actual);
    }

    [Fact]
    public void Create_ValidExternalId_ReturnsCardExternalId()
    {
        // Arrange
        const string externalId = "id1";

        // Act
        var actual = CardExternalId.Create(externalId);

        // Assert
        Assert.Equal("id1", actual.Value);
    }
}