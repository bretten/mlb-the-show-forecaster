using Refit;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Listings;

/// <summary>
/// Request for getting an Item's Listing
/// </summary>
/// <param name="Uuid">The ID of the Listing's Item</param>
public sealed record GetListingRequest(
    [property: AliasAs("uuid")]
    string Uuid
);