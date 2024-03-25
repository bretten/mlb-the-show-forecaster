using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities.Exceptions;

/// <summary>
/// Thrown when <see cref="Listing"/> tries to archive a <see cref="ListingHistoricalPrice"/> for a date it already has
/// historical prices for
/// </summary>
public sealed class ListingHistoricalPriceExistsException : Exception
{
    public ListingHistoricalPriceExistsException(string? message) : base(message)
    {
    }
}