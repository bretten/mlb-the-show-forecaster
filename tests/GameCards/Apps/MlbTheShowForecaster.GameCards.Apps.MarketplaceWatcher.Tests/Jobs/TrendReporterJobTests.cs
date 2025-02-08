using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs.Io;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;
using Microsoft.Extensions.Logging;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Tests.Jobs;

public class TrendReporterJobTests
{
    [Fact]
    public async Task Execute_JobInput_RunsJob()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var input = new SeasonJobInput(SeasonYear.Create(2024));
        var card1 = Faker.FakePlayerCard(externalId: Faker.FakeGuid1);
        var card2 = Faker.FakePlayerCard(externalId: Faker.FakeGuid2);
        var playerCards = new List<PlayerCard>() { card1, card2 };

        var stubPlayerCardRepository = new Mock<IPlayerCardRepository>();
        stubPlayerCardRepository.Setup(x => x.GetAll(input.Year))
            .ReturnsAsync(playerCards);

        var stubTrendReporter = new Mock<ITrendReporter>();
        stubTrendReporter.Setup(x => x.UpdateTrendReport(input.Year, card1.ExternalId, cToken))
            .Returns(Task.CompletedTask);
        stubTrendReporter.Setup(x => x.UpdateTrendReport(input.Year, card2.ExternalId, cToken))
            .ThrowsAsync(
                new TrendReportFactoryMissingDataException(card2, null, null, null, input.Year, card2.ExternalId));

        var mockLogger = Mock.Of<ILogger<TrendReporterJob>>();

        var job = new TrendReporterJob(stubPlayerCardRepository.Object, stubTrendReporter.Object, mockLogger);

        // Act
        var actual = await job.Execute(input, cToken);

        // Arrange
        Assert.Equal(2, actual.TotalPlayerCards);
        Assert.Equal(1, actual.UpdatedReports);
    }
}