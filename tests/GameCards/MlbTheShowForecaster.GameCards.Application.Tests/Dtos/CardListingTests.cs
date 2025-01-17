using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos;

public class CardListingTests
{
    [Fact]
    public void HasNewPrices_DomainListingHasDifferentPrices_ReturnsTrue()
    {
        // Arrange
        var externalCardListing = Faker.FakeCardListing(bestBuyPrice: 10, bestSellPrice: 20);
        var domainListing = Tests.TestClasses.Faker.FakeListing(buyPrice: 1, sellPrice: 2);

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
        var domainListing = Tests.TestClasses.Faker.FakeListing(buyPrice: 10, sellPrice: 20);

        // Act
        var actual = externalCardListing.HasNewPrices(domainListing);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void HasNewHistoricalPrices_DomainListingThatIsNotUpToDate_ReturnsTrue()
    {
        // Arrange
        var externalCardListing = Faker.FakeCardListing(historicalPrices: new List<CardListingPrice>()
            {
                Faker.FakeCardListingPrice(new DateOnly(2024, 4, 1)),
                Faker.FakeCardListingPrice(new DateOnly(2024, 4, 2))
            }
        );
        var domainListing = Tests.TestClasses.Faker.FakeListing(historicalPrices: new List<ListingHistoricalPrice>()
            {
                Tests.TestClasses.Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1))
            }
        );

        // Act
        var actual = externalCardListing.HasNewHistoricalPrices(domainListing);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void HasNewHistoricalPrices_DomainListingThatIsUpToDate_ReturnsFalse()
    {
        // Arrange
        var externalCardListing = Faker.FakeCardListing(historicalPrices: new List<CardListingPrice>()
            {
                Faker.FakeCardListingPrice(new DateOnly(2024, 4, 1)),
                Faker.FakeCardListingPrice(new DateOnly(2024, 4, 2))
            }
        );
        var domainListing = Tests.TestClasses.Faker.FakeListing(historicalPrices: new List<ListingHistoricalPrice>()
            {
                Tests.TestClasses.Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1)),
                Tests.TestClasses.Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 2))
            }
        );

        // Act
        var actual = externalCardListing.HasNewHistoricalPrices(domainListing);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void HasNewOrders_DomainListingThatIsNotUpToDate_LargerQuantity_ReturnsTrue()
    {
        // Arrange
        var date = new DateTime(2025, 1, 16, 1, 2, 0, DateTimeKind.Utc);
        var externalCardListing = Faker.FakeCardListing(completedOrders: new List<CardListingOrder>()
            {
                Faker.FakeCompletedOrder(date, price: 1, quantity: 3)
            }
        );
        var domainListing = Tests.TestClasses.Faker.FakeListing(orders: new List<ListingOrder>()
            {
                Tests.TestClasses.Faker.FakeListingOrder(date, price: 1, quantity: 2)
            }
        );

        // Act
        var actual = externalCardListing.HasNewOrders(domainListing);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void HasNewOrders_DomainListingThatIsNotUpToDate_DifferentPrice_ReturnsTrue()
    {
        // Arrange
        var date = new DateTime(2025, 1, 16, 1, 2, 0, DateTimeKind.Utc);
        var externalCardListing = Faker.FakeCardListing(completedOrders: new List<CardListingOrder>()
            {
                Faker.FakeCompletedOrder(date, price: 1, quantity: 3)
            }
        );
        var domainListing = Tests.TestClasses.Faker.FakeListing(orders: new List<ListingOrder>()
            {
                Tests.TestClasses.Faker.FakeListingOrder(date, price: 2, quantity: 3)
            }
        );

        // Act
        var actual = externalCardListing.HasNewOrders(domainListing);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void HasNewOrders_DomainListingThatIsNotUpToDate_NewOrder_ReturnsTrue()
    {
        // Arrange
        var date1 = new DateTime(2025, 1, 16, 1, 2, 0, DateTimeKind.Utc);
        var date2 = date1.AddMinutes(1);
        var externalCardListing = Faker.FakeCardListing(completedOrders: new List<CardListingOrder>()
            {
                Faker.FakeCompletedOrder(date1, price: 1, quantity: 3),
                Faker.FakeCompletedOrder(date2, price: 1, quantity: 3)
            }
        );
        var domainListing = Tests.TestClasses.Faker.FakeListing(orders: new List<ListingOrder>()
            {
                Tests.TestClasses.Faker.FakeListingOrder(date1, price: 1, quantity: 3)
            }
        );

        // Act
        var actual = externalCardListing.HasNewOrders(domainListing);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void HasNewOrders_DomainListingThatIsUpToDate_ReturnsFalse()
    {
        // Arrange
        var date1 = new DateTime(2025, 1, 16, 1, 2, 0, DateTimeKind.Utc);
        var date2 = date1.AddMinutes(1);
        var externalCardListing = Faker.FakeCardListing(completedOrders: new List<CardListingOrder>()
            {
                Faker.FakeCompletedOrder(date1, price: 1, quantity: 3),
                Faker.FakeCompletedOrder(date2, price: 1, quantity: 3)
            }
        );
        var domainListing = Tests.TestClasses.Faker.FakeListing(orders: new List<ListingOrder>()
            {
                Tests.TestClasses.Faker.FakeListingOrder(date1, price: 1, quantity: 3),
                Tests.TestClasses.Faker.FakeListingOrder(date2, price: 1, quantity: 3),
            }
        );

        // Act
        var actual = externalCardListing.HasNewOrders(domainListing);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void GetNewHistoricalPrices_DomainListingThatIsNotUpToDate_ReturnsNewHistoricalPrices()
    {
        // Arrange
        var externalCardListing = Faker.FakeCardListing(historicalPrices: new List<CardListingPrice>()
            {
                Faker.FakeCardListingPrice(new DateOnly(2024, 4, 1)),
                Faker.FakeCardListingPrice(new DateOnly(2024, 4, 2))
            }
        );
        var domainListing = Tests.TestClasses.Faker.FakeListing(historicalPrices: new List<ListingHistoricalPrice>()
            {
                Tests.TestClasses.Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1))
            }
        );

        // Act
        var actual = externalCardListing.GetNewHistoricalPrices(domainListing);

        // Assert
        Assert.Single(actual);
        Assert.Equal(new DateOnly(2024, 4, 2), actual[0].Date);
    }

    [Fact]
    public void GetNewHistoricalPrices_DomainListingThatIsUpToDate_ReturnsEmptyCollection()
    {
        // Arrange
        var externalCardListing = Faker.FakeCardListing(historicalPrices: new List<CardListingPrice>()
            {
                Faker.FakeCardListingPrice(new DateOnly(2024, 4, 1)),
                Faker.FakeCardListingPrice(new DateOnly(2024, 4, 2))
            }
        );
        var domainListing = Tests.TestClasses.Faker.FakeListing(historicalPrices: new List<ListingHistoricalPrice>()
            {
                Tests.TestClasses.Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1)),
                Tests.TestClasses.Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 2))
            }
        );

        // Act
        var actual = externalCardListing.GetNewHistoricalPrices(domainListing);

        // Assert
        Assert.Empty(actual);
    }

    [Fact]
    public void GetNewOrders_DomainListingThatIsNotUpToDate_LargerQuantity_ReturnsNewOrders()
    {
        // Arrange
        var date = new DateTime(2025, 1, 16, 1, 2, 0, DateTimeKind.Utc);
        var externalCardListing = Faker.FakeCardListing(completedOrders: new List<CardListingOrder>()
            {
                Faker.FakeCompletedOrder(date, price: 1, quantity: 3)
            }
        );
        var domainListing = Tests.TestClasses.Faker.FakeListing(orders: new List<ListingOrder>()
            {
                Tests.TestClasses.Faker.FakeListingOrder(date, price: 1, quantity: 2)
            }
        );

        // Act
        var actual = externalCardListing.GetNewOrders(domainListing);

        // Assert
        Assert.Single(actual);
        Assert.Equal(Faker.FakeCompletedOrder(date, price: 1, quantity: 3), actual[0]);
    }

    [Fact]
    public void GetNewOrders_DomainListingThatIsNotUpToDate_DifferentPrice_ReturnsNewOrders()
    {
        // Arrange
        var date = new DateTime(2025, 1, 16, 1, 2, 0, DateTimeKind.Utc);
        var externalCardListing = Faker.FakeCardListing(completedOrders: new List<CardListingOrder>()
            {
                Faker.FakeCompletedOrder(date, price: 1, quantity: 3)
            }
        );
        var domainListing = Tests.TestClasses.Faker.FakeListing(orders: new List<ListingOrder>()
            {
                Tests.TestClasses.Faker.FakeListingOrder(date, price: 2, quantity: 3)
            }
        );

        // Act
        var actual = externalCardListing.GetNewOrders(domainListing);

        // Assert
        Assert.Single(actual);
        Assert.Equal(Faker.FakeCompletedOrder(date, price: 1, quantity: 3), actual[0]);
    }

    [Fact]
    public void GetNewOrders_DomainListingThatIsNotUpToDate_NewOrder_ReturnsNewOrders()
    {
        // Arrange
        var date1 = new DateTime(2025, 1, 16, 1, 2, 0, DateTimeKind.Utc);
        var date2 = date1.AddMinutes(1);
        var externalCardListing = Faker.FakeCardListing(completedOrders: new List<CardListingOrder>()
            {
                Faker.FakeCompletedOrder(date1, price: 1, quantity: 3),
                Faker.FakeCompletedOrder(date2, price: 2, quantity: 4)
            }
        );
        var domainListing = Tests.TestClasses.Faker.FakeListing(orders: new List<ListingOrder>()
            {
                Tests.TestClasses.Faker.FakeListingOrder(date1, price: 1, quantity: 3)
            }
        );

        // Act
        var actual = externalCardListing.GetNewOrders(domainListing);

        // Assert
        Assert.Single(actual);
        Assert.Equal(Faker.FakeCompletedOrder(date2, price: 2, quantity: 4), actual[0]);
    }

    [Fact]
    public void GetNewOrders_DomainListingThatIsUpToDate_ReturnsEmptyCollection()
    {
        // Arrange
        var date1 = new DateTime(2025, 1, 16, 1, 2, 0, DateTimeKind.Utc);
        var date2 = date1.AddMinutes(1);
        var externalCardListing = Faker.FakeCardListing(completedOrders: new List<CardListingOrder>()
            {
                Faker.FakeCompletedOrder(date1, price: 1, quantity: 3),
                Faker.FakeCompletedOrder(date2, price: 2, quantity: 4)
            }
        );
        var domainListing = Tests.TestClasses.Faker.FakeListing(orders: new List<ListingOrder>()
            {
                Tests.TestClasses.Faker.FakeListingOrder(date1, price: 1, quantity: 3),
                Tests.TestClasses.Faker.FakeListingOrder(date2, price: 2, quantity: 4),
            }
        );

        // Act
        var actual = externalCardListing.GetNewOrders(domainListing);

        // Assert
        Assert.Empty(actual);
    }
}