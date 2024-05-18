using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdateListing.Exceptions;

/// <summary>
/// Thrown when <see cref="UpdateListingCommandHandler"/> cannot find the specified <see cref="Listing"/> to update
/// </summary>
public sealed class ListingNotFoundException : Exception
{
    public ListingNotFoundException(string? message) : base(message)
    {
    }
}