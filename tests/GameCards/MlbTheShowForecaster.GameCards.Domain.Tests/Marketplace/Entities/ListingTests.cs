using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Marketplace.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Marketplace.Entities;

public class ListingTests
{
    [Fact]
    public void HistoricalPricesChronologically_MultipleHistoricalPrices_ReturnsHistoricalPricesInOrderOfDate()
    {
        // Arrange
        var listing = Faker.FakeListing();
        listing.LogHistoricalPrice(new DateOnly(2024, 4, 2), buyPrice: NaturalNumber.Create(2),
            sellPrice: NaturalNumber.Create(2));
        listing.LogHistoricalPrice(new DateOnly(2024, 4, 1), buyPrice: NaturalNumber.Create(1),
            sellPrice: NaturalNumber.Create(1));

        // Act
        var actual = listing.HistoricalPricesChronologically;

        // Assert
        Assert.Equal(2, actual.Count);
        Assert.Equal(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1), 1, 1), actual[0]);
        Assert.Equal(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 2), 2, 2), actual[1]);
    }

    [Fact]
    public void OrdersChronologically_MultipleOrders_ReturnsOrdersInOrderOfDate()
    {
        // Arrange
        var listing = Faker.FakeListing();
        listing.UpdateOrders(new List<ListingOrder>()
        {
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 3, 0, DateTimeKind.Utc)), // 2
            Faker.FakeListingOrder(new DateTime(2025, 1, 18, 1, 2, 0, DateTimeKind.Utc)), // 3
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc)), // 1
        });

        // Act
        var actual = listing.OrdersChronologically;

        // Assert
        Assert.Equal(3, actual.Count);
        Assert.Equal(new List<ListingOrder>()
        {
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc)),
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 3, 0, DateTimeKind.Utc)),
            Faker.FakeListingOrder(new DateTime(2025, 1, 18, 1, 2, 0, DateTimeKind.Utc)),
        }, actual);
    }

    [Fact]
    public void PriceFor_DateHasPrices_ReturnsPrices()
    {
        // Arrange
        var listing = Faker.FakeListing();
        listing.LogHistoricalPrice(new DateOnly(2025, 1, 23), buyPrice: NaturalNumber.Create(2),
            sellPrice: NaturalNumber.Create(3));

        // Act
        var actual = listing.PriceFor(new DateOnly(2025, 1, 23));

        // Assert
        Assert.Equal(Faker.FakeListingHistoricalPrice(new DateOnly(2025, 1, 23), 2, 3), actual);
    }

    [Fact]
    public void PriceFor_DateHasNoPrices_ReturnsNull()
    {
        // Arrange
        var listing = Faker.FakeListing();
        listing.LogHistoricalPrice(new DateOnly(2025, 1, 23), buyPrice: NaturalNumber.Create(2),
            sellPrice: NaturalNumber.Create(3));

        // Act
        var actual = listing.PriceFor(new DateOnly(2025, 1, 24));

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void TotalOrdersFor_DateWithOrders_ReturnsTotalOrderQuantity()
    {
        // Arrange
        var listing = Faker.FakeListing();
        listing.UpdateOrders(new List<ListingOrder>()
        {
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 3, 0, DateTimeKind.Utc)),
            Faker.FakeListingOrder(new DateTime(2025, 1, 18, 1, 2, 0, DateTimeKind.Utc)),
            Faker.FakeListingOrder(new DateTime(2025, 1, 18, 1, 2, 0, DateTimeKind.Utc)),
            Faker.FakeListingOrder(new DateTime(2025, 1, 18, 1, 2, 0, DateTimeKind.Utc)),
            Faker.FakeListingOrder(new DateTime(2025, 1, 18, 1, 2, 0, DateTimeKind.Utc)),
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc)),
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc)),
        });

        // Act
        var actual = listing.TotalOrdersFor(new DateOnly(2025, 1, 17));

        // Assert
        Assert.Equal(3, actual.Value);
    }

    [Fact]
    public void TotalOrdersFor_DateTimeRange_ReturnsTotalOrderQuantity()
    {
        /*
         * Arrange
         */
        var listing = Faker.FakeListing();
        var ordersToAdd = new List<ListingOrder>();
        // Not in range
        for (int i = 0; i < 2; i++)
        {
            ordersToAdd.Add(Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc)));
        }

        // In range
        for (int i = 0; i < 1; i++)
        {
            ordersToAdd.Add(Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 3, 0, DateTimeKind.Utc)));
        }

        // In range
        for (int i = 0; i < 23; i++)
        {
            ordersToAdd.Add(Faker.FakeListingOrder(new DateTime(2025, 1, 18, 1, 1, 0, DateTimeKind.Utc)));
        }

        // Not in range
        for (int i = 0; i < 5; i++)
        {
            ordersToAdd.Add(Faker.FakeListingOrder(new DateTime(2025, 1, 18, 1, 2, 0, DateTimeKind.Utc)));
        }

        listing.UpdateOrders(ordersToAdd);
        var start = new DateTime(2025, 1, 17, 1, 3, 0, DateTimeKind.Utc);
        var end = new DateTime(2025, 1, 18, 1, 1, 0, DateTimeKind.Utc);

        // Act
        var actual = listing.TotalOrdersFor(start, end);

        // Assert
        Assert.Equal(24, actual.Value);
    }

    [Fact]
    public void LogHistoricalPrice_DateAlreadyExists_ThrowsException()
    {
        // Arrange
        var date = new DateOnly(2024, 4, 1);
        var buyPrice = NaturalNumber.Create(1);
        var sellPrice = NaturalNumber.Create(2);
        var listing = Faker.FakeListing();
        listing.LogHistoricalPrice(date, buyPrice: buyPrice, sellPrice: sellPrice);
        var action = () => listing.LogHistoricalPrice(date, buyPrice: buyPrice, sellPrice: sellPrice);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<ListingHistoricalPriceExistsException>(actual);
    }

    [Fact]
    public void LogHistoricalPrice_DateAndPrices_AddsPricesToHistoricalCollectionForDate()
    {
        // Arrange
        var date = new DateOnly(2024, 4, 1);
        var listing = Faker.FakeListing(buyPrice: 1, sellPrice: 1);

        var expected = Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1), 1, 1);

        // Act
        listing.LogHistoricalPrice(date, buyPrice: NaturalNumber.Create(1), sellPrice: NaturalNumber.Create(1));

        // Assert
        Assert.Equal(1, listing.HistoricalPricesChronologically.Count);
        Assert.Equal(expected, listing.HistoricalPricesChronologically[0]);
    }

    [Fact]
    public void UpdatePrice_NewPrices_ChangesPricesOnListing()
    {
        // Arrange
        var listing = Faker.FakeListing(buyPrice: 1, sellPrice: 1);

        // Act
        listing.UpdatePrices(NaturalNumber.Create(2), NaturalNumber.Create(3),
            Mock.Of<IListingPriceSignificantChangeThreshold>());

        // Assert
        Assert.Equal(2, listing.BuyPrice.Value);
        Assert.Equal(3, listing.SellPrice.Value);
    }

    [Fact]
    public void UpdatePrice_BuyPriceIncreasedSignificantly_RaisesBuyPriceIncreasedDomainEvent()
    {
        // Arrange
        var listing = Faker.FakeListing(buyPrice: 10, sellPrice: 10);
        var stubPriceChangeThreshold = new Mock<IListingPriceSignificantChangeThreshold>();
        stubPriceChangeThreshold.Setup(x => x.BuyPricePercentageChangeThreshold)
            .Returns(50);

        // Act
        listing.UpdatePrices(NaturalNumber.Create(20), NaturalNumber.Create(10), stubPriceChangeThreshold.Object);

        // Assert
        Assert.Single(listing.DomainEvents);
        Assert.IsType<ListingBuyPriceIncreasedEvent>(listing.DomainEvents[0]);
        var e = listing.DomainEvents[0] as ListingBuyPriceIncreasedEvent;
        Assert.Equal(10, e!.OriginalPrice.Value);
        Assert.Equal(20, e.NewPrice.Value);
        Assert.Equal(100, e.PercentageChange.PercentageChangeValue);
    }

    [Fact]
    public void UpdatePrice_BuyPriceDecreasedSignificantly_RaisesBuyPriceDecreasedDomainEvent()
    {
        // Arrange
        var listing = Faker.FakeListing(buyPrice: 20, sellPrice: 10);
        var stubPriceChangeThreshold = new Mock<IListingPriceSignificantChangeThreshold>();
        stubPriceChangeThreshold.Setup(x => x.BuyPricePercentageChangeThreshold)
            .Returns(50);

        // Act
        listing.UpdatePrices(NaturalNumber.Create(10), NaturalNumber.Create(10), stubPriceChangeThreshold.Object);

        // Assert
        Assert.Single(listing.DomainEvents);
        Assert.IsType<ListingBuyPriceDecreasedEvent>(listing.DomainEvents[0]);
        var e = listing.DomainEvents[0] as ListingBuyPriceDecreasedEvent;
        Assert.Equal(20, e!.OriginalPrice.Value);
        Assert.Equal(10, e.NewPrice.Value);
        Assert.Equal(-50m, e.PercentageChange.PercentageChangeValue);
    }

    [Fact]
    public void UpdatePrice_BuyPriceHadNegligibleChange_NoDomainEventRaised()
    {
        // Arrange
        var listing = Faker.FakeListing(buyPrice: 10, sellPrice: 10);
        var stubPriceChangeThresholds = new Mock<IListingPriceSignificantChangeThreshold>();
        stubPriceChangeThresholds.Setup(x => x.BuyPricePercentageChangeThreshold)
            .Returns(50);

        // Act
        listing.UpdatePrices(NaturalNumber.Create(11), NaturalNumber.Create(10), stubPriceChangeThresholds.Object);

        // Assert
        Assert.Empty(listing.DomainEvents);
    }

    [Fact]
    public void UpdatePrice_SellPriceIncreasedSignificantly_RaisesSellPriceIncreasedDomainEvent()
    {
        // Arrange
        var listing = Faker.FakeListing(buyPrice: 10, sellPrice: 10);
        var stubPriceChangeThreshold = new Mock<IListingPriceSignificantChangeThreshold>();
        stubPriceChangeThreshold.Setup(x => x.SellPricePercentageChangeThreshold)
            .Returns(50);

        // Act
        listing.UpdatePrices(NaturalNumber.Create(10), NaturalNumber.Create(20), stubPriceChangeThreshold.Object);

        // Assert
        Assert.Single(listing.DomainEvents);
        Assert.IsType<ListingSellPriceIncreasedEvent>(listing.DomainEvents[0]);
        var e = listing.DomainEvents[0] as ListingSellPriceIncreasedEvent;
        Assert.Equal(10, e!.OriginalPrice.Value);
        Assert.Equal(20, e.NewPrice.Value);
        Assert.Equal(100, e.PercentageChange.PercentageChangeValue);
    }

    [Fact]
    public void UpdatePrice_SellPriceDecreasedSignificantly_RaisesSellPriceDecreasedDomainEvent()
    {
        // Arrange
        var listing = Faker.FakeListing(buyPrice: 10, sellPrice: 20);
        var stubPriceChangeThreshold = new Mock<IListingPriceSignificantChangeThreshold>();
        stubPriceChangeThreshold.Setup(x => x.SellPricePercentageChangeThreshold)
            .Returns(50);

        // Act
        listing.UpdatePrices(NaturalNumber.Create(10), NaturalNumber.Create(10), stubPriceChangeThreshold.Object);

        // Assert
        Assert.Single(listing.DomainEvents);
        Assert.IsType<ListingSellPriceDecreasedEvent>(listing.DomainEvents[0]);
        var e = listing.DomainEvents[0] as ListingSellPriceDecreasedEvent;
        Assert.Equal(20, e!.OriginalPrice.Value);
        Assert.Equal(10, e.NewPrice.Value);
        Assert.Equal(-50m, e.PercentageChange.PercentageChangeValue);
    }

    [Fact]
    public void UpdatePrice_SellPriceHadNegligibleChange_NoDomainEventRaised()
    {
        // Arrange
        var listing = Faker.FakeListing(buyPrice: 10, sellPrice: 10);
        var stubPriceChangeThresholds = new Mock<IListingPriceSignificantChangeThreshold>();
        stubPriceChangeThresholds.Setup(x => x.SellPricePercentageChangeThreshold)
            .Returns(50);

        // Act
        listing.UpdatePrices(NaturalNumber.Create(10), NaturalNumber.Create(11), stubPriceChangeThresholds.Object);

        // Assert
        Assert.Empty(listing.DomainEvents);
    }

    [Fact]
    public void Create_ValidValues_ReturnsListing()
    {
        // Arrange
        var cardExternalId = Cards.TestClasses.Faker.FakeCardExternalId(Cards.TestClasses.Faker.FakeGuid1);
        const int buyPrice = 10;
        const int sellPrice = 20;
        var fakeHistoricalPrice = Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1),
            buyPrice: 1, sellPrice: 2);

        // Act
        var actual = Listing.Create(cardExternalId,
            buyPrice: NaturalNumber.Create(buyPrice),
            sellPrice: NaturalNumber.Create(sellPrice),
            new List<ListingHistoricalPrice>()
            {
                fakeHistoricalPrice
            });

        // Assert
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), actual.CardExternalId.Value);
        Assert.Equal(10, actual.BuyPrice.Value);
        Assert.Equal(20, actual.SellPrice.Value);
        Assert.Single(actual.HistoricalPricesChronologically);
        Assert.Equal(new DateOnly(2024, 4, 1), actual.HistoricalPricesChronologically[0].Date);
        Assert.Equal(1, actual.HistoricalPricesChronologically[0].BuyPrice.Value);
        Assert.Equal(2, actual.HistoricalPricesChronologically[0].SellPrice.Value);
    }
}