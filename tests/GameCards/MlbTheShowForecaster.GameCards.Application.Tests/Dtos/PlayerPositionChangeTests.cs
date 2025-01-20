using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos;

public class PlayerPositionChangeTests
{
    [Fact]
    public void IsApplied_PlayerCardWithPositionApplied_ReturnsTrue()
    {
        // Arrange
        var card = Faker.FakePlayerCard(position: Position.Shortstop);
        var positionChange = new PlayerPositionChange(new DateOnly(2024, 10, 28), card.ExternalId, Position.Shortstop);

        // Act
        var actual = positionChange.IsApplied(card);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void IsApplied_PlayerCardWithoutPositionApplied_ReturnsFalse()
    {
        // Arrange
        var card = Faker.FakePlayerCard(position: Position.ThirdBase);
        var positionChange = new PlayerPositionChange(new DateOnly(2024, 10, 28), card.ExternalId, Position.Shortstop);

        // Act
        var actual = positionChange.IsApplied(card);

        // Assert
        Assert.False(actual);
    }
}