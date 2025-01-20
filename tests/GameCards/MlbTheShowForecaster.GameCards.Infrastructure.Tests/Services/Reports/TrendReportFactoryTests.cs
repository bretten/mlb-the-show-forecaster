using System.Net;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.DomainApis.PerformanceApi;
using com.brettnamba.MlbTheShowForecaster.DomainApis.PerformanceApi.Responses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Repositories;
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
    [Fact]
    public async Task GetReport_YearAndCardExternalIdAndMissingData_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var playerCard = Faker.FakePlayerCard(year: 2024, name: Faker.FakeCardName("Dottie"),
            externalId: Faker.FakeGuid1, position: Position.CenterField, overallRating: 99);
        var listing = Domain.Tests.Marketplace.TestClasses.Faker.FakeListing(buyPrice: 123, sellPrice: 456,
            historicalPrices: new List<ListingHistoricalPrice>()
            {
                Domain.Tests.Marketplace.TestClasses.Faker.FakeListingHistoricalPrice(new DateOnly(2024, 10, 8),
                    buyPrice: 1, sellPrice: 2)
            });
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
            .Returns(new DateOnly(2024, 10, 8));

        var factory = new TrendReportFactory(stubPlayerCardRepository.Object, stubListingRepository.Object,
            stubForecastRepository.Object, mockPerformanceApi, mockPlayerMatcher, stubCalendar.Object);

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
        var playerCard = Faker.FakePlayerCard(year: 2024, name: Faker.FakeCardName("Dottie"),
            externalId: Faker.FakeGuid1, position: Position.CenterField, overallRating: 99);
        var listing = Domain.Tests.Marketplace.TestClasses.Faker.FakeListing(buyPrice: 123, sellPrice: 456,
            historicalPrices: new List<ListingHistoricalPrice>()
            {
                Domain.Tests.Marketplace.TestClasses.Faker.FakeListingHistoricalPrice(new DateOnly(2024, 10, 8),
                    buyPrice: 1, sellPrice: 2)
            });
        var forecast = Domain.Tests.Forecasts.TestClasses.Faker.FakePlayerCardForecast(mlbId: 100);
        forecast.Reassess(Domain.Tests.Forecasts.TestClasses.Faker.FakeBoostForecastImpact(
                startDate: new DateOnly(2024, 10, 8), endDate: new DateOnly(2024, 10, 9)),
            new DateOnly(2024, 10, 9));
        var player = Application.Tests.Dtos.TestClasses.Faker.FakePlayer();

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
        stubPerformanceApi.Setup(x => x.GetPlayerSeasonPerformance(2024, 100, "2024-03-01", "2024-10-08"))
            .ReturnsAsync(new ApiResponse<PlayerSeasonPerformanceResponse>(new HttpResponseMessage(HttpStatusCode.OK),
                new PlayerSeasonPerformanceResponse()
                {
                    MetricsByDate = new List<PerformanceMetricsByDate>()
                    {
                        new PerformanceMetricsByDate(Date: new DateOnly(2024, 10, 8),
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
                }, new RefitSettings()));

        var stubPlayerMatcher = new Mock<IPlayerMatcher>();
        stubPlayerMatcher.Setup(x => x.GetPlayerByName(playerCard.Name, playerCard.TeamShortName))
            .ReturnsAsync(player);

        var stubCalendar = new Mock<ICalendar>();
        stubCalendar.Setup(x => x.Today())
            .Returns(new DateOnly(2024, 10, 8));

        var factory = new TrendReportFactory(stubPlayerCardRepository.Object, stubListingRepository.Object,
            stubForecastRepository.Object, stubPerformanceApi.Object, stubPlayerMatcher.Object, stubCalendar.Object);

        // Act
        var actual = await factory.GetReport(playerCard.Year, playerCard.ExternalId, cToken);

        // Assert
        Assert.Equal(2024, actual.Year.Value);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), actual.CardExternalId.Value);
        Assert.Equal(100, actual.MlbId.Value);
        Assert.Equal("Dottie", actual.CardName.Value);
        Assert.Equal(Position.CenterField, actual.PrimaryPosition);
        Assert.Equal(99, actual.OverallRating.Value);

        Assert.Single(actual.MetricsByDate);
        Assert.Equal(new DateOnly(2024, 10, 8), actual.MetricsByDate[0].Date);
        Assert.Equal(1, actual.MetricsByDate[0].BuyPrice);
        Assert.Equal(2, actual.MetricsByDate[0].SellPrice);
        Assert.Equal(0.1m, actual.MetricsByDate[0].BattingScore);
        Assert.True(actual.MetricsByDate[0].SignificantBattingParticipation);
        Assert.Equal(0.2m, actual.MetricsByDate[0].PitchingScore);
        Assert.True(actual.MetricsByDate[0].SignificantPitchingParticipation);
        Assert.Equal(0.3m, actual.MetricsByDate[0].FieldingScore);
        Assert.True(actual.MetricsByDate[0].SignificantFieldingParticipation);
        Assert.Equal(0.111m, actual.MetricsByDate[0].BattingAverage);
        Assert.Equal(0.112m, actual.MetricsByDate[0].OnBasePercentage);
        Assert.Equal(0.113m, actual.MetricsByDate[0].Slugging);
        Assert.Equal(0.114m, actual.MetricsByDate[0].EarnedRunAverage);
        Assert.Equal(0.115m, actual.MetricsByDate[0].OpponentsBattingAverage);
        Assert.Equal(0.116m, actual.MetricsByDate[0].StrikeoutsPer9);
        Assert.Equal(0.117m, actual.MetricsByDate[0].BaseOnBallsPer9);
        Assert.Equal(0.118m, actual.MetricsByDate[0].HomeRunsPer9);
        Assert.Equal(0.119m, actual.MetricsByDate[0].FieldingPercentage);

        Assert.Single(actual.Impacts);
        Assert.Equal(new DateOnly(2024, 10, 8), actual.Impacts[0].Start);
        Assert.Equal(new DateOnly(2024, 10, 9), actual.Impacts[0].End);
        Assert.Equal($"Player's card was boosted for: Hit 5 HRs", actual.Impacts[0].Description);
    }

    [Fact]
    public async Task GetReport_YearAndMlbIdAndMissingData_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;
        PlayerCard? playerCard = null;
        var listing = Domain.Tests.Marketplace.TestClasses.Faker.FakeListing(buyPrice: 123, sellPrice: 456,
            historicalPrices: new List<ListingHistoricalPrice>()
            {
                Domain.Tests.Marketplace.TestClasses.Faker.FakeListingHistoricalPrice(new DateOnly(2024, 10, 8),
                    buyPrice: 1, sellPrice: 2)
            });
        var forecast = Domain.Tests.Forecasts.TestClasses.Faker.FakePlayerCardForecast(mlbId: 100,
            externalId: Faker.FakeGuid1);

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
            .Returns(new DateOnly(2024, 10, 8));

        var factory = new TrendReportFactory(stubPlayerCardRepository.Object, stubListingRepository.Object,
            stubForecastRepository.Object, mockPerformanceApi, mockPlayerMatcher, stubCalendar.Object);

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
        var playerCard = Faker.FakePlayerCard(year: 2024, name: Faker.FakeCardName("Dottie"),
            externalId: Faker.FakeGuid1, position: Position.CenterField, overallRating: 99);
        var listing = Domain.Tests.Marketplace.TestClasses.Faker.FakeListing(buyPrice: 123, sellPrice: 456,
            historicalPrices: new List<ListingHistoricalPrice>()
            {
                Domain.Tests.Marketplace.TestClasses.Faker.FakeListingHistoricalPrice(new DateOnly(2024, 10, 8),
                    buyPrice: 1, sellPrice: 2)
            });
        var forecast = Domain.Tests.Forecasts.TestClasses.Faker.FakePlayerCardForecast(mlbId: 100,
            externalId: playerCard.ExternalId.Value);
        forecast.Reassess(Domain.Tests.Forecasts.TestClasses.Faker.FakeBoostForecastImpact(
                startDate: new DateOnly(2024, 10, 8), endDate: new DateOnly(2024, 10, 9)),
            new DateOnly(2024, 10, 9));
        var player = Application.Tests.Dtos.TestClasses.Faker.FakePlayer();

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
        stubPerformanceApi.Setup(x => x.GetPlayerSeasonPerformance(2024, 100, "2024-03-01", "2024-10-08"))
            .ReturnsAsync(new ApiResponse<PlayerSeasonPerformanceResponse>(new HttpResponseMessage(HttpStatusCode.OK),
                new PlayerSeasonPerformanceResponse()
                {
                    MetricsByDate = new List<PerformanceMetricsByDate>()
                    {
                        new PerformanceMetricsByDate(Date: new DateOnly(2024, 10, 8),
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
                }, new RefitSettings()));

        var stubPlayerMatcher = new Mock<IPlayerMatcher>();
        stubPlayerMatcher.Setup(x => x.GetPlayerByName(playerCard.Name, playerCard.TeamShortName))
            .ReturnsAsync(player);

        var stubCalendar = new Mock<ICalendar>();
        stubCalendar.Setup(x => x.Today())
            .Returns(new DateOnly(2024, 10, 8));

        var factory = new TrendReportFactory(stubPlayerCardRepository.Object, stubListingRepository.Object,
            stubForecastRepository.Object, stubPerformanceApi.Object, stubPlayerMatcher.Object, stubCalendar.Object);

        // Act
        var actual = await factory.GetReport(playerCard.Year, forecast.MlbId!, cToken);

        // Assert
        Assert.Equal(2024, actual.Year.Value);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), actual.CardExternalId.Value);
        Assert.Equal(100, actual.MlbId.Value);
        Assert.Equal("Dottie", actual.CardName.Value);
        Assert.Equal(Position.CenterField, actual.PrimaryPosition);
        Assert.Equal(99, actual.OverallRating.Value);

        Assert.Single(actual.MetricsByDate);
        Assert.Equal(new DateOnly(2024, 10, 8), actual.MetricsByDate[0].Date);
        Assert.Equal(1, actual.MetricsByDate[0].BuyPrice);
        Assert.Equal(2, actual.MetricsByDate[0].SellPrice);
        Assert.Equal(0.1m, actual.MetricsByDate[0].BattingScore);
        Assert.True(actual.MetricsByDate[0].SignificantBattingParticipation);
        Assert.Equal(0.2m, actual.MetricsByDate[0].PitchingScore);
        Assert.True(actual.MetricsByDate[0].SignificantPitchingParticipation);
        Assert.Equal(0.3m, actual.MetricsByDate[0].FieldingScore);
        Assert.True(actual.MetricsByDate[0].SignificantFieldingParticipation);
        Assert.Equal(0.111m, actual.MetricsByDate[0].BattingAverage);
        Assert.Equal(0.112m, actual.MetricsByDate[0].OnBasePercentage);
        Assert.Equal(0.113m, actual.MetricsByDate[0].Slugging);
        Assert.Equal(0.114m, actual.MetricsByDate[0].EarnedRunAverage);
        Assert.Equal(0.115m, actual.MetricsByDate[0].OpponentsBattingAverage);
        Assert.Equal(0.116m, actual.MetricsByDate[0].StrikeoutsPer9);
        Assert.Equal(0.117m, actual.MetricsByDate[0].BaseOnBallsPer9);
        Assert.Equal(0.118m, actual.MetricsByDate[0].HomeRunsPer9);
        Assert.Equal(0.119m, actual.MetricsByDate[0].FieldingPercentage);

        Assert.Single(actual.Impacts);
        Assert.Equal(new DateOnly(2024, 10, 8), actual.Impacts[0].Start);
        Assert.Equal(new DateOnly(2024, 10, 9), actual.Impacts[0].End);
        Assert.Equal($"Player's card was boosted for: Hit 5 HRs", actual.Impacts[0].Description);
    }
}