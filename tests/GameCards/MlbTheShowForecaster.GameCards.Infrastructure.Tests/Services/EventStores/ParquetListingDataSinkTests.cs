using com.brettnamba.MlbTheShowForecaster.Common.Application.FileSystems;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.EventStores;
using Moq;
using StackExchange.Redis;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Services.EventStores;

public class ParquetListingDataSinkTests
{
    [Fact]
    public async Task Write_SeasonAndCount_WritesToParquet()
    {
        /*
         * Arrange
         */
        var year = SeasonYear.Create(2025);
        const int countToPullFromRedis = 1000;
        var settings = new ParquetListingDataSink.Settings(1, 2);

        // The next orders read from Redis
        var streamEntries = new List<StreamEntry>()
        {
            CreateStreamEntry("1", Faker.FakeGuid1, new DateTime(2025, 4, 14, 1, 2, 3), 1, 0),
            CreateStreamEntry("2", Faker.FakeGuid1, new DateTime(2025, 4, 14, 1, 2, 3), 1, 1),
            CreateStreamEntry("3", Faker.FakeGuid2, new DateTime(2025, 4, 15, 1, 2, 3), 2, 0),
            CreateStreamEntry("4", Faker.FakeGuid2, new DateTime(2025, 4, 16, 1, 2, 3), 3, 0),
            CreateStreamEntry("5", Faker.FakeGuid2, new DateTime(2025, 4, 17, 1, 2, 3), 4, 0),
        };

        // Stubbed redis
        var stubRedis = new Mock<IDatabase>();
        stubRedis.Setup(x => x.StreamReadAsync(RedisListingEventStore.OrdersEventStoreKey(year), "0",
                countToPullFromRedis, CommandFlags.None))
            .ReturnsAsync(streamEntries.ToArray());
        var stubRedisConnection = new Mock<IConnectionMultiplexer>();
        stubRedisConnection.Setup(x => x.GetDatabase(-1, null))
            .Returns(stubRedis.Object);

        var mockFileSystem = new Mock<IFileSystem>();

        var stubCalendar = new Mock<ICalendar>();
        stubCalendar.Setup(x => x.Today())
            .Returns(new DateOnly(2025, 4, 17));

        var sink = new ParquetListingDataSink(stubRedisConnection.Object, mockFileSystem.Object, stubCalendar.Object,
            settings);

        /*
         * Act
         */
        await sink.Write(year, countToPullFromRedis);

        /*
         * Assert
         */
        // Twice for 2025-04-14 (two entries over two files)
        mockFileSystem.Verify(x => x.StoreFile(
            It.IsAny<Stream>(),
            It.Is<string>(y => y.StartsWith("parquet/year=2025/month=04/day=14/2025-04-14-")),
            true
        ), Times.Exactly(2));
        // Once for 2025-04-15 (one entry in one file)
        mockFileSystem.Verify(x => x.StoreFile(
            It.IsAny<Stream>(),
            It.Is<string>(y => y.StartsWith("parquet/year=2025/month=04/day=15/2025-04-15-")),
            true
        ), Times.Once);
        // None for 2025-04-16 since it is excluded by settings
        mockFileSystem.Verify(x => x.StoreFile(
            It.IsAny<Stream>(),
            It.Is<string>(y => y.StartsWith("parquet/year=2025/month=04/day=16/2025-04-16-")),
            true
        ), Times.Never);
        // None for 2025-04-17 since it is excluded by settings
        mockFileSystem.Verify(x => x.StoreFile(
            It.IsAny<Stream>(),
            It.Is<string>(y => y.StartsWith("parquet/year=2025/month=04/day=17/2025-04-17-")),
            true
        ), Times.Never);
    }

    private StreamEntry CreateStreamEntry(string id, Guid cardExternalId, DateTime date, int price, int sequenceNumber)
    {
        return new StreamEntry(id: new RedisValue(id), new List<NameValueEntry>()
        {
            new NameValueEntry("card_external_id", cardExternalId.ToString(RedisListingEventStore.GuidFormat)),
            new NameValueEntry("date", date.ToString(RedisListingEventStore.DateTimeFormat)),
            new NameValueEntry("price", price),
            new NameValueEntry("sequence_number", sequenceNumber)
        }.ToArray());
    }
}