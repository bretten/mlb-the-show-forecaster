using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.ValueObjects;

public class AbilityAttributeTests
{
    [Fact]
    public void Create_BelowMinValue_ThrowsException()
    {
        // Arrange
        const int abilityRating = -1;
        var action = () => AbilityAttribute.Create(abilityRating);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<AbilityAttributeOutOfRangeException>(actual);
    }

    [Fact]
    public void Create_AboveMaxValue_ThrowsException()
    {
        // Arrange
        const int abilityRating = 126;
        var action = () => AbilityAttribute.Create(abilityRating);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<AbilityAttributeOutOfRangeException>(actual);
    }

    [Fact]
    public void Create_ValidValue_ReturnsAbilityAttribute()
    {
        // Arrange
        const int abilityRating = 100;

        // Act
        var actual = AbilityAttribute.Create(abilityRating);

        // Assert
        Assert.Equal(100, actual.Value);
    }
}