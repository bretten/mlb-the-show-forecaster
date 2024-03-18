using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Marketplace.ValueObjects;

public class ListingPriceSignificantChangeThresholdTests
{
    [Fact]
    public void Create_ListingPricePercentageChangeThresholds_ReturnsThresholds()
    {
        // Arrange
        const decimal buyPricePercentageChangeThreshold = 15m;
        const decimal sellPricePercentageChangeThreshold = 20m;

        // Act
        var actual = ListingPriceSignificantChangeThreshold.Create(
            buyPricePercentageChangeThreshold: buyPricePercentageChangeThreshold,
            sellPricePercentageChangeThreshold: sellPricePercentageChangeThreshold
        );

        // Assert
        Assert.Equal(15m, actual.BuyPricePercentageChangeThreshold);
        Assert.Equal(20m, actual.SellPricePercentageChangeThreshold);
    }
}