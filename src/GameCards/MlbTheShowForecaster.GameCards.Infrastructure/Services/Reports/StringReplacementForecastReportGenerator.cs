using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Extensions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Publishing;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Publishing.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Publishing;

/// <summary>
/// Simple string replacement report generator that only has native .NET dependencies
/// </summary>
public sealed class StringReplacementForecastReportGenerator : IForecastReportGenerator
{
    /// <summary>
    /// The <see cref="PlayerCard"/> repository
    /// </summary>
    private readonly IPlayerCardRepository _playerCardRepository;

    /// <summary>
    /// Template for the layout
    /// </summary>
    private readonly string _templateHtml;

    /// <summary>
    /// Template for displaying a single <see cref="PlayerCardForecast"/>
    /// </summary>
    private readonly string _playerCardForecastHtml;

    /// <summary>
    /// Template for displaying a single <see cref="ForecastImpact"/>
    /// </summary>
    private readonly string _forecastImpactHtml;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="options">Configuration</param>
    /// <param name="playerCardRepository">The <see cref="PlayerCard"/> repository</param>
    /// <exception cref="FileNotFoundException">Thrown if the templates don't exist</exception>
    public StringReplacementForecastReportGenerator(ForecastReportOptions options,
        IPlayerCardRepository playerCardRepository)
    {
        _playerCardRepository = playerCardRepository;

        // Make sure all templates are provided
        if (!File.Exists(options.Templates["Layout"]) || !File.Exists(options.Templates["PlayerCardForecast"])
                                                      || !File.Exists(options.Templates["ForecastImpact"]))
        {
            throw new FileNotFoundException(
                $"All of the following templates are required: {options.Templates["Layout"]}, {options.Templates["PlayerCardForecast"]}, {options.Templates["ForecastImpact"]}");
        }

        _templateHtml = File.ReadAllText(options.Templates["Layout"]);
        _playerCardForecastHtml = File.ReadAllText(options.Templates["PlayerCardForecast"]);
        _forecastImpactHtml = File.ReadAllText(options.Templates["ForecastImpact"]);
    }

    /// <inheritdoc />
    public async Task<ForecastReport> Generate(SeasonYear year, IEnumerable<PlayerCardForecast> forecasts,
        DateOnly date)
    {
        var immutableForecasts = forecasts.ToImmutableList();

        var html = _templateHtml.Replace("{{Year}}", year.Value.ToString()[(4 - 2)..]);
        html = await ReplaceContentPlaceHolder(immutableForecasts, date, html);

        return new ForecastReport(year, immutableForecasts, html);
    }

    /// <summary>
    /// Replaces the content in the template with the <see cref="PlayerCardForecast"/>s
    /// </summary>
    /// <param name="forecasts"><see cref="PlayerCardForecast"/>s to report on</param>
    /// <param name="date">The date of the report</param>
    /// <param name="html">The template HTML</param>
    /// <returns>The resulting HTML</returns>
    private async Task<string> ReplaceContentPlaceHolder(IReadOnlyList<PlayerCardForecast> forecasts, DateOnly date,
        string html)
    {
        var gainerForecasts = forecasts.Where(x => x.EstimateDemandFor(date).Value > 0).ToImmutableList();
        var loserForecasts = forecasts.Where(x => x.EstimateDemandFor(date).Value <= 0).ToImmutableList();
        html = html.Replace("{{Gainers}}", await BuildRows(gainerForecasts, date));
        html = html.Replace("{{Losers}}", await BuildRows(loserForecasts, date));
        return html;
    }

    /// <summary>
    /// Builds rows for each <see cref="PlayerCardForecast"/>
    /// </summary>
    /// <param name="forecasts"><see cref="PlayerCardForecast"/>s to report on</param>
    /// <param name="date">The date of the report</param>
    /// <returns>The resulting HTML</returns>
    private async Task<string> BuildRows(IReadOnlyList<PlayerCardForecast> forecasts, DateOnly date)
    {
        var b = new StringBuilder();
        foreach (var forecast in forecasts)
        {
            b.Append(await BuildRow(forecast, date));
        }

        return b.ToString();
    }

    /// <summary>
    /// Builds HTML for a single <see cref="PlayerCardForecast"/>
    /// </summary>
    /// <param name="forecast"><see cref="PlayerCardForecast"/> to report on</param>
    /// <param name="date">The date of the report</param>
    /// <returns>The HTML for the <see cref="PlayerCardForecast"/></returns>
    /// <exception cref="NoPlayerCardForForecastReportException">Thrown if the corresponding <see cref="PlayerCard"/> is not found</exception>
    private async Task<string> BuildRow(PlayerCardForecast forecast, DateOnly date)
    {
        var playerCard = await _playerCardRepository.GetByExternalId(forecast.CardExternalId);
        if (playerCard == null)
        {
            throw new NoPlayerCardForForecastReportException(
                $"No player card found for {forecast.CardExternalId.Value}");
        }

        var impacts = forecast.ForecastImpactsChronologically
            .Where(x => x.EndDate >= date)
            .OrderByDescending(x => x.Demand.Value);

        var b = new StringBuilder();
        foreach (var i in impacts)
        {
            b.Append(BuildForecastImpact(i));
        }

        return _playerCardForecastHtml.Replace("{{YY}}", forecast.Year.Value.ToString()[(4 - 2)..])
            .Replace("{{CardExternalId}}", forecast.CardExternalId.AsStringDigits)
            .Replace("{{Name}}", playerCard.Name.Value)
            .Replace("{{Impacts}}", b.ToString());
    }

    /// <summary>
    /// Returns HTML for the specified <see cref="ForecastImpact"/>
    /// </summary>
    /// <param name="impact">The forecast impact</param>
    /// <returns>The resulting HTML</returns>
    private string BuildForecastImpact(ForecastImpact impact)
    {
        return impact switch
        {
            BattingStatsForecastImpact forecastImpact => BuildForecastImpact(forecastImpact),
            PitchingStatsForecastImpact forecastImpact => BuildForecastImpact(forecastImpact),
            FieldingStatsForecastImpact forecastImpact => BuildForecastImpact(forecastImpact),
            OverallRatingChangeForecastImpact forecastImpact => BuildForecastImpact(forecastImpact),
            BoostForecastImpact forecastImpact => BuildForecastImpact(forecastImpact),
            PositionChangeForecastImpact forecastImpact => BuildForecastImpact(forecastImpact),
            _ => _forecastImpactHtml.Replace("{{Type}}", impact.GetType().Name.Replace("ForecastImpact", ""))
                .Replace("{{BadgeType}}", impact.Demand.Value > 0 ? "text-bg-success" : "text-bg-danger")
                .Replace("{{Change}}", "")
                .Replace("{{Tooltip}}", "")
        };
    }

    /// <summary>
    /// Returns HTML for the specified <see cref="ForecastImpact"/>
    /// </summary>
    /// <param name="impact">The forecast impact</param>
    /// <returns>The resulting HTML</returns>
    private string BuildForecastImpact(BattingStatsForecastImpact impact)
    {
        return _forecastImpactHtml
            .Replace("{{Type}}", "Batting")
            .Replace("{{BadgeType}}", impact.Demand.Value > 0 ? "text-bg-success" : "text-bg-danger")
            .Replace("{{Tooltip}}", "")
            .Replace("{{Change}}",
                $" {impact.PercentageChange.PercentageChangeValue.ToString(CultureInfo.InvariantCulture)}%");
    }

    /// <summary>
    /// Returns HTML for the specified <see cref="ForecastImpact"/>
    /// </summary>
    /// <param name="impact">The forecast impact</param>
    /// <returns>The resulting HTML</returns>
    private string BuildForecastImpact(PitchingStatsForecastImpact impact)
    {
        return _forecastImpactHtml
            .Replace("{{Type}}", "Pitching")
            .Replace("{{BadgeType}}", impact.Demand.Value > 0 ? "text-bg-success" : "text-bg-danger")
            .Replace("{{Tooltip}}", "")
            .Replace("{{Change}}",
                $" {impact.PercentageChange.PercentageChangeValue.ToString(CultureInfo.InvariantCulture)}%");
    }

    private string BuildForecastImpact(FieldingStatsForecastImpact impact)
    {
        return _forecastImpactHtml
            .Replace("{{Type}}", "Fielding")
            .Replace("{{BadgeType}}", impact.Demand.Value > 0 ? "text-bg-success" : "text-bg-danger")
            .Replace("{{Tooltip}}", "")
            .Replace("{{Change}}",
                $" {impact.PercentageChange.PercentageChangeValue.ToString(CultureInfo.InvariantCulture)}%");
    }

    /// <summary>
    /// Returns HTML for the specified <see cref="ForecastImpact"/>
    /// </summary>
    /// <param name="impact">The forecast impact</param>
    /// <returns>The resulting HTML</returns>
    private string BuildForecastImpact(OverallRatingChangeForecastImpact impact)
    {
        return _forecastImpactHtml
            .Replace("{{Type}}", "OVR")
            .Replace("{{BadgeType}}", impact.Demand.Value > 0 ? "text-bg-success" : "text-bg-danger")
            .Replace("{{Tooltip}}", "")
            .Replace("{{Change}}", $" {impact.OldRating.Value} > {impact.NewRating.Value}");
    }

    /// <summary>
    /// Returns HTML for the specified <see cref="ForecastImpact"/>
    /// </summary>
    /// <param name="impact">The forecast impact</param>
    /// <returns>The resulting HTML</returns>
    private string BuildForecastImpact(BoostForecastImpact impact)
    {
        return _forecastImpactHtml
            .Replace("{{Type}}", "Boost")
            .Replace("{{BadgeType}}", impact.Demand.Value > 0 ? "text-bg-success" : "text-bg-danger")
            .Replace("{{Tooltip}}", impact.BoostReason)
            .Replace("{{Change}}", "");
    }

    /// <summary>
    /// Returns HTML for the specified <see cref="ForecastImpact"/>
    /// </summary>
    /// <param name="impact">The forecast impact</param>
    /// <returns>The resulting HTML</returns>
    private string BuildForecastImpact(PositionChangeForecastImpact impact)
    {
        return _forecastImpactHtml
            .Replace("{{Type}}", "Pos")
            .Replace("{{BadgeType}}", impact.Demand.Value > 0 ? "text-bg-success" : "text-bg-danger")
            .Replace("{{Tooltip}}", "")
            .Replace("{{Change}}", $" {impact.OldPosition.GetDisplayName()} > {impact.NewPosition.GetDisplayName()}");
    }
}