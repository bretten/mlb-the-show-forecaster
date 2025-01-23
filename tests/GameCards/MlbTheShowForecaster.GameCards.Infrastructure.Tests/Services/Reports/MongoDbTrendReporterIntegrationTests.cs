using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Dtos.TestClasses;
using MongoDB.Driver;
using Moq;
using Testcontainers.MongoDb;
using Faker = com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses.Faker;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Services.Reports;

public class MongoDbTrendReporterIntegrationTests : IAsyncLifetime
{
    private readonly MongoDbContainer _container;
    private const string U = "mongo";
    private const string P = "password99";
    private const int Port = 27017;

    private int HostPort => _container.GetMappedPublicPort(Port);

    /// <summary>
    /// Configures the container options that will be used for each test
    /// </summary>
    /// <exception cref="DockerNotRunningException">Thrown if Docker is not running</exception>
    public MongoDbTrendReporterIntegrationTests()
    {
        try
        {
            _container = new MongoDbBuilder()
                .WithName(GetType().Name + Guid.NewGuid())
                .WithUsername(U)
                .WithPassword(P)
                .WithPortBinding(27017, true)
                .Build();
        }
        catch (ArgumentException e)
        {
            if (!e.Message.Contains("Docker is either not running or misconfigured"))
            {
                throw;
            }

            throw new DockerNotRunningException($"Docker is required to run tests for {GetType().Name}");
        }
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task UpdateTrendReport_YearAndCardExternalId_InsertsReport()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var trendReport = Faker.FakeTrendReport(year: 2024, externalId: Faker.FakeGuid1, mlbId: 100,
            orders1H: 1,
            orders24H: 2,
            buyPrice: 3,
            buyPriceChange24H: 4,
            sellPrice: 5,
            sellPriceChange24H: 6,
            score: 7,
            scoreChange2W: 8,
            metricsByDate: new List<TrendMetricsByDate>()
            {
                Faker.FakeTrendMetricsByDate(new DateOnly(2024, 10, 8)),
                Faker.FakeTrendMetricsByDate(new DateOnly(2024, 10, 9)),
            }, impacts: new List<TrendImpact>()
            {
                Faker.FakeTrendImpact(new DateOnly(2024, 10, 8), new DateOnly(2024, 10, 9)),
                Faker.FakeTrendImpact(new DateOnly(2024, 10, 9), new DateOnly(2024, 10, 10)),
            });

        var stubTrendReportFactory = new Mock<ITrendReportFactory>();
        stubTrendReportFactory.Setup(x => x.GetReport(trendReport.Year, trendReport.CardExternalId, cToken))
            .ReturnsAsync(trendReport);

        var mongoClient = GetMongoClient();
        var mongoConfig = GetMongoConfig();

        var reporter = new MongoDbTrendReporter(stubTrendReportFactory.Object, mongoClient, mongoConfig);
        var (db, collection) =
            GetDbAndCollection(mongoClient, mongoConfig); // Serializer registers twice if before reporter

        // Act
        await reporter.UpdateTrendReport(trendReport.Year, trendReport.CardExternalId, cToken);

        // Assert
        var actual = collection.Find(FilterDefinition<TrendReport>.Empty).First();
        Asserter.Equal(expected: trendReport, actual);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task UpdateTrendReport_YearAndMlbId_InsertsReport()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var trendReport = Faker.FakeTrendReport(year: 2024, externalId: Faker.FakeGuid1, mlbId: 100,
            orders1H: 1,
            orders24H: 2,
            buyPrice: 3,
            buyPriceChange24H: 4,
            sellPrice: 5,
            sellPriceChange24H: 6,
            score: 7,
            scoreChange2W: 8,
            metricsByDate: new List<TrendMetricsByDate>()
            {
                Faker.FakeTrendMetricsByDate(new DateOnly(2024, 10, 8)),
                Faker.FakeTrendMetricsByDate(new DateOnly(2024, 10, 9)),
            }, impacts: new List<TrendImpact>()
            {
                Faker.FakeTrendImpact(new DateOnly(2024, 10, 8), new DateOnly(2024, 10, 9)),
                Faker.FakeTrendImpact(new DateOnly(2024, 10, 9), new DateOnly(2024, 10, 10)),
            });

        var stubTrendReportFactory = new Mock<ITrendReportFactory>();
        stubTrendReportFactory.Setup(x => x.GetReport(trendReport.Year, trendReport.MlbId, cToken))
            .ReturnsAsync(trendReport);

        var mongoClient = GetMongoClient();
        var mongoConfig = GetMongoConfig();

        var reporter = new MongoDbTrendReporter(stubTrendReportFactory.Object, mongoClient, mongoConfig);
        var (db, collection) =
            GetDbAndCollection(mongoClient, mongoConfig); // Serializer registers twice if before reporter

        // Act
        await reporter.UpdateTrendReport(trendReport.Year, trendReport.MlbId, cToken);

        // Assert
        var actual = collection.Find(FilterDefinition<TrendReport>.Empty).First();
        Asserter.Equal(expected: trendReport, actual);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task UpdateTrendReport_YearAndCardExternalId_UpdatesReport()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var trendReport = Faker.FakeTrendReport(year: 2024, externalId: Faker.FakeGuid1, mlbId: 100,
            orders1H: 1,
            orders24H: 2,
            buyPrice: 3,
            buyPriceChange24H: 4,
            sellPrice: 5,
            sellPriceChange24H: 6,
            score: 7,
            scoreChange2W: 8,
            metricsByDate: new List<TrendMetricsByDate>()
            {
                Faker.FakeTrendMetricsByDate(new DateOnly(2024, 10, 8), 0, 0, null, false, null, false, null, false,
                    null, null, null, null, null, null, null, null, null),
                Faker.FakeTrendMetricsByDate(new DateOnly(2024, 10, 9)),
            }, impacts: new List<TrendImpact>()
            {
                Faker.FakeTrendImpact(new DateOnly(2024, 10, 8), new DateOnly(2024, 10, 9)),
                Faker.FakeTrendImpact(new DateOnly(2024, 10, 9), new DateOnly(2024, 10, 10)),
            });

        var stubTrendReportFactory = new Mock<ITrendReportFactory>();
        stubTrendReportFactory.Setup(x => x.GetReport(trendReport.Year, trendReport.CardExternalId, cToken))
            .ReturnsAsync(trendReport);

        var mongoClient = GetMongoClient();
        var mongoConfig = GetMongoConfig();

        var reporter = new MongoDbTrendReporter(stubTrendReportFactory.Object, mongoClient, mongoConfig);
        var (db, collection) =
            GetDbAndCollection(mongoClient, mongoConfig); // Serializer registers twice if before reporter

        await reporter.UpdateTrendReport(trendReport.Year, trendReport.CardExternalId, cToken); // Insert
        var trendReport2 = Faker.FakeTrendReport(year: 2024, externalId: Faker.FakeGuid1, mlbId: 100, overallRating: 99,
            orders1H: 10,
            orders24H: 20,
            buyPrice: 30,
            buyPriceChange24H: 40,
            sellPrice: 50,
            sellPriceChange24H: 60,
            score: 70,
            scoreChange2W: 80,
            metricsByDate: new List<TrendMetricsByDate>()
            {
                Faker.FakeTrendMetricsByDate(new DateOnly(2024, 10, 8)),
                Faker.FakeTrendMetricsByDate(new DateOnly(2024, 10, 9)),
                Faker.FakeTrendMetricsByDate(new DateOnly(2024, 10, 10)),
                Faker.FakeTrendMetricsByDate(new DateOnly(2024, 10, 11)),
            }, impacts: new List<TrendImpact>()
            {
                Faker.FakeTrendImpact(new DateOnly(2024, 10, 8), new DateOnly(2024, 10, 9)),
                Faker.FakeTrendImpact(new DateOnly(2024, 10, 9), new DateOnly(2024, 10, 10)),
                Faker.FakeTrendImpact(new DateOnly(2024, 10, 10), new DateOnly(2024, 10, 11)),
            });
        stubTrendReportFactory.Setup(x => x.GetReport(trendReport2.Year, trendReport2.CardExternalId, cToken))
            .ReturnsAsync(trendReport2);


        // Act
        await reporter.UpdateTrendReport(trendReport2.Year, trendReport2.CardExternalId, cToken);

        // Assert
        var actual = collection.Find(FilterDefinition<TrendReport>.Empty).First();
        Asserter.Equal(expected: trendReport2, actual);
    }

    [Theory]
    [Trait("Category", "Integration")]
    // Name, Asc
    [InlineData(1, 2, nameof(TrendReport.CardName), ITrendReporter.SortOrder.Asc, new[] { "Alan", "Dot" })]
    [InlineData(2, 2, nameof(TrendReport.CardName), ITrendReporter.SortOrder.Asc, new[] { "èrnie" })]
    // Name, Desc
    [InlineData(1, 2, nameof(TrendReport.CardName), ITrendReporter.SortOrder.Desc, new[] { "èrnie", "Dot" })]
    [InlineData(2, 2, nameof(TrendReport.CardName), ITrendReporter.SortOrder.Desc, new[] { "Alan" })]
    // OVR, Asc
    [InlineData(1, 2, nameof(TrendReport.OverallRating), ITrendReporter.SortOrder.Asc, new[] { "Alan", "èrnie" })]
    [InlineData(2, 2, nameof(TrendReport.OverallRating), ITrendReporter.SortOrder.Asc, new[] { "Dot" })]
    // OVR, Desc
    [InlineData(1, 2, nameof(TrendReport.OverallRating), ITrendReporter.SortOrder.Desc, new[] { "Dot", "èrnie" })]
    [InlineData(2, 2, nameof(TrendReport.OverallRating), ITrendReporter.SortOrder.Desc, new[] { "Alan" })]
    public async Task GetTrendReports_YearAndCardExternalId_InsertsReport(int page, int pageSize, string sortField,
        ITrendReporter.SortOrder sortOrder, string[] expectedCardNames)
    {
        // Arrange
        var cToken = CancellationToken.None;
        var trendReport1 = Faker.FakeTrendReport(year: 2024, externalId: Faker.FakeGuid1, mlbId: 100, cardName: "Alan",
            overallRating: 60);
        var trendReport2 = Faker.FakeTrendReport(year: 2024, externalId: Faker.FakeGuid2, mlbId: 200, cardName: "Dot",
            overallRating: 99);
        var trendReport3 = Faker.FakeTrendReport(year: 2024, externalId: Faker.FakeGuid3, mlbId: 300, cardName: "èrnie",
            overallRating: 70);

        var stubTrendReportFactory = new Mock<ITrendReportFactory>();
        stubTrendReportFactory.Setup(x => x.GetReport(trendReport1.Year, trendReport1.CardExternalId, cToken))
            .ReturnsAsync(trendReport1);
        stubTrendReportFactory.Setup(x => x.GetReport(trendReport2.Year, trendReport2.CardExternalId, cToken))
            .ReturnsAsync(trendReport2);
        stubTrendReportFactory.Setup(x => x.GetReport(trendReport3.Year, trendReport3.CardExternalId, cToken))
            .ReturnsAsync(trendReport3);

        var mongoClient = GetMongoClient();
        var mongoConfig = GetMongoConfig();

        var reporter = new MongoDbTrendReporter(stubTrendReportFactory.Object, mongoClient, mongoConfig);

        await reporter.UpdateTrendReport(trendReport1.Year, trendReport1.CardExternalId, cToken);
        await reporter.UpdateTrendReport(trendReport2.Year, trendReport2.CardExternalId, cToken);
        await reporter.UpdateTrendReport(trendReport3.Year, trendReport3.CardExternalId, cToken);

        var year = trendReport1.Year;

        // Act
        var paginatedResult = await reporter.GetTrendReports(year, page, pageSize, sortField, sortOrder, cToken);
        var actual = paginatedResult.Items.ToList();

        // Assert
        for (var i = 0; i < expectedCardNames.Length; i++)
        {
            var expectedCardName = expectedCardNames[i];
            var correspondingItem = actual[i];
            Assert.True(expectedCardName == correspondingItem.CardName.Value);

            switch (correspondingItem.CardName.Value)
            {
                case "Alan":
                    Asserter.Equal(trendReport1, correspondingItem);
                    break;
                case "Dot":
                    Asserter.Equal(trendReport2, correspondingItem);
                    break;
                case "èrnie":
                    Asserter.Equal(trendReport3, correspondingItem);
                    break;
            }
        }
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public async Task DisposeAsync() => await _container.DisposeAsync();

    private IMongoClient GetMongoClient()
    {
        return new MongoClient($"mongodb://{U}:{P}@localhost:{HostPort}/?authSource=admin");
    }

    private static MongoDbTrendReporter.MongoDbTrendReporterConfig GetMongoConfig(string database = "local",
        string collection = "trend-reports")
    {
        return new MongoDbTrendReporter.MongoDbTrendReporterConfig(
            Database: database,
            Collection: collection
        );
    }

    private static (IMongoDatabase, IMongoCollection<TrendReport>) GetDbAndCollection(IMongoClient client,
        MongoDbTrendReporter.MongoDbTrendReporterConfig config)
    {
        var db = client.GetDatabase(config.Database);
        var c = db.GetCollection<TrendReport>(config.Collection);
        return (db, c);
    }

    private sealed class DockerNotRunningException : Exception
    {
        public DockerNotRunningException(string? message) : base(message)
        {
        }
    }
}