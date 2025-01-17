using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Listings;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;

/// <summary>
/// Thrown when a <see cref="ListingOrderDto"/> has an invalid date string format
/// </summary>
public sealed class InvalidTheShowListingOrderDateFormatException : Exception
{
    public InvalidTheShowListingOrderDateFormatException(string? message) : base(message)
    {
    }
}