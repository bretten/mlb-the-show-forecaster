using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums.Extensions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.Enums.Extensions;

public class RarityExtensionsTests
{
    [Theory]
    [InlineData(Rarity.Diamond, Rarity.Gold)]
    [InlineData(Rarity.Gold, Rarity.Silver)]
    [InlineData(Rarity.Silver, Rarity.Bronze)]
    [InlineData(Rarity.Bronze, Rarity.Common)]
    public void GreaterThan_LeftRarityIsBetter_ReturnsTrue(Rarity left, Rarity right)
    {
        // Assemble

        // Act
        var actual = left.GreaterThan(right);

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [InlineData(Rarity.Diamond, Rarity.Diamond)]
    [InlineData(Rarity.Gold, Rarity.Gold)]
    [InlineData(Rarity.Silver, Rarity.Silver)]
    [InlineData(Rarity.Bronze, Rarity.Bronze)]
    [InlineData(Rarity.Common, Rarity.Common)]
    public void GreaterThan_SameRarity_ReturnsFalse(Rarity left, Rarity right)
    {
        // Assemble

        // Act
        var actual = left.GreaterThan(right);

        // Assert
        Assert.False(actual);
    }

    [Theory]
    [InlineData(Rarity.Common, Rarity.Bronze)]
    [InlineData(Rarity.Bronze, Rarity.Silver)]
    [InlineData(Rarity.Silver, Rarity.Gold)]
    [InlineData(Rarity.Gold, Rarity.Diamond)]
    public void LessThan_LeftRarityIsWorse_ReturnsTrue(Rarity left, Rarity right)
    {
        // Assemble

        // Act
        var actual = left.LessThan(right);

        // Assert
        Assert.True(actual);
    }

    [Theory]
    [InlineData(Rarity.Diamond, Rarity.Diamond)]
    [InlineData(Rarity.Gold, Rarity.Gold)]
    [InlineData(Rarity.Silver, Rarity.Silver)]
    [InlineData(Rarity.Bronze, Rarity.Bronze)]
    [InlineData(Rarity.Common, Rarity.Common)]
    public void LessThan_SameRarity_ReturnsFalse(Rarity left, Rarity right)
    {
        // Assemble

        // Act
        var actual = left.LessThan(right);

        // Assert
        Assert.False(actual);
    }
}