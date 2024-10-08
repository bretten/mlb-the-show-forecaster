using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Reports;
using MongoDB.Driver;
using Moq;
using Testcontainers.MongoDb;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Services.Reports;

public class MongoDbTrendReporterIntegrationTests : IAsyncLifetime
{
    private readonly MongoDbContainer _container;
    private const string U = "mongo";
    private const string P = "password99";
    private const int Port = 50000;

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
                .WithPortBinding(Port, 27017)
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
        //Assert.Equal(trendReport, actual);
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public async Task DisposeAsync() => await _container.StopAsync();

    private static IMongoClient GetMongoClient()
    {
        return new MongoClient($"mongodb://{U}:{P}@localhost:{Port}/?authSource=admin");
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