using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
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

    [Fact]
    public void IsApplied_PlayerCardWithRatingApplied_ReturnsTrue()
    {
        // Arrange
        var date = new DateOnly(2024, 4, 1);
        var startDate = new DateOnly(2024, 4, 1);
        var endDate = new DateOnly(2024, 4, 2);
        var card = Tests.TestClasses.Faker.FakePlayerCard();
        card.AddHistoricalRating(Tests.TestClasses.Faker.FakeBaselinePlayerCardHistoricalRating(startDate, endDate));
        var ratingChange = new PlayerRatingChange { Date = date };

        // Act
        var actual = ratingChange.IsApplied(card);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void IsApplied_PlayerCardWithoutRatingApplied_ReturnsFalse()
    {
        // Arrange
        var date = new DateOnly(2024, 4, 1);
        var card = Tests.TestClasses.Faker.FakePlayerCard();
        var ratingChange = new PlayerRatingChange { Date = date };

        // Act
        var actual = ratingChange.IsApplied(card);

        // Assert
        Assert.False(actual);
    }
}