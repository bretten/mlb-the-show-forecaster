using System.Text.RegularExpressions;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Forecasts.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Reports.Exceptions;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Services.Reports;

public partial class StringReplacementForecastReportGeneratorTests
{
    private static readonly string LayoutTemplatePath =
        $"Services{Path.DirectorySeparatorChar}Reports{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}Layout.template.html";

    private static readonly string PlayerCardForecastTemplatePath =
        $"Services{Path.DirectorySeparatorChar}Reports{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}PlayerCardForecast.template.html";

    private static readonly string ForecastImpactTemplatePath =
        $"Services{Path.DirectorySeparatorChar}Reports{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}ForecastImpact.template.html";

    [Fact]
    public void Constructor_MissingTemplate_ThrowsException()
    {
        // Arrange
        var options = new ForecastReportOptions(templates: new Dictionary<string, string>()
        {
            { "Layout", @"Path\Does\Not\Exist" },
            { "PlayerCardForecast", PlayerCardForecastTemplatePath },
            { "ForecastImpact", ForecastImpactTemplatePath },
        });

        var action = () => new StringReplacementForecastReportGenerator(options, Mock.Of<IPlayerCardRepository>());

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<FileNotFoundException>(actual);
    }

    [Fact]
    public async Task Generate_MissingForecast_ThrowsException()
    {
        // Arrange
        var date = Faker.EndDate.AddDays(-5); // All the forecast impacts will default to an end date after this date
        var forecast = Faker.FakePlayerCardForecast(externalId: Faker.FakeGuid1);
        forecast.Reassess(Faker.FakeBattingStatsForecastImpact(oldScore: 0.2m, newScore: 0.5m), date);

        var stubPlayerCardRepo = new Mock<IPlayerCardRepository>();
        stubPlayerCardRepo.Setup(x => x.GetByExternalId(forecast.CardExternalId))
            .ReturnsAsync((PlayerCard?)null);

        var options = new ForecastReportOptions(templates: new Dictionary<string, string>()
        {
            { "Layout", LayoutTemplatePath },
            { "PlayerCardForecast", PlayerCardForecastTemplatePath },
            { "ForecastImpact", ForecastImpactTemplatePath },
        });
        var generator = new StringReplacementForecastReportGenerator(options, stubPlayerCardRepo.Object);

        var action = async () =>
            await generator.Generate(SeasonYear.Create(2024), new List<PlayerCardForecast>() { forecast }, date);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<NoPlayerCardForForecastReportException>(actual);
    }

    [Fact]
    public async Task Generate_PlayerCardForecasts_CreatesReport()
    {
        /*
         * Arrange
         */
        var date = Faker.EndDate.AddDays(-5); // All the forecast impacts will default to an end date after this date
        var forecasts = new List<PlayerCardForecast>();

        // Corresponding player cards
        var playerCard1 = Domain.Tests.Cards.TestClasses.Faker.FakePlayerCard(externalId: Faker.FakeGuid1, name: "n1");
        var playerCard2 = Domain.Tests.Cards.TestClasses.Faker.FakePlayerCard(externalId: Faker.FakeGuid2, name: "n2");
        var playerCard3 = Domain.Tests.Cards.TestClasses.Faker.FakePlayerCard(externalId: Faker.FakeGuid3, name: "n3");
        var playerCard4 = Domain.Tests.Cards.TestClasses.Faker.FakePlayerCard(externalId: Faker.FakeGuid4, name: "n4");

        // Gainer forecasts
        var gainerForecast1 = Faker.FakePlayerCardForecast(externalId: Faker.FakeGuid1);
        gainerForecast1.Reassess(Faker.FakeBattingStatsForecastImpact(oldScore: 0.2m, newScore: 0.5m), date);
        gainerForecast1.Reassess(Faker.FakeFieldingStatsForecastImpact(oldScore: 0.2m, newScore: 0.5m), date);
        gainerForecast1.Reassess(
            Faker.FakeBoostForecastImpact("Blasted a walk-off home run to cap off an Astros comeback on 8/19."), date);
        gainerForecast1.Reassess(Faker.FakeOverallRatingChangeForecastImpact(60, 80), date);
        gainerForecast1.Reassess(Faker.FakePositionChangeForecastImpact(Position.RightField, Position.LeftField), date);
        gainerForecast1.Reassess(Faker.FakePlayerFreeAgencyForecastImpact(date.AddMonths(-1)), date); // No influence

        var gainerForecast2 = Faker.FakePlayerCardForecast(externalId: Faker.FakeGuid2, position: Position.Pitcher);
        gainerForecast2.Reassess(Faker.FakePitchingStatsForecastImpact(oldScore: 0.2m, newScore: 0.5m), date);
        gainerForecast2.Reassess(Faker.FakePlayerActivationForecastImpact(), date);
        gainerForecast2.Reassess(Faker.FakePlayerTeamSigningForecastImpact(), date);
        gainerForecast2.Reassess(Faker.FakePlayerFreeAgencyForecastImpact(date.AddMonths(-1)), date); // No influence

        // Loser forecasts
        var loserForecast1 = Faker.FakePlayerCardForecast(externalId: Faker.FakeGuid3);
        loserForecast1.Reassess(Faker.FakeBattingStatsForecastImpact(oldScore: 0.5m, newScore: 0.2m), date);
        loserForecast1.Reassess(Faker.FakeFieldingStatsForecastImpact(oldScore: 0.5m, newScore: 0.2m), date);
        loserForecast1.Reassess(Faker.FakePlayerActivationForecastImpact(date.AddMonths(-1)), date); // No influence

        var loserForecast2 = Faker.FakePlayerCardForecast(externalId: Faker.FakeGuid4, position: Position.Pitcher);
        loserForecast2.Reassess(Faker.FakePitchingStatsForecastImpact(oldScore: 0.5m, newScore: 0.2m), date);
        loserForecast2.Reassess(Faker.FakePlayerFreeAgencyForecastImpact(), date);
        loserForecast2.Reassess(Faker.FakePlayerDeactivationForecastImpact(), date);
        loserForecast2.Reassess(Faker.FakePlayerActivationForecastImpact(date.AddMonths(-1)), date); // No influence

        // Input
        forecasts.Add(gainerForecast1);
        forecasts.Add(gainerForecast2);
        forecasts.Add(loserForecast1);
        forecasts.Add(loserForecast2);

        // PlayerCard repo
        var stubPlayerCardRepo = new Mock<IPlayerCardRepository>();
        stubPlayerCardRepo.Setup(x => x.GetByExternalId(gainerForecast1.CardExternalId))
            .ReturnsAsync(playerCard1);
        stubPlayerCardRepo.Setup(x => x.GetByExternalId(gainerForecast2.CardExternalId))
            .ReturnsAsync(playerCard2);
        stubPlayerCardRepo.Setup(x => x.GetByExternalId(loserForecast1.CardExternalId))
            .ReturnsAsync(playerCard3);
        stubPlayerCardRepo.Setup(x => x.GetByExternalId(loserForecast2.CardExternalId))
            .ReturnsAsync(playerCard4);

        // Generator
        var options = new ForecastReportOptions(templates: new Dictionary<string, string>()
        {
            { "Layout", LayoutTemplatePath },
            { "PlayerCardForecast", PlayerCardForecastTemplatePath },
            { "ForecastImpact", ForecastImpactTemplatePath },
        });
        var generator = new StringReplacementForecastReportGenerator(options, stubPlayerCardRepo.Object);

        /*
         * Act
         */
        var actual = await generator.Generate(SeasonYear.Create(2024), forecasts, date);

        /*
         * Assert
         */
        var expectedHtml = Whitespace()
            .Replace(
                (await File.ReadAllTextAsync(
                    @"Services\Reports\TestFiles\StringReplacementForecastGenerator_Expected.html")).Trim(), " ");
        var actualHtml = Whitespace().Replace(actual.Html.Trim(), " ");
        Assert.Equal(expectedHtml, actualHtml);
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex Whitespace();
}