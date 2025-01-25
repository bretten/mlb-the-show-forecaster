using System.Collections.Immutable;
using System.Net;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.DomainApis.PerformanceApi;
using com.brettnamba.MlbTheShowForecaster.DomainApis.PerformanceApi.Responses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Reports.Exceptions;
using Moq;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Services.Reports;

public class TrendReportFactoryTests
{
    private static readonly DateOnly Today = new DateOnly(2024, 9, 8);
    private static readonly DateOnly Tomorrow = Today.AddDays(1);
    private static readonly DateTime Now = new DateTime(2024, 9, 8, 10, 0, 0, DateTimeKind.Utc);

    private static readonly TrendReportFactory.MetricDatesAndTimes MetricDatesAndTimes =
        new TrendReportFactory.MetricDatesAndTimes(SeasonYear.Create(2024), Today, Now);

    private static PlayerCard PlayerCard() => Faker.FakePlayerCard(year: 2024, name: Faker.FakeCardName("Dottie"),
        externalId: Faker.FakeGuid1, position: Position.CenterField, overallRating: 99, isBoosted: true);

    private static Listing Listing() => Domain.Tests.Marketplace.TestClasses.Faker.FakeListing(buyPrice: 123,
        sellPrice: 4560,
        historicalPrices: new List<ListingHistoricalPrice>()
        {
            Domain.Tests.Marketplace.TestClasses.Faker.FakeListingHistoricalPrice(MetricDatesAndTimes.ScoreDate2W,
                buyPrice: 1, sellPrice: 1),
            Domain.Tests.Marketplace.TestClasses.Faker.FakeListingHistoricalPrice(MetricDatesAndTimes.Yesterday,
                buyPrice: 1230, sellPrice: 456),
            Domain.Tests.Marketplace.TestClasses.Faker.FakeListingHistoricalPrice(MetricDatesAndTimes.Today,
                buyPrice: 123, sellPrice: 4560)
        },
        orders: new List<ListingOrder>()
        {
            // Included in past hour
            Domain.Tests.Marketplace.TestClasses.Faker.FakeListingOrder(new DateTime(2024, 9, 8, 9, 0, 0,
                DateTimeKind.Utc), price: 10, quantity: 1),
            // Included in past 24 hours
            Domain.Tests.Marketplace.TestClasses.Faker.FakeListingOrder(new DateTime(2024, 9, 7, 10, 0, 0,
                DateTimeKind.Utc), price: 20, quantity: 2),
            // Not included
            Domain.Tests.Marketplace.TestClasses.Faker.FakeListingOrder(new DateTime(2024, 9, 7, 9, 0, 0,
                DateTimeKind.Utc), price: 40, quantity: 4),
        });

    private static PlayerCardForecast Forecast()
    {
        var forecast = Domain.Tests.Forecasts.TestClasses.Faker.FakePlayerCardForecast(mlbId: 100);
        var boost = Domain.Tests.Forecasts.TestClasses.Faker.FakeBoostForecastImpact(
            startDate: MetricDatesAndTimes.Today, endDate: Tomorrow);
        forecast.Reassess(boost, date: Tomorrow);
        return forecast;
    }

    private static Player Player() => Application.Tests.Dtos.TestClasses.Faker.FakePlayer(mlbId: 100);

    private static readonly TrendReport ExpectedReport = new TrendReport(
        Year: SeasonYear.Create(2024),
        CardExternalId: CardExternalId.Create(new Guid("00000000-0000-0000-0000-000000000001")),
        MlbId: MlbId.Create(100),
        CardName: Faker.FakeCardName("Dottie"),
        PrimaryPosition: Position.CenterField,
        OverallRating: Faker.FakeOverallRating(99),
        IsBoosted: true,
        Orders1H: 1,
        Orders24H: 3,
        BuyPrice: 123,
        BuyPriceChange24H: -90,
        SellPrice: 4560,
        SellPriceChange24H: 900,
        Score: 0.1m,
        ScoreChange2W: -80.39m,
        Demand: 1,
        MetricsByDate: new List<TrendMetricsByDate>()
        {
            new TrendMetricsByDate(
                Date: MetricDatesAndTimes.ScoreDate2W,
                BuyPrice: 1,
                SellPrice: 1,
                BattingScore: 0.51m,
                SignificantBattingParticipation: true,
                PitchingScore: 0.52m,
                SignificantPitchingParticipation: true,
                FieldingScore: 0.53m,
                SignificantFieldingParticipation: true,
                BattingAverage: 0.5111m,
                OnBasePercentage: 0.5112m,
                Slugging: 0.5113m,
                EarnedRunAverage: 0.5114m,
                OpponentsBattingAverage: 0.5115m,
                StrikeoutsPer9: 0.5116m,
                BaseOnBallsPer9: 0.5117m,
                HomeRunsPer9: 0.5118m,
                FieldingPercentage: 0.5119m,
                Demand: 0,
                OrderCount: 0
            ),
            new TrendMetricsByDate(
                Date: MetricDatesAndTimes.Yesterday,
                BuyPrice: 1230,
                SellPrice: 456,
                BattingScore: 0.11m,
                SignificantBattingParticipation: true,
                PitchingScore: 0.22m,
                SignificantPitchingParticipation: true,
                FieldingScore: 0.33m,
                SignificantFieldingParticipation: true,
                BattingAverage: 0.1111m,
                OnBasePercentage: 0.1122m,
                Slugging: 0.1133m,
                EarnedRunAverage: 0.1144m,
                OpponentsBattingAverage: 0.1155m,
                StrikeoutsPer9: 0.1166m,
                BaseOnBallsPer9: 0.1177m,
                HomeRunsPer9: 0.1188m,
                FieldingPercentage: 0.1199m,
                Demand: 0,
                OrderCount: 6
            ),
            new TrendMetricsByDate(
                Date: MetricDatesAndTimes.Today,
                BuyPrice: 123,
                SellPrice: 4560,
                BattingScore: 0.1m,
                SignificantBattingParticipation: true,
                PitchingScore: 0.2m,
                SignificantPitchingParticipation: true,
                FieldingScore: 0.3m,
                SignificantFieldingParticipation: true,
                BattingAverage: 0.111m,
                OnBasePercentage: 0.112m,
                Slugging: 0.113m,
                EarnedRunAverage: 0.114m,
                OpponentsBattingAverage: 0.115m,
                StrikeoutsPer9: 0.116m,
                BaseOnBallsPer9: 0.117m,
                HomeRunsPer9: 0.118m,
                FieldingPercentage: 0.119m,
                Demand: 1,
                OrderCount: 1
            ),
        }.ToImmutableList(),
        Impacts: new List<TrendImpact>()
        {
            new TrendImpact(Start: MetricDatesAndTimes.Today, End: Tomorrow,
                Description: "Player's card was boosted for: Hit 5 HRs",
                Demand: 3)
        }.ToImmutableList()
    );

    private static PlayerSeasonPerformanceResponse FakePlayerSeasonPerformanceResponse =>
        new PlayerSeasonPerformanceResponse()
        {
            MetricsByDate = new List<PerformanceMetricsByDate>()
            {
                new PerformanceMetricsByDate(Date: MetricDatesAndTimes.ScoreDate2W,
                    BattingScore: 0.51m,
                    SignificantBattingParticipation: true,
                    PitchingScore: 0.52m,
                    SignificantPitchingParticipation: true,
                    FieldingScore: 0.53m,
                    SignificantFieldingParticipation: true,
                    BattingAverage: 0.5111m,
                    OnBasePercentage: 0.5112m,
                    Slugging: 0.5113m,
                    EarnedRunAverage: 0.5114m,
                    OpponentsBattingAverage: 0.5115m,
                    StrikeoutsPer9: 0.5116m,
                    BaseOnBallsPer9: 0.5117m,
                    HomeRunsPer9: 0.5118m,
                    FieldingPercentage: 0.5119m
                ),
                new PerformanceMetricsByDate(Date: MetricDatesAndTimes.Yesterday,
                    BattingScore: 0.11m,
                    SignificantBattingParticipation: true,
                    PitchingScore: 0.22m,
                    SignificantPitchingParticipation: true,
                    FieldingScore: 0.33m,
                    SignificantFieldingParticipation: true,
                    BattingAverage: 0.1111m,
                    OnBasePercentage: 0.1122m,
                    Slugging: 0.1133m,
                    EarnedRunAverage: 0.1144m,
                    OpponentsBattingAverage: 0.1155m,
                    StrikeoutsPer9: 0.1166m,
                    BaseOnBallsPer9: 0.1177m,
                    HomeRunsPer9: 0.1188m,
                    FieldingPercentage: 0.1199m
                ),
                new PerformanceMetricsByDate(Date: MetricDatesAndTimes.Today,
                    BattingScore: 0.1m,
                    SignificantBattingParticipation: true,
                    PitchingScore: 0.2m,
                    SignificantPitchingParticipation: true,
                    FieldingScore: 0.3m,
                    SignificantFieldingParticipation: true,
                    BattingAverage: 0.111m,
                    OnBasePercentage: 0.112m,
                    Slugging: 0.113m,
                    EarnedRunAverage: 0.114m,
                    OpponentsBattingAverage: 0.115m,
                    StrikeoutsPer9: 0.116m,
                    BaseOnBallsPer9: 0.117m,
                    HomeRunsPer9: 0.118m,
                    FieldingPercentage: 0.119m
                )
            }
        };

    [Fact]
    public async Task GetReport_YearAndCardExternalIdAndMissingData_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var playerCard = PlayerCard();
        var listing = Listing();
        PlayerCardForecast? forecast = null;

        var stubPlayerCardRepository = new Mock<IPlayerCardRepository>();
        stubPlayerCardRepository.Setup(x => x.GetByExternalId(playerCard.ExternalId))
            .ReturnsAsync(playerCard);

        var stubListingRepository = new Mock<IListingRepository>();
        stubListingRepository.Setup(x => x.GetByExternalId(playerCard.ExternalId, cToken))
            .ReturnsAsync(listing);

        var stubForecastRepository = new Mock<IForecastRepository>();
        stubForecastRepository.Setup(x => x.GetBy(playerCard.Year, playerCard.ExternalId))
            .ReturnsAsync(forecast);

        var mockPerformanceApi = Mock.Of<IPerformanceApi>();

        var mockPlayerMatcher = Mock.Of<IPlayerMatcher>();

        var stubCalendar = new Mock<ICalendar>();
        stubCalendar.Setup(x => x.Today())
            .Returns(MetricDatesAndTimes.Today);

        var mockClock = Mock.Of<IClock>();

        var factory = new TrendReportFactory(stubPlayerCardRepository.Object, stubListingRepository.Object,
            stubForecastRepository.Object, mockPerformanceApi, mockPlayerMatcher, stubCalendar.Object, mockClock);

        var action = async () => await factory.GetReport(playerCard.Year, playerCard.ExternalId, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<TrendReportFactoryMissingDataException>(actual);
    }

    [Fact]
    public async Task GetReport_YearAndCardExternalId_ReturnsTrendReport()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var playerCard = PlayerCard();
        var listing = Listing();
        var forecast = Forecast();
        var player = Player();

        var stubPlayerCardRepository = new Mock<IPlayerCardRepository>();
        stubPlayerCardRepository.Setup(x => x.GetByExternalId(playerCard.ExternalId))
            .ReturnsAsync(playerCard);

        var stubListingRepository = new Mock<IListingRepository>();
        stubListingRepository.Setup(x => x.GetByExternalId(playerCard.ExternalId, cToken))
            .ReturnsAsync(listing);

        var stubForecastRepository = new Mock<IForecastRepository>();
        stubForecastRepository.Setup(x => x.GetBy(playerCard.Year, playerCard.ExternalId))
            .ReturnsAsync(forecast);

        var stubPerformanceApi = new Mock<IPerformanceApi>();
        stubPerformanceApi.Setup(x =>
                x.GetPlayerSeasonPerformance(playerCard.Year.Value, player.MlbId.Value, "2024-03-01", "2024-09-08"))
            .ReturnsAsync(new ApiResponse<PlayerSeasonPerformanceResponse>(new HttpResponseMessage(HttpStatusCode.OK),
                FakePlayerSeasonPerformanceResponse, new RefitSettings()));

        var stubPlayerMatcher = new Mock<IPlayerMatcher>();
        stubPlayerMatcher.Setup(x => x.GetPlayerByName(playerCard.Name, playerCard.TeamShortName))
            .ReturnsAsync(player);

        var stubCalendar = new Mock<ICalendar>();
        stubCalendar.Setup(x => x.Today())
            .Returns(Today);

        var stubClock = new Mock<IClock>();
        stubClock.Setup(x => x.UtcNow())
            .Returns(Now);

        var stubOrderCountMapper = new Mock<TrendReportFactory.IOrderCountMapper>();
        stubOrderCountMapper.Setup(x => x.Map(listing, MetricDatesAndTimes))
            .Returns(new TrendReportFactory.OrderCount(1, 24));

        var stubPriceMapper = new Mock<TrendReportFactory.IPriceMapper>();
        stubPriceMapper.Setup(x => x.Map(listing, MetricDatesAndTimes))
            .Returns(new TrendReportFactory.Prices(1, 2, 3, 4));

        var stubMetricMapper = new Mock<TrendReportFactory.IPerformanceMetricMapper>();
        var expectedMetrics = new List<TrendMetricsByDate>()
        {
            Application.Tests.Dtos.TestClasses.Faker.FakeTrendMetricsByDate(new DateOnly(2024, 9, 7)),
            Application.Tests.Dtos.TestClasses.Faker.FakeTrendMetricsByDate(new DateOnly(2024, 9, 8)),
        };
        stubMetricMapper.Setup(x => x.Map(listing, forecast, player, FakePlayerSeasonPerformanceResponse.MetricsByDate,
                MetricDatesAndTimes))
            .Returns(new TrendReportFactory.PerformanceMetrics(5, 25.1m, expectedMetrics));

        var factory = new TrendReportFactory(stubPlayerCardRepository.Object, stubListingRepository.Object,
            stubForecastRepository.Object, stubPerformanceApi.Object, stubPlayerMatcher.Object, stubCalendar.Object,
            stubClock.Object, stubOrderCountMapper.Object, stubPriceMapper.Object, stubMetricMapper.Object);

        // Act
        var actual = await factory.GetReport(playerCard.Year, playerCard.ExternalId, cToken);

        // Assert
        Assert.Equal(playerCard.Year, actual.Year);
        Assert.Equal(playerCard.ExternalId, actual.CardExternalId);
        Assert.Equal(player.MlbId, actual.MlbId);
        Assert.Equal(playerCard.Name, actual.CardName);
        Assert.Equal(playerCard.Position, actual.PrimaryPosition);
        Assert.Equal(playerCard.OverallRating, actual.OverallRating);
        Assert.True(actual.IsBoosted);
        Assert.Equal(1, actual.Orders1H);
        Assert.Equal(24, actual.Orders24H);
        Assert.Equal(1, actual.BuyPrice);
        Assert.Equal(2, actual.BuyPriceChange24H);
        Assert.Equal(3, actual.SellPrice);
        Assert.Equal(4, actual.SellPriceChange24H);
        Assert.Equal(5, actual.Score);
        Assert.Equal(25.1m, actual.ScoreChange2W);
        Assert.Equal(1, actual.Demand);

        Assert.Equal(expectedMetrics, actual.MetricsByDate);

        Assert.Equal(ExpectedReport.Impacts.Count, actual.Impacts.Count);
        Assert.True(ExpectedReport.Impacts.SequenceEqual(actual.Impacts));
    }

    [Fact]
    public async Task GetReport_YearAndMlbIdAndMissingData_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;
        PlayerCard? playerCard = null;
        var listing = Listing();
        var forecast = Forecast();

        var stubPlayerCardRepository = new Mock<IPlayerCardRepository>();
        stubPlayerCardRepository.Setup(x => x.GetByExternalId(forecast.CardExternalId))
            .ReturnsAsync(playerCard);

        var stubListingRepository = new Mock<IListingRepository>();
        stubListingRepository.Setup(x => x.GetByExternalId(forecast.CardExternalId, cToken))
            .ReturnsAsync(listing);

        var stubForecastRepository = new Mock<IForecastRepository>();
        stubForecastRepository.Setup(x => x.GetBy(forecast.Year, forecast.MlbId!))
            .ReturnsAsync(forecast);

        var mockPerformanceApi = Mock.Of<IPerformanceApi>();

        var mockPlayerMatcher = Mock.Of<IPlayerMatcher>();

        var stubCalendar = new Mock<ICalendar>();
        stubCalendar.Setup(x => x.Today())
            .Returns(MetricDatesAndTimes.Today);

        var mockClock = Mock.Of<IClock>();

        var factory = new TrendReportFactory(stubPlayerCardRepository.Object, stubListingRepository.Object,
            stubForecastRepository.Object, mockPerformanceApi, mockPlayerMatcher, stubCalendar.Object, mockClock);

        var action = async () => await factory.GetReport(forecast.Year, forecast.MlbId!, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<TrendReportFactoryMissingDataException>(actual);
    }

    [Fact]
    public async Task GetReport_YearAndMlbId_ReturnsTrendReport()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var playerCard = PlayerCard();
        var listing = Listing();
        var forecast = Forecast();
        var player = Player();

        var stubPlayerCardRepository = new Mock<IPlayerCardRepository>();
        stubPlayerCardRepository.Setup(x => x.GetByExternalId(playerCard.ExternalId))
            .ReturnsAsync(playerCard);

        var stubListingRepository = new Mock<IListingRepository>();
        stubListingRepository.Setup(x => x.GetByExternalId(playerCard.ExternalId, cToken))
            .ReturnsAsync(listing);

        var stubForecastRepository = new Mock<IForecastRepository>();
        stubForecastRepository.Setup(x => x.GetBy(forecast.Year, forecast.MlbId!))
            .ReturnsAsync(forecast);

        var stubPerformanceApi = new Mock<IPerformanceApi>();
        stubPerformanceApi.Setup(x =>
                x.GetPlayerSeasonPerformance(playerCard.Year.Value, player.MlbId.Value, "2024-03-01", "2024-09-08"))
            .ReturnsAsync(new ApiResponse<PlayerSeasonPerformanceResponse>(new HttpResponseMessage(HttpStatusCode.OK),
                FakePlayerSeasonPerformanceResponse, new RefitSettings()));

        var stubPlayerMatcher = new Mock<IPlayerMatcher>();
        stubPlayerMatcher.Setup(x => x.GetPlayerByName(playerCard.Name, playerCard.TeamShortName))
            .ReturnsAsync(player);

        var stubCalendar = new Mock<ICalendar>();
        stubCalendar.Setup(x => x.Today())
            .Returns(MetricDatesAndTimes.Today);

        var stubClock = new Mock<IClock>();
        stubClock.Setup(x => x.UtcNow())
            .Returns(Now);

        var stubOrderCountMapper = new Mock<TrendReportFactory.IOrderCountMapper>();
        stubOrderCountMapper.Setup(x => x.Map(listing, MetricDatesAndTimes))
            .Returns(new TrendReportFactory.OrderCount(1, 24));

        var stubPriceMapper = new Mock<TrendReportFactory.IPriceMapper>();
        stubPriceMapper.Setup(x => x.Map(listing, MetricDatesAndTimes))
            .Returns(new TrendReportFactory.Prices(1, 2, 3, 4));

        var stubMetricMapper = new Mock<TrendReportFactory.IPerformanceMetricMapper>();
        var expectedMetrics = new List<TrendMetricsByDate>()
        {
            Application.Tests.Dtos.TestClasses.Faker.FakeTrendMetricsByDate(new DateOnly(2024, 9, 7)),
            Application.Tests.Dtos.TestClasses.Faker.FakeTrendMetricsByDate(new DateOnly(2024, 9, 8)),
        };
        stubMetricMapper.Setup(x => x.Map(listing, forecast, player, FakePlayerSeasonPerformanceResponse.MetricsByDate,
                MetricDatesAndTimes))
            .Returns(new TrendReportFactory.PerformanceMetrics(5, 25.1m, expectedMetrics));

        var factory = new TrendReportFactory(stubPlayerCardRepository.Object, stubListingRepository.Object,
            stubForecastRepository.Object, stubPerformanceApi.Object, stubPlayerMatcher.Object, stubCalendar.Object,
            stubClock.Object, stubOrderCountMapper.Object, stubPriceMapper.Object, stubMetricMapper.Object);

        // Act
        var actual = await factory.GetReport(playerCard.Year, forecast.MlbId!, cToken);

        // Assert
        Assert.Equal(playerCard.Year, actual.Year);
        Assert.Equal(playerCard.ExternalId, actual.CardExternalId);
        Assert.Equal(player.MlbId, actual.MlbId);
        Assert.Equal(playerCard.Name, actual.CardName);
        Assert.Equal(playerCard.Position, actual.PrimaryPosition);
        Assert.Equal(playerCard.OverallRating, actual.OverallRating);
        Assert.True(actual.IsBoosted);
        Assert.Equal(1, actual.Orders1H);
        Assert.Equal(24, actual.Orders24H);
        Assert.Equal(1, actual.BuyPrice);
        Assert.Equal(2, actual.BuyPriceChange24H);
        Assert.Equal(3, actual.SellPrice);
        Assert.Equal(4, actual.SellPriceChange24H);
        Assert.Equal(5, actual.Score);
        Assert.Equal(25.1m, actual.ScoreChange2W);
        Assert.Equal(1, actual.Demand);

        Assert.Equal(expectedMetrics, actual.MetricsByDate);

        Assert.Equal(ExpectedReport.Impacts.Count, actual.Impacts.Count);
        Assert.True(ExpectedReport.Impacts.SequenceEqual(actual.Impacts));
    }

    [Fact]
    public void MetricDatesAndTimes_Values_BeforeEndOfSeason_MapsDatesAndTimes()
    {
        // Arrange
        var season = SeasonYear.Create(2024);
        var today = new DateOnly(2024, 8, 2);
        var now = new DateTime(2024, 8, 2, 4, 0, 0, DateTimeKind.Utc);
        var metricDatesAndTimes = new TrendReportFactory.MetricDatesAndTimes(season, today, now);

        // Act
        var actualYesterday = metricDatesAndTimes.Yesterday;
        var actualEndOfSeason = metricDatesAndTimes.EndOfSeason;
        var actualScoreDate = metricDatesAndTimes.ScoreDate;
        var actualScoreDate2W = metricDatesAndTimes.ScoreDate2W;
        var actualOneHourAgo = metricDatesAndTimes.OneHourAgo;
        var actualOneDayAgo = metricDatesAndTimes.OneDayAgo;

        // Assert
        Assert.Equal(new DateOnly(2024, 8, 1), actualYesterday);
        Assert.Equal(new DateOnly(2024, 10, 1), actualEndOfSeason);
        Assert.Equal(new DateOnly(2024, 8, 2), actualScoreDate);
        Assert.Equal(new DateOnly(2024, 8, 2).AddDays(-14), actualScoreDate2W);
        Assert.Equal(new DateTime(2024, 8, 2, 3, 0, 0, 0, DateTimeKind.Utc), actualOneHourAgo);
        Assert.Equal(new DateTime(2024, 8, 1, 4, 0, 0, 0, DateTimeKind.Utc), actualOneDayAgo);
    }

    [Fact]
    public void MetricDatesAndTimes_Values_AfterEndOfSeason_MapsDatesAndTimes()
    {
        // Arrange
        var season = SeasonYear.Create(2024);
        var today = new DateOnly(2024, 10, 20);
        var now = new DateTime(2024, 10, 20, 4, 0, 0, DateTimeKind.Utc);
        var metricDatesAndTimes = new TrendReportFactory.MetricDatesAndTimes(season, today, now);

        // Act
        var actualYesterday = metricDatesAndTimes.Yesterday;
        var actualEndOfSeason = metricDatesAndTimes.EndOfSeason;
        var actualScoreDate = metricDatesAndTimes.ScoreDate;
        var actualScoreDate2W = metricDatesAndTimes.ScoreDate2W;
        var actualOneHourAgo = metricDatesAndTimes.OneHourAgo;
        var actualOneDayAgo = metricDatesAndTimes.OneDayAgo;

        // Assert
        Assert.Equal(new DateOnly(2024, 10, 19), actualYesterday);
        Assert.Equal(new DateOnly(2024, 10, 1), actualEndOfSeason);
        Assert.Equal(new DateOnly(2024, 10, 1), actualScoreDate);
        Assert.Equal(new DateOnly(2024, 10, 1).AddDays(-14), actualScoreDate2W);
        Assert.Equal(new DateTime(2024, 10, 20, 3, 0, 0, 0, DateTimeKind.Utc), actualOneHourAgo);
        Assert.Equal(new DateTime(2024, 10, 19, 4, 0, 0, 0, DateTimeKind.Utc), actualOneDayAgo);
    }

    [Fact]
    public void OrderCountMapper_Map_Listing_MapsOrderCounts()
    {
        // Arrange
        var mapper = new TrendReportFactory.OrderCountMapper();

        // Act
        var actual = mapper.Map(Listing(), MetricDatesAndTimes);

        // Assert
        Assert.Equal(1, actual.Orders1H);
        Assert.Equal(3, actual.Orders24H);
    }

    [Fact]
    public void PriceMapper_Map_Listing_MapsPrices()
    {
        // Arrange
        var mapper = new TrendReportFactory.PriceMapper();

        // Act
        var actual = mapper.Map(Listing(), MetricDatesAndTimes);

        // Assert
        Assert.Equal(123, actual.BuyPrice);
        Assert.Equal(-90, actual.BuyPriceChange24H);
        Assert.Equal(4560, actual.SellPrice);
        Assert.Equal(900, actual.SellPriceChange24H);
    }

    [Fact]
    public void PerformanceMetricMapper_Map_Metrics_MapsPerformance()
    {
        // Arrange
        var mapper = new TrendReportFactory.PerformanceMetricMapper();

        // Act
        var actual = mapper.Map(Listing(), Forecast(), Player(), FakePlayerSeasonPerformanceResponse.MetricsByDate,
            MetricDatesAndTimes);

        // Assert
        Assert.Equal(ExpectedReport.MetricsByDate.Count, actual.Metrics.Count);
        Assert.True(ExpectedReport.MetricsByDate.SequenceEqual(actual.Metrics));
    }
}