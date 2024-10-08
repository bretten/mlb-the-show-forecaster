using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Reports;

/// <summary>
/// Reports on a player's stat trends and uses MongoDB to store and retrieve them
/// </summary>
public sealed class MongoDbTrendReporter : ITrendReporter
{
    /// <summary>
    /// Creates <see cref="TrendReport"/>s
    /// </summary>
    private readonly ITrendReportFactory _trendReportFactory;

    /// <summary>
    /// MongoDB client
    /// </summary>
    private readonly IMongoClient _mongoClient;

    /// <summary>
    /// Config for MongoDB
    /// </summary>
    private readonly MongoDbTrendReporterConfig _config;

    /// <summary>
    /// Forces replaces to be upserts
    /// </summary>
    private static readonly ReplaceOptions ReplaceOptions = new ReplaceOptions { IsUpsert = true };

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="trendReportFactory">Creates <see cref="TrendReport"/>s</param>
    /// <param name="mongoClient">MongoDB client</param>
    /// <param name="config">Config for MongoDB</param>
    public MongoDbTrendReporter(ITrendReportFactory trendReportFactory, IMongoClient mongoClient,
        MongoDbTrendReporterConfig config)
    {
        _trendReportFactory = trendReportFactory;
        _mongoClient = mongoClient;
        _config = config;
        RegisterSerializers();
    }

    /// <inheritdoc />
    public async Task UpdateTrendReport(SeasonYear year, CardExternalId cardExternalId,
        CancellationToken cancellationToken)
    {
        // Create the updated trend report
        var trendReport = await _trendReportFactory.GetReport(year, cardExternalId, cancellationToken);

        // Save the report
        await Upsert(trendReport, cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateTrendReport(SeasonYear year, MlbId mlbId, CancellationToken cancellationToken)
    {
        // Create the updated trend report
        var trendReport = await _trendReportFactory.GetReport(year, mlbId, cancellationToken);

        // Save the report
        await Upsert(trendReport, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TrendReport>> GetTrendReports(SeasonYear year, int page, int pageSize,
        ITrendReporter.SortField? sortField, ITrendReporter.SortOrder? sortOrder, CancellationToken cancellationToken)
    {
        var collection = await GetCollection();
        var filter = Builders<TrendReport>.Filter.Eq(nameof(TrendReport.Year), year.Value);

        var sortFieldDef = sortField switch
        {
            ITrendReporter.SortField.OverallRating => nameof(TrendReport.OverallRating),
            _ => nameof(TrendReport.CardName),
        };

        var sortOrderDef = sortOrder switch
        {
            ITrendReporter.SortOrder.Desc => Builders<TrendReport>.Sort.Descending(sortFieldDef),
            _ => Builders<TrendReport>.Sort.Ascending(sortFieldDef)
        };

        return collection.Find(filter)
            .Sort(sortOrderDef)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToList(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Inserts or updates the <see cref="TrendReport"/>
    /// </summary>
    /// <param name="trendReport"><see cref="TrendReport"/> to update</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    private async Task Upsert(TrendReport trendReport, CancellationToken cancellationToken)
    {
        var collection = await GetCollection();

        var builder = Builders<TrendReport>.Filter;
        var filter = builder.And(builder.Eq(nameof(TrendReport.Year), trendReport.Year.Value),
            builder.Eq(nameof(TrendReport.CardExternalId), trendReport.CardExternalId.Value.ToString()));

        var exists = await collection.Find(filter).CountDocumentsAsync(cancellationToken) > 0;
        if (!exists)
        {
            await collection.InsertOneAsync(trendReport, cancellationToken: cancellationToken);
        }
        else
        {
            await collection.ReplaceOneAsync(filter, trendReport, ReplaceOptions, cancellationToken);
        }
    }

    /// <summary>
    /// Gets the MongoDB collection for <see cref="TrendReport"/>s
    /// </summary>
    /// <returns>MongoDB collection for <see cref="TrendReport"/>s</returns>
    private async Task<IMongoCollection<TrendReport>> GetCollection()
    {
        var db = _mongoClient.GetDatabase(_config.Database);

        var exists = (await db.ListCollectionNamesAsync()).ToList().Contains(_config.Collection);
        if (!exists)
        {
            await db.CreateCollectionAsync(_config.Collection, new CreateCollectionOptions()
            {
                Collation = new Collation("en", strength: CollationStrength.Primary)
            });
        }

        return db.GetCollection<TrendReport>(_config.Collection);
    }

    /// <summary>
    /// Registers serializers for MongoDB
    /// </summary>
    private static void RegisterSerializers()
    {
        BsonSerializer.RegisterSerializer(typeof(TrendReport), new TrendReportSerializer());
    }

    /// <summary>
    /// Configuration for MongoDB
    /// </summary>
    public sealed record MongoDbTrendReporterConfig(string Database, string Collection);

    /// <summary>
    /// BSON serializer for <see cref="TrendReport"/>
    ///
    /// Delegates serialization to <see cref="TrendReportJsonConverter"/> by:
    /// - Serializing from <see cref="TrendReport"/> to JSON, then to BSON on MongoDB writes
    /// - Deserializing as BSON, converting to JSON, and then <see cref="TrendReport"/> on MongoDB reads
    /// </summary>
    private sealed class TrendReportSerializer : IBsonSerializer<TrendReport>, IBsonDocumentSerializer
    {
        /// <summary>
        /// Standard BSON serializer that will be used as the intermediary between BSON and JSON
        /// </summary>
        private static readonly IBsonSerializer Serializer = BsonSerializer.LookupSerializer(typeof(BsonDocument));

        /// <inheritdoc />
        public Type ValueType => typeof(TrendReport);

        /// <inheritdoc />
        public TrendReport Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            // Deserialize document to BSON and remove the _id as it is not on the target object nor is it needed
            var document = Serializer.Deserialize(context, args);
            var bsonDocument = document.ToBsonDocument();
            bsonDocument.Remove("_id");

            // BSON to JSON
            var result = BsonExtensionMethods.ToJson(bsonDocument)!;
            // JSON to target object
            return JsonSerializer.Deserialize<TrendReport>(result, new JsonSerializerOptions()
            {
                Converters = { new TrendReportJsonConverter() }
            })!;
        }

        /// <inheritdoc />
        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TrendReport value)
        {
            // Value to JSON
            var jsonDocument = JsonSerializer.Serialize(value, new JsonSerializerOptions()
            {
                Converters = { new TrendReportJsonConverter() }
            });
            // JSON to BSON
            var bsonDocument = BsonSerializer.Deserialize<BsonDocument>(jsonDocument);
            Serializer.Serialize(context, bsonDocument.AsBsonValue);
        }

        /// <inheritdoc />
        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize(context, args);
        }

        /// <inheritdoc />
        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            Serialize(context, args, (TrendReport)value);
        }

        /// <inheritdoc />
        public bool TryGetMemberSerializationInfo(string memberName,
            [UnscopedRef] out BsonSerializationInfo? serializationInfo)
        {
            serializationInfo = null;
            return false;
        }
    }
}