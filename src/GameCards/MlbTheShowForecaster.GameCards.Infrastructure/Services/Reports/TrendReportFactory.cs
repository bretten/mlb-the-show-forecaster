using System.Collections.Immutable;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Extensions;
using com.brettnamba.MlbTheShowForecaster.DomainApis.PerformanceApi;
using com.brettnamba.MlbTheShowForecaster.DomainApis.PerformanceApi.Responses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Reports.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Reports;

/// <inheritdoc />
public sealed class TrendReportFactory : ITrendReportFactory
{
    /// <summary>
    /// The <see cref="PlayerCard"/> repository
    /// </summary>
    private readonly IPlayerCardRepository _playerCardRepository;

    /// <summary>
    /// The <see cref="Listing"/> repository
    /// </summary>
    private readonly IListingRepository _listingRepository;

    /// <summary>
    /// The <see cref="PlayerCardForecast"/> repository
    /// </summary>
    private readonly IForecastRepository _forecastRepository;

    /// <summary>
    /// Performance API for querying stat metrics
    /// </summary>
    private readonly IPerformanceApi _performanceApi;

    /// <summary>
    /// Gets player info
    /// </summary>
    private readonly IPlayerMatcher _playerMatcher;

    /// <summary>
    /// Calendar to get today's date
    /// </summary>
    private readonly ICalendar _calendar;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerCardRepository">The <see cref="PlayerCard"/> repository</param>
    /// <param name="listingRepository">The <see cref="Listing"/> repository</param>
    /// <param name="forecastRepository">The <see cref="PlayerCardForecast"/> repository</param>
    /// <param name="performanceApi">Performance API for querying stat metrics</param>
    /// <param name="playerMatcher">Gets player info</param>
    /// <param name="calendar">Calendar to get today's date</param>
    public TrendReportFactory(IPlayerCardRepository playerCardRepository, IListingRepository listingRepository,
        IForecastRepository forecastRepository, IPerformanceApi performanceApi, IPlayerMatcher playerMatcher,
        ICalendar calendar)
    {
        _playerCardRepository = playerCardRepository;
        _listingRepository = listingRepository;
        _forecastRepository = forecastRepository;
        _performanceApi = performanceApi;
        _calendar = calendar;
        _playerMatcher = playerMatcher;
    }

    /// <inheritdoc />
    public async Task<TrendReport> GetReport(SeasonYear year, CardExternalId cardExternalId,
        CancellationToken cancellationToken)
    {
        // Get card, pricing and forecasts
        var (card, listing, forecast, mlbId) = await GetCard(year, cardExternalId, cancellationToken);

        // Get performance metrics
        var performanceMetrics = await GetPerformanceMetrics(year, mlbId);

        // Get player info
        var player = await GetPlayerInfo(card);

        return MapReport(card, listing, forecast, mlbId, performanceMetrics, player);
    }

    /// <inheritdoc />
    public async Task<TrendReport> GetReport(SeasonYear year, MlbId mlbId, CancellationToken cancellationToken)
    {
        // Get card, pricing and forecasts
        var (card, listing, forecast) = await GetCard(year, mlbId, cancellationToken);

        // Get performance metrics
        var performanceMetrics = await GetPerformanceMetrics(year, mlbId);

        // Get player info
        var player = await GetPlayerInfo(card);

        return MapReport(card, listing, forecast, mlbId, performanceMetrics, player);
    }

    /// <summary>
    /// Gets card information by season and card external ID
    /// </summary>
    /// <param name="year">The season</param>
    /// <param name="cardExternalId">The card external ID</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>Card data</returns>
    /// <exception cref="TrendReportFactoryMissingDataException">Thrown if the card data could not be retrieved</exception>
    private async Task<(PlayerCard, Listing, PlayerCardForecast, MlbId)> GetCard(SeasonYear year,
        CardExternalId cardExternalId, CancellationToken cancellationToken)
    {
        var card = await _playerCardRepository.GetByExternalId(cardExternalId);
        var forecast = await _forecastRepository.GetBy(year, cardExternalId);
        var listing = await _listingRepository.GetByExternalId(cardExternalId, cancellationToken);

        if (card == null || listing == null || forecast == null || forecast.MlbId == null)
        {
            throw new TrendReportFactoryMissingDataException(card, listing, forecast, forecast?.MlbId, year,
                cardExternalId);
        }

        return (card, listing, forecast, forecast.MlbId);
    }

    /// <summary>
    /// Gets card information by season and MLB ID
    /// </summary>
    /// <param name="year">The season</param>
    /// <param name="mlbId">The MLB ID</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>Card data</returns>
    /// <exception cref="TrendReportFactoryMissingDataException">Thrown if the card data could not be retrieved</exception>
    private async Task<(PlayerCard, Listing, PlayerCardForecast)> GetCard(SeasonYear year, MlbId mlbId,
        CancellationToken cancellationToken)
    {
        var forecast = await _forecastRepository.GetBy(year, mlbId);
        var card = await _playerCardRepository.GetByExternalId(forecast?.CardExternalId!);
        var listing = await _listingRepository.GetByExternalId(forecast?.CardExternalId!, cancellationToken);

        if (forecast == null || card == null || listing == null)
        {
            throw new TrendReportFactoryMissingDataException(year, mlbId);
        }

        return (card, listing, forecast);
    }

    /// <summary>
    /// Gets performance metrics by day for the specified player's season
    /// </summary>
    /// <param name="year">The season</param>
    /// <param name="mlbId">The MLB ID of the player</param>
    /// <returns><see cref="PerformanceMetricsByDate"/> collection</returns>
    private async Task<IReadOnlyList<PerformanceMetricsByDate>> GetPerformanceMetrics(SeasonYear year, MlbId mlbId)
    {
        var start = StartOfSeason(year);
        var end = _calendar.Today();
        var performanceMetrics =
            await _performanceApi.GetPlayerSeasonPerformance(year.Value, mlbId.Value, start.ToString("O"),
                end.ToString("O"));
        return performanceMetrics.Content.MetricsByDate;
    }

    /// <summary>
    /// Gets player info
    /// </summary>
    /// <param name="card"><see cref="PlayerCard"/></param>
    /// <returns><see cref="Player"/> or null if none was found</returns>
    private async Task<Player> GetPlayerInfo(PlayerCard card)
    {
        return await _playerMatcher.GetPlayerByName(card.Name, card.TeamShortName)
               ?? throw new TrendReportFactoryMissingDataException(card);
    }

    /// <summary>
    /// Maps all data to a <see cref="TrendReport"/>
    /// </summary>
    /// <param name="card"><see cref="PlayerCard"/></param>
    /// <param name="listing"><see cref="Listing"/></param>
    /// <param name="forecast"><see cref="PlayerCardForecast"/></param>
    /// <param name="mlbId"><see cref="MlbId"/></param>
    /// <param name="performanceMetrics"><see cref="PerformanceMetricsByDate"/> collection</param>
    /// <param name="player"><see cref="Player"/></param>
    /// <returns><see cref="TrendReport"/></returns>
    private static TrendReport MapReport(PlayerCard card, Listing listing, PlayerCardForecast forecast, MlbId mlbId,
        IReadOnlyList<PerformanceMetricsByDate> performanceMetrics, Player player)
    {
        return new TrendReport(
            Year: card.Year,
            CardExternalId: card.ExternalId,
            MlbId: mlbId,
            CardName: card.Name,
            PrimaryPosition: player.Position,
            OverallRating: card.OverallRating,
            MetricsByDate: listing.HistoricalPricesChronologically.Select(x => MapMetrics(x, performanceMetrics))
                .ToImmutableList(),
            Impacts: forecast.ForecastImpactsChronologically.Select(MapForecastImpact).ToImmutableList()
        );
    }

    /// <summary>
    /// Maps card price and player stat metrics by date
    /// </summary>
    /// <param name="historicalPrice">Historical card prices</param>
    /// <param name="performanceMetrics">Stat trends</param>
    /// <returns><see cref="TrendMetricsByDate"/></returns>
    private static TrendMetricsByDate MapMetrics(ListingHistoricalPrice historicalPrice,
        IReadOnlyList<PerformanceMetricsByDate> performanceMetrics)
    {
        var hasPerformance = performanceMetrics.Any(y => y.Date == historicalPrice.Date);
        var p = performanceMetrics.FirstOrDefault(y => y.Date == historicalPrice.Date);
        return new TrendMetricsByDate(
            Date: historicalPrice.Date,
            BuyPrice: historicalPrice.BuyPrice.Value,
            SellPrice: historicalPrice.SellPrice.Value,
            BattingScore: hasPerformance ? p.BattingScore : null,
            SignificantBattingParticipation: hasPerformance && p.SignificantBattingParticipation,
            PitchingScore: hasPerformance ? p.PitchingScore : null,
            SignificantPitchingParticipation: hasPerformance && p.SignificantPitchingParticipation,
            FieldingScore: hasPerformance ? p.FieldingScore : null,
            SignificantFieldingParticipation: hasPerformance && p.SignificantFieldingParticipation,
            BattingAverage: hasPerformance ? p.BattingAverage : null,
            OnBasePercentage: hasPerformance ? p.OnBasePercentage : null,
            Slugging: hasPerformance ? p.Slugging : null,
            EarnedRunAverage: hasPerformance ? p.EarnedRunAverage : null,
            OpponentsBattingAverage: hasPerformance ? p.OpponentsBattingAverage : null,
            StrikeoutsPer9: hasPerformance ? p.StrikeoutsPer9 : null,
            BaseOnBallsPer9: hasPerformance ? p.BaseOnBallsPer9 : null,
            HomeRunsPer9: hasPerformance ? p.HomeRunsPer9 : null,
            FieldingPercentage: hasPerformance ? p.FieldingPercentage : null);
    }

    /// <summary>
    /// Maps a <see cref="ForecastImpact"/> to a <see cref="TrendImpact"/>
    /// </summary>
    private static TrendImpact MapForecastImpact(ForecastImpact forecastImpact)
    {
        var description = forecastImpact switch
        {
            PlayerActivationForecastImpact => "Player was activated",
            PlayerDeactivationForecastImpact => "Player was deactivated",
            PlayerTeamSigningForecastImpact => "Player signed a new team contract",
            PlayerFreeAgencyForecastImpact => "Player entered free agency",
            PositionChangeForecastImpact impact =>
                $"Player's position changed from {impact.OldPosition.GetDisplayName()} to {impact.NewPosition.GetDisplayName()}",
            BoostForecastImpact impact => $"Player's card was boosted for: {impact.BoostReason}",
            OverallRatingChangeForecastImpact impact =>
                $"Player's rating changed from {impact.OldRating.Value} to {impact.NewRating.Value}",
            BattingStatsForecastImpact impact => impact.IsImprovement
                ? $"Player's batting score improved from {impact.OldScore} to {impact.NewScore} (+{impact.PercentageChange.PercentageChangeValue})"
                : $"Player's batting score declined from {impact.OldScore} to {impact.NewScore} (-{impact.PercentageChange.PercentageChangeValue})",
            PitchingStatsForecastImpact impact => impact.IsImprovement
                ? $"Player's pitching score improved from {impact.OldScore} to {impact.NewScore} (+{impact.PercentageChange.PercentageChangeValue})"
                : $"Player's pitching score declined from {impact.OldScore} to {impact.NewScore} (-{impact.PercentageChange.PercentageChangeValue})",
            FieldingStatsForecastImpact impact => impact.IsImprovement
                ? $"Player's fielding score improved from {impact.OldScore} to {impact.NewScore} (+{impact.PercentageChange.PercentageChangeValue})"
                : $"Player's fielding score declined from {impact.OldScore} to {impact.NewScore} (-{impact.PercentageChange.PercentageChangeValue})",
            _ => $"Player's demand changed by {forecastImpact.Demand.Value}"
        };

        return new TrendImpact(forecastImpact.StartDate, forecastImpact.EndDate, description);
    }

    /// <summary>
    /// The start of the season
    /// </summary>
    private static DateOnly StartOfSeason(SeasonYear year) => new(year.Value, 3, 1);
}