using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.Items;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi;

/// <summary>
/// MLB The Show API - https://mlb24.theshow.com/apis/docs/item (subdomains exist for mlb21 - mlb24)
///
/// <para>Allows integration with the MLB The Show API so that Items, Listings and Roster Updates can be retrieved</para>
/// </summary>
public interface IMlbTheShowApi
{
    [Get("/apis/items.json?type=mlb_card&page={request.Page}")]
    Task<GetMlbCardItemsResponse> GetMlbCardItems(GetMlbCardItemsRequest request);
}