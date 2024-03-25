using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.ValueObjects;

public class OverallRatingTests
{
    [Theory]
    [InlineData(40, Rarity.Common)]
    [InlineData(64, Rarity.Common)]
    [InlineData(65, Rarity.Bronze)]
    [InlineData(74, Rarity.Bronze)]
    [InlineData(75, Rarity.Silver)]
    [InlineData(79, Rarity.Silver)]
    [InlineData(80, Rarity.Gold)]
    [InlineData(84, Rarity.Gold)]
    [InlineData(85, Rarity.Diamond)]
    [InlineData(99, Rarity.Diamond)]
    public void Rarity_RatingRarityEndpoint_ReturnsCorrespondingRarity(int rating, Rarity expected)
    {
        // Arrange
        var overallRating = OverallRating.Create(rating);

        // Act
        var actual = overallRating.Rarity;
        actual = overallRating.Rarity; // Code coverage for property-backing field

        // Assert
        Assert.Equal(actual, expected);
    }

    [Fact]
    public void Create_BelowMinValue_ThrowsException()
    {
        // Arrange
        const int rating = 39;
        var action = () => OverallRating.Create(rating);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<OverallRatingOutOfRangeException>(actual);
    }

    [Fact]
    public void Create_AboveMaxValue_ThrowsException()
    {
        // Arrange
        const int rating = 100;
        var action = () => OverallRating.Create(rating);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<OverallRatingOutOfRangeException>(actual);
    }

    [Fact]
    public void Create_ValidValue_ReturnsOverallRating()
    {
        // Arrange
        const int rating = 99;

        // Act
        var actual = OverallRating.Create(rating);

        // Assert
        Assert.Equal(99, actual.Value);
    }
}