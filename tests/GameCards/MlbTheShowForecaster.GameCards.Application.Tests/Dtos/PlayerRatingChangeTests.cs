using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos;

public class PlayerRatingChangeTests
{
    [Fact]
    public void Improved_HigherNewRating_ReturnsTrue()
    {
        // Arrange
        var playerRatingChange = Faker.FakePlayerRatingChange(newOverallRating: 90, oldOverallRating: 80);

        // Act
        var actual = playerRatingChange.Improved;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void Improved_LowerNewRating_ReturnsFalse()
    {
        // Arrange
        var playerRatingChange = Faker.FakePlayerRatingChange(newOverallRating: 80, oldOverallRating: 90);

        // Act
        var actual = playerRatingChange.Improved;

        // Assert
        Assert.False(actual);
    }
}