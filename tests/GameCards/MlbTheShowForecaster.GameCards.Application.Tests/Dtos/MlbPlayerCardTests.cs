using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos;

public class MlbPlayerCardTests
{
    [Theory]
    [InlineData(Rarity.Common, 4)]
    [InlineData(Rarity.Bronze, 3)]
    [InlineData(Rarity.Silver, 2)]
    [InlineData(Rarity.Gold, 1)]
    [InlineData(Rarity.Diamond, 0)]
    public void Priority_RarityEnum_ReturnsRarityEnumInteger(Rarity rarity, int expected)
    {
        // Arrange
        var mlbPlayerCard = Faker.FakeMlbPlayerCard(rarity: rarity);

        // Act
        var actual = mlbPlayerCard.Priority;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(CardSeries.Live, true)]
    [InlineData(CardSeries.Rookie, false)]
    public void IsSupported_CardSeries_ReturnsTrueIfSupported(CardSeries series, bool expected)
    {
        // Arrange
        var mlbPlayerCard = Faker.FakeMlbPlayerCard(series: series);

        // Act
        var actual = mlbPlayerCard.IsSupported;

        // Assert
        Assert.Equal(expected, actual);
    }
}