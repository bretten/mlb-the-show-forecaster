using System.Text.Json;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Reports;
using Microsoft.AspNetCore.Mvc;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Api.Controllers;

/// <summary>
/// Exposes <see cref="TrendReport"/>s
/// </summary>
[ApiController]
[Route("[controller]s")]
public class TrendController : Controller
{
    /// <summary>
    /// Provides <see cref="TrendReport"/>s
    /// </summary>
    private readonly ITrendReporter _trendReporter;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="trendReporter">Provides <see cref="TrendReport"/>s</param>
    public TrendController(ITrendReporter trendReporter)
    {
        _trendReporter = trendReporter;
    }

    /// <summary>
    /// Returns <see cref="TrendReport"/>s by page and sort criteria
    /// </summary>
    /// <param name="season">The season</param>
    /// <param name="page">The page</param>
    /// <param name="pageSize">The page size</param>
    /// <param name="sortField">The field to sort on</param>
    /// <param name="sortOrder">The sort direction</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns><see cref="JsonResult"/> of <see cref="TrendReport"/>s</returns>
    public async Task<IActionResult> Index([FromQuery] ushort season, [FromQuery] int page, [FromQuery] int pageSize,
        [FromQuery] ITrendReporter.SortField? sortField = null, [FromQuery] ITrendReporter.SortOrder? sortOrder = null,
        CancellationToken cancellationToken = default)
    {
        var results = await _trendReporter.GetTrendReports(SeasonYear.Create(season), page, pageSize, sortField,
            sortOrder, cancellationToken);
        return Json(results, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new TrendReportJsonConverter() }
        });
    }
}