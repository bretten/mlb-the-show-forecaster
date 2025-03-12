using System.Collections.Immutable;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums.Extensions;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Extensions;
using com.brettnamba.MlbTheShowForecaster.DomainApis.PerformanceApi;
using com.brettnamba.MlbTheShowForecaster.DomainApis.PerformanceApi.Responses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports.Exceptions;
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
    /// Clock to get the current time
    /// </summary>
    private readonly IClock _clock;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerCardRepository">The <see cref="PlayerCard"/> repository</param>
    /// <param name="listingRepository">The <see cref="Listing"/> repository</param>
    /// <param name="forecastRepository">The <see cref="PlayerCardForecast"/> repository</param>
    /// <param name="performanceApi">Performance API for querying stat metrics</param>
    /// <param name="playerMatcher">Gets player info</param>
    /// <param name="calendar">Calendar to get today's date</param>
    /// <param name="clock">Clock to get the current time</param>
    public TrendReportFactory(IPlayerCardRepository playerCardRepository, IListingRepository listingRepository,
        IForecastRepository forecastRepository, IPerformanceApi performanceApi, IPlayerMatcher playerMatcher,
        ICalendar calendar, IClock clock)
    {
        _playerCardRepository = playerCardRepository;
        _listingRepository = listingRepository;
        _forecastRepository = forecastRepository;
        _performanceApi = performanceApi;
        _calendar = calendar;
        _clock = clock;
        _playerMatcher = playerMatcher;
        _orderMap = new OrderCountMapper();
        _priceMap = new PriceMapper();
        _performanceMetricsMap = new PerformanceMetricMapper();
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
        var card = await _playerCardRepository.GetByExternalId(year, cardExternalId);
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
        var card = await _playerCardRepository.GetByExternalId(year, forecast?.CardExternalId!);
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
    /// <returns><see cref="Player"/></returns>
    /// <exception cref="TrendReportFactoryMissingDataException">Thrown if no player was found</exception>
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
    private TrendReport MapReport(PlayerCard card, Listing listing, PlayerCardForecast forecast, MlbId mlbId,
        IReadOnlyList<PerformanceMetricsByDate> performanceMetrics, Player player)
    {
        // Reference dates and times that will determine the metrics
        var today = _calendar.Today();
        var now = _clock.UtcNow().UtcDateTime;
        var datesAndTimes = new MetricDatesAndTimes(card.Year, today, now);

        // Order counts
        var orderCounts = _orderMap.Map(listing, datesAndTimes);

        // Price comparisons
        var prices = _priceMap.Map(listing, datesAndTimes);

        // Performance metrics
        var performance = _performanceMetricsMap.Map(listing, forecast, player, performanceMetrics, datesAndTimes);

        return new TrendReport(
            Year: card.Year,
            CardExternalId: card.ExternalId,
            MlbId: mlbId,
            CardName: card.Name,
            PrimaryPosition: player.Position.IsTwoWayPlayer() ? player.Position : card.Position,
            OverallRating: card.OverallRating,
            MetricsByDate: performance.Metrics,
            Impacts: forecast.ForecastImpactsChronologically.Select(MapForecastImpact).ToImmutableList(),
            IsBoosted: card.IsBoosted,
            Orders1H: orderCounts.Orders1H,
            Orders24H: orderCounts.Orders24H,
            BuyPrice: prices.BuyPrice,
            BuyPriceChange24H: prices.BuyPriceChange24H,
            SellPrice: prices.SellPrice,
            SellPriceChange24H: prices.SellPriceChange24H,
            Score: performance.Score,
            ScoreChange2W: performance.ScoreChange2W,
            Demand: forecast.EstimateDemandFor(today).Value
        );
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

        return new TrendImpact(forecastImpact.StartDate, forecastImpact.EndDate, description,
            forecastImpact.Demand.Value);
    }

    /// <summary>
    /// The start of the season
    /// </summary>
    private static DateOnly StartOfSeason(SeasonYear year) => new(year.Value, 3, 1);

    /// <summary>
    /// <inheritdoc cref="IOrderCountMapper"/>
    /// </summary>
    private readonly IOrderCountMapper _orderMap;

    /// <summary>
    /// <inheritdoc cref="IPriceMapper"/>
    /// </summary>
    private readonly IPriceMapper _priceMap;

    /// <summary>
    /// <inheritdoc cref="IPerformanceMetricMapper"/>
    /// </summary>
    private readonly IPerformanceMetricMapper _performanceMetricsMap;

    internal TrendReportFactory(IPlayerCardRepository playerCardRepository, IListingRepository listingRepository,
        IForecastRepository forecastRepository, IPerformanceApi performanceApi, IPlayerMatcher playerMatcher,
        ICalendar calendar, IClock clock, IOrderCountMapper orderMap, IPriceMapper priceMap,
        IPerformanceMetricMapper performanceMetricsMap)
    {
        _playerCardRepository = playerCardRepository;
        _listingRepository = listingRepository;
        _forecastRepository = forecastRepository;
        _performanceApi = performanceApi;
        _calendar = calendar;
        _clock = clock;
        _playerMatcher = playerMatcher;
        _orderMap = orderMap;
        _priceMap = priceMap;
        _performanceMetricsMap = performanceMetricsMap;
    }

    /// <summary>
    /// The different dates and times that will be used to calculate metrics
    /// </summary>
    internal record MetricDatesAndTimes(SeasonYear Season, DateOnly Today, DateTime Now)
    {
        public DateOnly Yesterday = Today.AddDays(-1);
        public DateOnly EndOfSeason = new DateOnly(Season.Value, 10, 1);
        public DateOnly ScoreDate => Today >= EndOfSeason ? EndOfSeason : Today;
        public DateOnly ScoreDate2W => ScoreDate.AddDays(-14);
        public DateTime OneHourAgo = Now.AddHours(-1);
        public DateTime OneDayAgo = Now.AddHours(-24);
    }

    /// <summary>
    /// Order count metrics for different time frames
    /// </summary>
    internal readonly record struct OrderCount(int Orders1H, int Orders24H);

    /// <summary>
    /// Maps order count metrics for different time frames
    /// </summary>
    internal interface IOrderCountMapper
    {
        OrderCount Map(Listing listing, MetricDatesAndTimes times);
    }

    /// <inheritdoc />
    internal sealed class OrderCountMapper : IOrderCountMapper
    {
        public OrderCount Map(Listing listing, MetricDatesAndTimes times)
        {
            var orders1H = listing.TotalOrdersFor(times.OneHourAgo, times.Now);
            var orders24H = listing.TotalOrdersFor(times.OneDayAgo, times.Now);
            return new OrderCount(Orders1H: orders1H.Value, Orders24H: orders24H.Value);
        }
    }

    /// <summary>
    /// Listing prices and price changes
    /// </summary>
    internal readonly record struct Prices(
        int BuyPrice,
        decimal BuyPriceChange24H,
        int SellPrice,
        decimal SellPriceChange24H);

    /// <summary>
    /// Maps prices and price changes
    /// </summary>
    internal interface IPriceMapper
    {
        Prices Map(Listing listing, MetricDatesAndTimes times);
    }

    /// <inheritdoc />
    internal sealed class PriceMapper : IPriceMapper
    {
        public Prices Map(Listing listing, MetricDatesAndTimes times)
        {
            var buyPrice = listing.BuyPrice;
            var sellPrice = listing.SellPrice;
            var yesterdayPrice = listing.PriceFor(times.Yesterday);
            var yesterdayBuyPrice = yesterdayPrice?.BuyPrice ?? NaturalNumber.Create(0);
            var yesterdaySellPrice = yesterdayPrice?.SellPrice ?? NaturalNumber.Create(0);
            var buyPricePercentageChange = yesterdayBuyPrice.Value > 0
                ? PercentageChange.Create(yesterdayBuyPrice, buyPrice, true).PercentageChangeValue
                : 0;
            var sellPricePercentageChange = yesterdaySellPrice.Value > 0
                ? PercentageChange.Create(yesterdaySellPrice, sellPrice, true).PercentageChangeValue
                : 0;
            return new Prices(BuyPrice: buyPrice.Value, BuyPriceChange24H: buyPricePercentageChange,
                SellPrice: sellPrice.Value, SellPriceChange24H: sellPricePercentageChange);
        }
    }

    /// <summary>
    /// Player performance metrics
    /// </summary>
    internal readonly record struct PerformanceMetrics(
        decimal Score,
        decimal ScoreChange2W,
        IReadOnlyList<TrendMetricsByDate> Metrics);

    /// <summary>
    /// Maps player performance metrics
    /// </summary>
    internal interface IPerformanceMetricMapper
    {
        PerformanceMetrics Map(Listing listing, PlayerCardForecast forecast, Player player,
            IReadOnlyList<PerformanceMetricsByDate> performanceMetrics, MetricDatesAndTimes times);
    }

    /// <inheritdoc />
    internal sealed class PerformanceMetricMapper : IPerformanceMetricMapper
    {
        public PerformanceMetrics Map(Listing listing, PlayerCardForecast forecast, Player player,
            IReadOnlyList<PerformanceMetricsByDate> performanceMetrics, MetricDatesAndTimes times)
        {
            // Performance metrics by date
            var metricsByDate = listing.HistoricalPricesChronologically
                .Select(x => MapMetrics(x, performanceMetrics, forecast, listing.TotalOrdersFor(x.Date).Value))
                .ToImmutableList();

            // Score comparisons
            decimal currentScore;
            decimal score2W;
            if (player.Position.IsOnlyPitcher())
            {
                currentScore = metricsByDate.FirstOrDefault(x => x.Date == times.ScoreDate).PitchingScore ?? 0;
                score2W = metricsByDate.FirstOrDefault(x => x.Date == times.ScoreDate2W).PitchingScore ?? 0;
            }
            else
            {
                currentScore = metricsByDate.FirstOrDefault(x => x.Date == times.ScoreDate).BattingScore ?? 0;
                score2W = metricsByDate.FirstOrDefault(x => x.Date == times.ScoreDate2W).BattingScore ?? 0;
            }

            var scorePercentageChange = PercentageChange.Create(score2W, currentScore, true);
            return new PerformanceMetrics(currentScore, scorePercentageChange.PercentageChangeValue, metricsByDate);
        }

        /// <summary>
        /// Maps card price and player stat metrics by date
        /// </summary>
        /// <param name="historicalPrice">Historical card prices</param>
        /// <param name="performanceMetrics">Stat trends</param>
        /// <param name="forecast"><see cref="PlayerCardForecast"/></param>
        /// <param name="orders">Order count for the day</param>
        /// <returns><see cref="TrendMetricsByDate"/></returns>
        private static TrendMetricsByDate MapMetrics(ListingHistoricalPrice historicalPrice,
            IReadOnlyList<PerformanceMetricsByDate> performanceMetrics, PlayerCardForecast forecast, int orders)
        {
            var p = performanceMetrics?.FirstOrDefault(y => y.Date == historicalPrice.Date);
            var demand = forecast.EstimateDemandFor(historicalPrice.Date).Value;

            return new TrendMetricsByDate(
                Date: historicalPrice.Date,
                BuyPrice: historicalPrice.BuyPrice.Value,
                SellPrice: historicalPrice.SellPrice.Value,
                BattingScore: p?.BattingScore ?? null,
                SignificantBattingParticipation: p?.SignificantBattingParticipation ?? false,
                PitchingScore: p?.PitchingScore ?? null,
                SignificantPitchingParticipation: p?.SignificantPitchingParticipation ?? false,
                FieldingScore: p?.FieldingScore ?? null,
                SignificantFieldingParticipation: p?.SignificantFieldingParticipation ?? false,
                BattingAverage: p?.BattingAverage ?? null,
                OnBasePercentage: p?.OnBasePercentage ?? null,
                Slugging: p?.Slugging ?? null,
                EarnedRunAverage: p?.EarnedRunAverage ?? null,
                OpponentsBattingAverage: p?.OpponentsBattingAverage ?? null,
                StrikeoutsPer9: p?.StrikeoutsPer9 ?? null,
                BaseOnBallsPer9: p?.BaseOnBallsPer9 ?? null,
                HomeRunsPer9: p?.HomeRunsPer9 ?? null,
                FieldingPercentage: p?.FieldingPercentage ?? null,
                Demand: demand,
                OrderCount: orders);
        }
    }
}