using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Listings;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;

/// <summary>
/// Thrown when a <see cref="ListingPriceDto"/> has an invalid date string format
/// </summary>
public sealed class InvalidTheShowListingPriceDateFormatException : Exception
{
    public InvalidTheShowListingPriceDateFormatException(string? message) : base(message)
    {
    }
}