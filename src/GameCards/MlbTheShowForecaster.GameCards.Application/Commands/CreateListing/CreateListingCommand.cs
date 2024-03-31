using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreateListing;

/// <summary>
/// Command that creates a <see cref="Listing"/>
/// </summary>
/// <param name="ExternalCardListing">Details about the listing</param>
internal readonly record struct CreateListingCommand(
    CardListing ExternalCardListing
) : ICommand;