using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Marketplace.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static Listing FakeListing(Guid? cardExternalId = null, int buyPrice = 0, int sellPrice = 0)
    {
        return Listing.Create(CardExternalId.Create(cardExternalId ?? Cards.TestClasses.Faker.FakeGuid1),
            NaturalNumber.Create(buyPrice), NaturalNumber.Create(sellPrice));
    }

    public static ListingHistoricalPrice FakeListingHistoricalPrice(DateOnly date, int buyPrice = 0, int sellPrice = 0)
    {
        return ListingHistoricalPrice.Create(date, NaturalNumber.Create(buyPrice), NaturalNumber.Create(sellPrice));
    }

    public static ListingOrder FakeListingOrder(DateTime? date = null, int price = 0, int quantity = 0)
    {
        return ListingOrder.Create(date ?? new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc),
            price: NaturalNumber.Create(price),
            quantity: NaturalNumber.Create(quantity)
        );
    }
}