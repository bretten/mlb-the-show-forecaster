using System.Diagnostics.CodeAnalysis;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Listings;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Listings;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.RosterUpdates;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.Listings;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.RosterUpdates;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Fakes;

/// <summary>
/// Fake of <see cref="IMlbTheShowApi"/>
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class FakeMlbTheShowApi : IMlbTheShowApi
{
    /// <summary>
    /// Which year the client corresponds to
    /// </summary>
    private readonly Year _year;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="year">Which year the client corresponds to</param>
    public FakeMlbTheShowApi(Year year)
    {
        _year = year;
    }

    /// <inheritdoc />
    public Task<ItemDto> GetItem(GetItemRequest request)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<GetItemsPaginatedResponse> GetItems(GetItemsRequest request)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ListingDto<ItemDto>> GetListing(GetListingRequest request)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<GetListingsPaginatedResponse> GetListings(GetListingsRequest request)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<GetRosterUpdateResponse> GetRosterUpdate(GetRosterUpdateRequest request)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<GetRosterUpdatesResponse> GetRosterUpdates()
    {
        throw new NotImplementedException();
    }
}