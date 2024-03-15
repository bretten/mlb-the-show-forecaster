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
    public static Listing FakeListing(string cardId = "1", int buyPrice = 0, int sellPrice = 0)
    {
        return Listing.Create(CardId.Create(cardId), NaturalNumber.Create(buyPrice), NaturalNumber.Create(sellPrice));
    }

    public static ListingHistoricalPrice FakeListingHistoricalPrice(DateOnly date, int buyPrice = 0, int sellPrice = 0)
    {
        return ListingHistoricalPrice.Create(date, NaturalNumber.Create(buyPrice), NaturalNumber.Create(sellPrice));
    }
}