using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos;

public class CardListingTests
{
    [Fact]
    public void HasNewPrices_DomainListingHasDifferentPrices_ReturnsTrue()
    {
        // Arrange
        var externalCardListing = Faker.FakeCardListing(bestBuyPrice: 10, bestSellPrice: 20);
        var domainListing = Domain.Tests.Marketplace.TestClasses.Faker.FakeListing(buyPrice: 1, sellPrice: 2);

        // Act
        var actual = externalCardListing.HasNewPrices(domainListing);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void HasNewPrices_DomainListingHasSamePrices_ReturnsFalse()
    {
        // Arrange
        var externalCardListing = Faker.FakeCardListing(bestBuyPrice: 10, bestSellPrice: 20);
        var domainListing = Domain.Tests.Marketplace.TestClasses.Faker.FakeListing(buyPrice: 10, sellPrice: 20);

        // Act
        var actual = externalCardListing.HasNewPrices(domainListing);

        // Assert
        Assert.False(actual);
    }
}