using System.Data.Common;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Forecasts.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Forecasts.EntityFrameworkCore;
using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Testcontainers.PostgreSql;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Forecasts.EntityFrameworkCore;

public class EntityFrameworkCoreForecastRepositoryIntegrationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    /// <summary>
    /// Configures the container options that will be used for each test
    /// </summary>
    /// <exception cref="DockerNotRunningException">Thrown if Docker is not running</exception>
    public EntityFrameworkCoreForecastRepositoryIntegrationTests()
    {
        try
        {
            _container = new PostgreSqlBuilder()
                .WithName(GetType().Name + Guid.NewGuid())
                .WithUsername("postgres")
                .WithPassword("password99")
                .WithPortBinding(5432, true)
                .WithWaitStrategy(Wait.ForUnixContainer()
                    .UntilPortIsAvailable(5432, o => o.WithTimeout(TimeSpan.FromMinutes(1)))
                    .UntilCommandIsCompleted(["pg_isready", "-U", "postgres", "-d", "postgres"],
                        o => o.WithTimeout(TimeSpan.FromMinutes(1)))
                )
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
    public async Task Add_PlayerCardForecast_AddsPlayerCardForecastToDbContextSet()
    {
        // Arrange
        var fakePlayerCardForecast = Faker.FakePlayerCardForecast(externalId: Faker.FakeGuid1);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        var repo = new EntityFrameworkCoreForecastRepository(dbContext);

        // Act
        await repo.Add(fakePlayerCardForecast);
        await dbContext.SaveChangesAsync();

        // Assert
        Assert.NotNull(dbContext.PlayerCardForecasts);
        Assert.Equal(1, dbContext.PlayerCardForecasts.Count());
        Assert.Equal(fakePlayerCardForecast, dbContext.PlayerCardForecasts.First());
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Update_PlayerCardForecast_UpdatesPlayerCardForecastInDbContextSet()
    {
        // Arrange
        var fakePlayerCardForecast = Faker.FakePlayerCardForecast(externalId: Faker.FakeGuid1, mlbId: null);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        await dbContext.AddAsync(fakePlayerCardForecast);
        await dbContext.SaveChangesAsync();

        var repo = new EntityFrameworkCoreForecastRepository(dbContext);

        // Act
        fakePlayerCardForecast.SetMlbId(MlbId.Create(5));
        fakePlayerCardForecast.Reassess(Faker.FakePlayerActivationForecastImpact(), Faker.EndDate.AddDays(-5));
        await repo.Update(fakePlayerCardForecast);
        await dbContext.SaveChangesAsync();

        // Assert
        await using var assertConnection =
            await GetDbConnection(); // Re-create the context so that the record is freshly retrieved from the database
        await using var assertDbContext = GetDbContext(assertConnection);
        await assertDbContext.Database.MigrateAsync();
        var assertRepo = new EntityFrameworkCoreForecastRepository(dbContext);
        var actual = await assertRepo.GetBy(fakePlayerCardForecast.Year, fakePlayerCardForecast.CardExternalId);
        Assert.NotNull(actual);
        Assert.Equal(fakePlayerCardForecast, actual);
        Assert.Equal(5, actual.MlbId!.Value);
        Assert.Equal(1, actual.ForecastImpactsChronologically.Count);
        Assert.IsType<PlayerActivationForecastImpact>(actual.ForecastImpactsChronologically[0]);
        Assert.Equal(Faker.StartDate, actual.ForecastImpactsChronologically[0].StartDate);
        Assert.Equal(Faker.EndDate, actual.ForecastImpactsChronologically[0].EndDate);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetByCardExternalId_CardExternalId_ReturnsPlayerCardForecastFromDbContextSet()
    {
        // Arrange
        var year = SeasonYear.Create(2024);
        var fakePlayerCardForecast = Faker.FakePlayerCardForecast(year: year.Value, externalId: Faker.FakeGuid1);
        fakePlayerCardForecast.Reassess(Faker.FakePlayerActivationForecastImpact(), Faker.EndDate.AddDays(-3));

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        await dbContext.AddAsync(fakePlayerCardForecast);
        await dbContext.SaveChangesAsync();

        await using var assertConnection =
            await GetDbConnection(); // Re-create the context so that the record is freshly retrieved from the database
        await using var assertDbContext = GetDbContext(assertConnection);
        await assertDbContext.Database.MigrateAsync();
        var repo = new EntityFrameworkCoreForecastRepository(assertDbContext);

        // Act
        var actual = await repo.GetBy(year, fakePlayerCardForecast.CardExternalId);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(fakePlayerCardForecast, actual);
        Assert.Single(actual.ForecastImpactsChronologically);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetByMlbId_MlbId_ReturnsPlayerCardForecastFromDbContextSet()
    {
        // Arrange
        var year = SeasonYear.Create(2024);
        var fakePlayerCardForecast = Faker.FakePlayerCardForecast(year: year.Value, mlbId: 1);
        fakePlayerCardForecast.Reassess(Faker.FakePlayerActivationForecastImpact(), Faker.EndDate.AddDays(-3));

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        await dbContext.AddAsync(fakePlayerCardForecast);
        await dbContext.SaveChangesAsync();

        await using var assertConnection =
            await GetDbConnection(); // Re-create the context so that the record is freshly retrieved from the database
        await using var assertDbContext = GetDbContext(assertConnection);
        await assertDbContext.Database.MigrateAsync();
        var repo = new EntityFrameworkCoreForecastRepository(assertDbContext);

        // Act
        var actual = await repo.GetBy(year, fakePlayerCardForecast.MlbId!);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(fakePlayerCardForecast, actual);
        Assert.Single(actual.ForecastImpactsChronologically);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetImpactedForecasts_Date_ReturnsAllImpactedForecasts()
    {
        /*
         * Arrange
         */
        var seasonYear = SeasonYear.Create(2024);
        var fakePlayerCardForecast1 =
            Faker.FakePlayerCardForecast(year: seasonYear.Value, externalId: Faker.FakeGuid1, mlbId: 1);
        var fakePlayerCardForecast2 =
            Faker.FakePlayerCardForecast(year: seasonYear.Value, externalId: Faker.FakeGuid2, mlbId: 2);
        var fakePlayerCardForecast3 =
            Faker.FakePlayerCardForecast(year: seasonYear.Value, externalId: Faker.FakeGuid3, mlbId: 3);

        var today = new DateOnly(2024, 8, 13);
        var dateHasPassed = new DateOnly(2024, 4, 1);
        var futureDate = new DateOnly(2024, 10, 1);

        // Forecast 1 is no longer impacted because its impact has already ended
        fakePlayerCardForecast1.Reassess(
            Faker.FakePlayerActivationForecastImpact(dateHasPassed.AddDays(-1), dateHasPassed), today);
        // Forecast 2 is still being influenced by its impact because its end date is in the future
        fakePlayerCardForecast2.Reassess(Faker.FakePlayerActivationForecastImpact(dateHasPassed, futureDate), today);
        // Forecast 3 has not yet been influenced
        fakePlayerCardForecast3.Reassess(
            Faker.FakePlayerActivationForecastImpact(futureDate, futureDate.AddDays(1)), today);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        await dbContext.AddAsync(fakePlayerCardForecast1);
        await dbContext.AddAsync(fakePlayerCardForecast2);
        await dbContext.AddAsync(fakePlayerCardForecast3);
        await dbContext.SaveChangesAsync();

        await using var assertConnection =
            await GetDbConnection(); // Re-create the context so that the record is freshly retrieved from the database
        await using var assertDbContext = GetDbContext(assertConnection);
        await assertDbContext.Database.MigrateAsync();
        var repo = new EntityFrameworkCoreForecastRepository(assertDbContext);

        /*
         * Act
         */
        var actual = await repo.GetImpactedForecasts(today);

        /*
         * Assert
         */
        Assert.NotNull(actual);
        var actualList = actual.ToList();
        Assert.Single(actualList);
        Assert.Equal(fakePlayerCardForecast2, actualList[0]);
        Assert.DoesNotContain(fakePlayerCardForecast1, actualList);
        Assert.DoesNotContain(fakePlayerCardForecast3, actualList);
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public async Task DisposeAsync() => await _container.DisposeAsync();

    private async Task<DbConnection> GetDbConnection()
    {
        NpgsqlConnection connection = new(_container.GetConnectionString() + ";Pooling=false;");
        await connection.OpenAsync();
        return connection;
    }

    private ForecastsDbContext GetDbContext(DbConnection connection)
    {
        var contextOptions = new DbContextOptionsBuilder<ForecastsDbContext>()
            .UseNpgsql(connection)
            .LogTo(Console.WriteLine)
            .Options;
        return new ForecastsDbContext(contextOptions);
    }

    private sealed class DockerNotRunningException : Exception
    {
        public DockerNotRunningException(string? message) : base(message)
        {
        }
    }
}