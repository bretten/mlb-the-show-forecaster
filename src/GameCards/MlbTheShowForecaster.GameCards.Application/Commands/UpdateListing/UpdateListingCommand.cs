using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdateListing;

/// <summary>
/// Command that updates a <see cref="Listing"/>
/// </summary>
/// <param name="DomainListing">The <see cref="Listing"/> to update</param>
/// <param name="ExternalCardListing">The new data to update the <see cref="Listing"/> with</param>
/// <param name="PriceChangeThreshold">The percentage change threshold that determines significant listing price changes</param>
internal readonly record struct UpdateListingCommand(
    Listing DomainListing,
    CardListing ExternalCardListing,
    IListingPriceSignificantChangeThreshold PriceChangeThreshold
) : ICommand;