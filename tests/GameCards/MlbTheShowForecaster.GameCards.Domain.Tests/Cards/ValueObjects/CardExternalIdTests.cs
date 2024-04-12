using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.ValueObjects;

public class CardExternalIdTests
{
    [Fact]
    public void ValueStringDigits_ValidGuid_ReturnsGuidAsStringWithoutHyphens()
    {
        // Arrange
        var externalId = CardExternalId.Create(new Guid("00000000-0000-0000-0000-000000000001"));

        // Act
        var actual = externalId.ValueStringDigits;

        // Assert
        Assert.Equal("00000000000000000000000000000001", actual);
    }

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
    public void Create_InvalidGuidString_ThrowsException()
    {
        // Arrange
        const string externalId = "abc";
        var action = () => CardExternalId.Create(externalId);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidCardExternalIdException>(actual);
    }

    [Fact]
    public void Create_ValidGuid_ReturnsCardExternalId()
    {
        // Arrange
        var externalId = new Guid("00000000-0000-0000-0000-000000000001");

        // Act
        var actual = CardExternalId.Create(externalId);

        // Assert
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), actual.Value);
    }
}