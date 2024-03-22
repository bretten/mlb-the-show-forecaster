using System.Text.Json.Serialization;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests;

/// <summary>
/// Defines a paginated request
/// </summary>
/// <param name="Page">The desired page</param>
public abstract record PaginatedRequest(
    [property: AliasAs("page")]
    [property: JsonPropertyName("page")]
    int Page
);