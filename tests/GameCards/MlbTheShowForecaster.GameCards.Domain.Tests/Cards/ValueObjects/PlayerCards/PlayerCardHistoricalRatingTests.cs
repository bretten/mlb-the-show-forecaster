using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.ValueObjects.PlayerCards;

public class PlayerCardHistoricalRatingTests
{
    [Fact]
    public void IsBaseline_TypeBaseline_ReturnsTrue()
    {
        // Arrange
        var rating = Faker.FakeBaselinePlayerCardHistoricalRating();

        // Act
        var actual = rating.IsBaseline;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void IsTemporary_TypeTemp_ReturnsTrue()
    {
        // Arrange
        var rating = Faker.FakeTemporaryPlayerCardHistoricalRating();

        // Act
        var actual = rating.IsTemporary;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void IsBoost_TypeBoost_ReturnsTrue()
    {
        // Arrange
        var rating = Faker.FakeBoostPlayerCardHistoricalRating();

        // Act
        var actual = rating.IsBoost;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void End_AlreadyEnded_ThrowsException()
    {
        // Arrange
        var rating = Faker.FakeBaselinePlayerCardHistoricalRating();
        rating.End(new DateOnly(2024, 5, 29));

        var action = () => rating.End(new DateOnly(2024, 5, 29)); // Repeat the end

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<PlayerCardHistoricalRatingAlreadyEndedException>(actual);
    }

    [Fact]
    public void End_NoEndDate_SetsEndDate()
    {
        // Arrange
        var rating = Faker.FakeBaselinePlayerCardHistoricalRating();

        // Act
        rating.End(new DateOnly(2024, 5, 29));

        // Assert
        Assert.Equal(new DateOnly(2024, 5, 29), rating.EndDate);
    }
}