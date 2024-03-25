using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses;

/// <summary>
/// Defines a paginated response
/// </summary>
/// <param name="Page">The requested page</param>
/// <param name="PerPage">The number of results per page</param>
/// <param name="TotalPages">The total number of pages</param>
public abstract record PaginatedResponse(
    [property: JsonPropertyName("page")]
    int Page,
    [property: JsonPropertyName("per_page")]
    int PerPage,
    [property: JsonPropertyName("total_pages")]
    int TotalPages
);