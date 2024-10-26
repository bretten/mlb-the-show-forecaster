using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
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
    /// The season string
    /// </summary>
    private readonly string _season;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="year">Which year the client corresponds to</param>
    public FakeMlbTheShowApi(Year year)
    {
        _year = year;
        _season = ((int)year).ToString();
    }

    /// <inheritdoc />
    public Task<ItemDto> GetItem(GetItemRequest request)
    {
        var json = File.ReadAllText(Paths.Card(Paths.Fakes, _season, request.Uuid));
        var response = JsonSerializer.Deserialize<ItemDto>(json)!;
        return Task.FromResult(response);
    }

    /// <inheritdoc />
    public Task<GetItemsPaginatedResponse> GetItems(GetItemsRequest request)
    {
        //var json = File.ReadAllText(Paths.PagedCards(Paths.Fakes, _season, request.Page.ToString()));

        // When reading selected cards, everything is on the first page, so stop on the 2nd page
        if (request.Page == 2)
        {
            return Task.FromResult(new GetItemsPaginatedResponse(2, 0, 2, new List<ItemDto>()));
        }

        var json = File.ReadAllText(Paths.SelectedCards(Paths.Fakes, _season));
        var response = JsonSerializer.Deserialize<GetItemsPaginatedResponse>(json)!;
        return Task.FromResult(response);
    }

    /// <inheritdoc />
    public Task<ListingDto<ItemDto>> GetListing(GetListingRequest request)
    {
        var json = File.ReadAllText(Paths.Listing(Paths.Fakes, _season, request.Uuid));
        var response = JsonSerializer.Deserialize<ListingDto<ItemDto>>(json)!;
        return Task.FromResult(response);
    }

    /// <inheritdoc />
    public Task<GetListingsPaginatedResponse> GetListings(GetListingsRequest request)
    {
        var json = File.ReadAllText(Paths.PagedListings(Paths.Fakes, _season, request.Page.ToString()));
        var response = JsonSerializer.Deserialize<GetListingsPaginatedResponse>(json)!;
        return Task.FromResult(response);
    }

    /// <inheritdoc />
    public Task<GetRosterUpdateResponse> GetRosterUpdate(GetRosterUpdateRequest request)
    {
        var json = File.ReadAllText(Paths.RosterUpdate(Paths.Fakes, _season, request.Id.ToString()));
        var response = JsonSerializer.Deserialize<GetRosterUpdateResponse>(json)!;
        return Task.FromResult(response);
    }

    /// <inheritdoc />
    public Task<GetRosterUpdatesResponse> GetRosterUpdates()
    {
        var json = File.ReadAllText(Paths.RosterUpdateList(Paths.Fakes, _season));
        var response = JsonSerializer.Deserialize<GetRosterUpdatesResponse>(json)!;
        return Task.FromResult(response);
    }
}