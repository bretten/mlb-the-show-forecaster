using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Listings;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Exceptions;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Listings;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.RosterUpdates;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.Listings;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.RosterUpdates;
using Refit;

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
    /// Options
    /// </summary>
    private readonly FakeMlbTheShowApiOptions _options;

    /// <summary>
    /// Fallback API, which can be a mock server
    /// </summary>
    private readonly IMlbTheShowApi _api;

    /// <summary>
    /// True if all player cards specified in <see cref="FakeMlbTheShowApiOptions.PlayerCardFilter"/> have been found
    /// </summary>
    private bool _foundAllFilteredPlayerCards;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="year">Which year the client corresponds to</param>
    /// <param name="options">Options</param>
    public FakeMlbTheShowApi(Year year, FakeMlbTheShowApiOptions options)
    {
        _year = year;
        _options = options;

        _season = ((int)year).ToString();
        _api = RestService.For<IMlbTheShowApi>(
            Resiliency.ResilientClient(GetUrl(), new ResponseFilterDelegatingHandler(new HttpClientHandler(), options)),
            new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(
                    new JsonSerializerOptions()
                    {
                        Converters = { new JsonStringEnumConverter() }
                    }
                ),
            }
        );
    }

    /// <inheritdoc />
    public async Task<ItemDto> GetItem(GetItemRequest request)
    {
        if (!_options.UseLocalFiles)
        {
            return await _api.GetItem(request);
        }

        var json = await File.ReadAllTextAsync(Paths.Card(Paths.Fakes, _season, request.Uuid));
        var response = JsonSerializer.Deserialize<ItemDto>(json)!;
        return await Task.FromResult(response);
    }

    /// <inheritdoc />
    public async Task<GetItemsPaginatedResponse> GetItems(GetItemsRequest request)
    {
        if (_options.PlayerCardFilter != null)
        {
            return await FilterPlayers();
        }

        if (!_options.UseLocalFiles)
        {
            return await _api.GetItems(request);
        }

        // When reading selected cards, everything is on the first page, so stop on the 2nd page
        // if (request.Page == 2)
        // {
        //     return await Task.FromResult(new GetItemsPaginatedResponse(2, 0, 2, new List<ItemDto>()));
        // }
        // var json = await File.ReadAllTextAsync(Paths.SelectedCards(Paths.Fakes, _season));

        var json = await File.ReadAllTextAsync(Paths.PagedCards(Paths.Fakes, _season, request.Page.ToString()));
        var response = JsonSerializer.Deserialize<GetItemsPaginatedResponse>(json)!;
        return await Task.FromResult(response);
    }

    /// <inheritdoc />
    public async Task<ListingDto<ItemDto>> GetListing(GetListingRequest request)
    {
        if (!_options.UseLocalFiles)
        {
            return await _api.GetListing(request);
        }

        var json = await File.ReadAllTextAsync(Paths.Listing(Paths.Fakes, _season, request.Uuid));
        var response = JsonSerializer.Deserialize<ListingDto<ItemDto>>(json)!;
        return await Task.FromResult(response);
    }

    /// <inheritdoc />
    public async Task<GetListingsPaginatedResponse> GetListings(GetListingsRequest request)
    {
        if (!_options.UseLocalFiles)
        {
            return await _api.GetListings(request);
        }

        var json = await File.ReadAllTextAsync(Paths.PagedListings(Paths.Fakes, _season, request.Page.ToString()));
        var response = JsonSerializer.Deserialize<GetListingsPaginatedResponse>(json)!;
        return await Task.FromResult(response);
    }

    /// <inheritdoc />
    public async Task<GetRosterUpdateResponse> GetRosterUpdate(GetRosterUpdateRequest request)
    {
        if (!_options.UseLocalFiles)
        {
            return await _api.GetRosterUpdate(request);
        }

        var json = await File.ReadAllTextAsync(Paths.RosterUpdate(Paths.Fakes, _season, request.Id.ToString()));
        var response = JsonSerializer.Deserialize<GetRosterUpdateResponse>(json)!;
        return await Task.FromResult(response);
    }

    /// <inheritdoc />
    public async Task<GetRosterUpdatesResponse> GetRosterUpdates()
    {
        if (!_options.UseLocalFiles)
        {
            return await _api.GetRosterUpdates();
        }

        var json = await File.ReadAllTextAsync(Paths.RosterUpdateList(Paths.Fakes, _season));
        var response = JsonSerializer.Deserialize<GetRosterUpdatesResponse>(json)!;
        return await Task.FromResult(response);
    }

    /// <summary>
    /// Requests player cards from the fallback API until all player cards in the filter are found
    /// </summary>
    /// <returns>The filtered response</returns>
    private async Task<GetItemsPaginatedResponse> FilterPlayers()
    {
        // If the filter has finished finding all the players, make the next page empty to indicate that it is done
        if (_foundAllFilteredPlayerCards)
        {
            _foundAllFilteredPlayerCards = false;
            return await Task.FromResult(new GetItemsPaginatedResponse(2, 25, 2, new List<ItemDto>()));
        }

        // First page
        var pageOneResponse = await _api.GetItems(new GetItemsRequest(1, ItemType.MlbCard));

        // Queue to represent the different pages to request
        var queue = new ConcurrentQueue<int>();
        for (var i = 2; i <= pageOneResponse.TotalPages; i++)
        {
            queue.Enqueue(i);
        }

        // Check each page for player cards specified in the filter until all are found
        GetItemsPaginatedResponse? filteredResponse = null;
        await Parallel.ForEachAsync(queue, new ParallelOptions { MaxDegreeOfParallelism = 5 }, async (page, ct) =>
        {
            // Check if all players in the filter have been found
            if (_foundAllFilteredPlayerCards)
            {
                // Rather than canceling the task and having to wrap the parallel operation in a try-catch, simply short-circuit any remaining items
                return;
            }

            var r = await _api.GetItems(new GetItemsRequest(page, ItemType.MlbCard));
            // The filtering delegating handler will change TotalPages to 1 when all cards in the filter have been found
            if (r.TotalPages == 1)
            {
                _foundAllFilteredPlayerCards = true;
                filteredResponse = r;
            }
        });

        if (filteredResponse == null)
        {
            throw new Exception($"{nameof(FilterPlayers)} could not finish filtering");
        }

        return filteredResponse;
    }

    /// <summary>
    /// Gets the URL that the fallback <see cref="_api"/> should use
    /// </summary>
    /// <returns>URL for the fallback <see cref="_api"/></returns>
    private string GetUrl()
    {
        if (!string.IsNullOrWhiteSpace(_options.BaseAddress))
        {
            return _options.BaseAddress;
        }

        return _year switch
        {
            Year.Season2021 => Constants.BaseUrl2021,
            Year.Season2022 => Constants.BaseUrl2022,
            Year.Season2023 => Constants.BaseUrl2023,
            Year.Season2024 => Constants.BaseUrl2024,
            _ => throw new UnsupportedMlbTheShowYearException($"MLB The Show does not support the year {_year}")
        };
    }
}