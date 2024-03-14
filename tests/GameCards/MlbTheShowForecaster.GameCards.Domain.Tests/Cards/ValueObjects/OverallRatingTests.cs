using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.ValueObjects;

public class OverallRatingTests
{
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