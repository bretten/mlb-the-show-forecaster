﻿using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Pagination;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Extensions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Polly;
using Polly.Retry;

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
    /// Retry policy for concurrency situations
    ///
    /// A TrendReport can be inserted from multiple sources
    /// When trying to insert two or more of the same TrendReport at the same time, add a retry for duplicate key errors
    /// When retried, it will be treated as an idempotent action and will just be an additive update
    /// </summary>
    private static readonly ResiliencePipeline RetryPipeline = new ResiliencePipelineBuilder()
        .AddRetry(new RetryStrategyOptions
        {
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
            MaxRetryAttempts = 4,
            Delay = TimeSpan.FromSeconds(5)
        })
        .AddTimeout(TimeSpan.FromMinutes(1))
        .Build();

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
        await RetryPipeline.ExecuteAsync(async _ => { await Upsert(trendReport, cancellationToken); },
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateTrendReport(SeasonYear year, MlbId mlbId, CancellationToken cancellationToken)
    {
        // Create the updated trend report
        var trendReport = await _trendReportFactory.GetReport(year, mlbId, cancellationToken);

        // Save the report
        await RetryPipeline.ExecuteAsync(async _ => { await Upsert(trendReport, cancellationToken); },
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PaginationResult<TrendReport>> GetTrendReports(SeasonYear year, int page, int pageSize,
        string? sortField, ITrendReporter.SortOrder? sortOrder, ITrendReporter.CardFilter? cardFilter,
        CancellationToken cancellationToken)
    {
        var collection = await GetCollection();
        var filter = Builders<TrendReport>.Filter.Eq(nameof(TrendReport.Year), year.Value);
        if (cardFilter != null)
        {
            filter = Builders<TrendReport>.Filter.And(filter, FilterCards(cardFilter));
        }

        var sf = sortField?.ToLower();
        var sortFieldDef = sf switch
        {
            _ when nameof(TrendReport.OverallRating).ToLower() == sf => nameof(TrendReport.OverallRating),
            _ when nameof(TrendReport.PrimaryPosition).ToLower() == sf => nameof(TrendReport.PrimaryPosition),
            _ when nameof(TrendReport.Orders1H).ToLower() == sf => nameof(TrendReport.Orders1H),
            _ when nameof(TrendReport.Orders24H).ToLower() == sf => nameof(TrendReport.Orders24H),
            _ when nameof(TrendReport.BuyPrice).ToLower() == sf => nameof(TrendReport.BuyPrice),
            _ when nameof(TrendReport.BuyPriceChange24H).ToLower() == sf => nameof(TrendReport.BuyPriceChange24H),
            _ when nameof(TrendReport.SellPrice).ToLower() == sf => nameof(TrendReport.SellPrice),
            _ when nameof(TrendReport.SellPriceChange24H).ToLower() == sf => nameof(TrendReport.SellPriceChange24H),
            _ when nameof(TrendReport.Score).ToLower() == sf => nameof(TrendReport.Score),
            _ when nameof(TrendReport.ScoreChange2W).ToLower() == sf => nameof(TrendReport.ScoreChange2W),
            _ => nameof(TrendReport.CardName)
        };

        var sortOrderDef = sortOrder switch
        {
            ITrendReporter.SortOrder.Desc => Builders<TrendReport>.Sort.Descending(sortFieldDef),
            _ => Builders<TrendReport>.Sort.Ascending(sortFieldDef)
        };

        var totalItems = await collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var items = collection.Find(filter)
            .Sort(sortOrderDef)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToList(cancellationToken: cancellationToken);

        return PaginationResult<TrendReport>.Create(page: page, pageSize: pageSize, totalItems: totalItems, items);
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

        var collection = db.GetCollection<TrendReport>(_config.Collection);

        // Indexes
        var index = new CreateIndexModel<TrendReport>(Builders<TrendReport>.IndexKeys
            .Ascending(x => x.Year)
            .Ascending(x => x.CardExternalId), new CreateIndexOptions() { Unique = true });
        await collection.Indexes.CreateOneAsync(index);

        return collection;
    }

    /// <summary>
    /// Registers serializers for MongoDB
    /// </summary>
    private static void RegisterSerializers()
    {
        BsonSerializer.TryRegisterSerializer(typeof(TrendReport), new TrendReportSerializer());
        BsonSerializer.TryRegisterSerializer(typeof(TrendMetricsByDate), new TrendMetricsByDateSerializer());
        BsonSerializer.TryRegisterSerializer(typeof(TrendImpact), new TrendImpactSerializer());
    }

    /// <summary>
    /// Builds a filter for card types
    /// </summary>
    /// <param name="filter">The type of filter to build</param>
    /// <returns><see cref="FilterDefinition{TDocument}"/></returns>
    private static FilterDefinition<TrendReport>? FilterCards(ITrendReporter.CardFilter? filter)
    {
        return filter switch
        {
            ITrendReporter.CardFilter.Boosted => Builders<TrendReport>.Filter.Eq(nameof(TrendReport.IsBoosted), true),
            ITrendReporter.CardFilter.Diamond =>
                Builders<TrendReport>.Filter.Gte(nameof(TrendReport.OverallRating), 85),
            ITrendReporter.CardFilter.Gold => Builders<TrendReport>.Filter.And(
                Builders<TrendReport>.Filter.Lt(nameof(TrendReport.OverallRating), 85),
                Builders<TrendReport>.Filter.Gte(nameof(TrendReport.OverallRating), 80)),
            ITrendReporter.CardFilter.Silver => Builders<TrendReport>.Filter.And(
                Builders<TrendReport>.Filter.Lt(nameof(TrendReport.OverallRating), 80),
                Builders<TrendReport>.Filter.Gte(nameof(TrendReport.OverallRating), 75)),
            ITrendReporter.CardFilter.Bronze => Builders<TrendReport>.Filter.And(
                Builders<TrendReport>.Filter.Lt(nameof(TrendReport.OverallRating), 75),
                Builders<TrendReport>.Filter.Gte(nameof(TrendReport.OverallRating), 65)),
            ITrendReporter.CardFilter.Common => Builders<TrendReport>.Filter.Lt(nameof(TrendReport.OverallRating), 65),
            _ => null
        };
    }

    /// <summary>
    /// Configuration for MongoDB
    /// </summary>
    public sealed record MongoDbTrendReporterConfig(string Database, string Collection);

    /// <summary>
    /// BSON serializer for <see cref="TrendReport"/>
    /// </summary>
    private sealed class TrendReportSerializer : SerializerBase<TrendReport>, IBsonDocumentSerializer
    {
        /// <inheritdoc />
        public override TrendReport Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var trendMetricsByDateSerializer = BsonSerializer.LookupSerializer<TrendMetricsByDate>();
            var trendImpactSerializer = BsonSerializer.LookupSerializer<TrendImpact>();

            // The field names may not be in a specific order, so have placeholders for the values
            int? year = null;
            string? cardExternalId = null;
            int? mlbId = null;
            string? cardName = null;
            string? position = null;
            int? overallRating = null;
            List<TrendMetricsByDate>? metrics = null;
            List<TrendImpact>? impacts = null;
            bool isBoosted = false;
            int? orders1H = null;
            int? orders24H = null;
            int? buyPrice = null;
            decimal? buyPriceChange24H = null;
            int? sellPrice = null;
            decimal? sellPriceChange24H = null;
            decimal? score = null;
            decimal? scoreChange2W = null;
            int? demand = null;

            var r = context.Reader;
            r.ReadStartDocument();
            while (r.ReadBsonType() != BsonType.EndOfDocument)
            {
                var fieldName = r.ReadName();
                var type = r.CurrentBsonType;

                switch (fieldName)
                {
                    case nameof(TrendReport.Year):
                        year = r.ReadInt32();
                        break;
                    case nameof(TrendReport.CardExternalId):
                        cardExternalId = r.ReadString();
                        break;
                    case nameof(TrendReport.MlbId):
                        mlbId = r.ReadInt32();
                        break;
                    case nameof(TrendReport.CardName):
                        cardName = r.ReadString();
                        break;
                    case nameof(TrendReport.PrimaryPosition):
                        position = r.ReadString();
                        break;
                    case nameof(TrendReport.OverallRating):
                        overallRating = r.ReadInt32();
                        break;
                    case nameof(TrendReport.MetricsByDate):
                        metrics = new List<TrendMetricsByDate>();
                        r.ReadStartArray();
                        while (r.ReadBsonType() != BsonType.EndOfDocument)
                        {
                            metrics.Add(trendMetricsByDateSerializer.Deserialize(context, args));
                        }

                        r.ReadEndArray();
                        break;
                    case nameof(TrendReport.Impacts):
                        impacts = new List<TrendImpact>();
                        r.ReadStartArray();
                        while (r.ReadBsonType() != BsonType.EndOfDocument)
                        {
                            impacts.Add(trendImpactSerializer.Deserialize(context, args));
                        }

                        r.ReadEndArray();
                        break;
                    case nameof(TrendReport.IsBoosted):
                        isBoosted = r.ReadBoolean();
                        break;
                    case nameof(TrendReport.Orders1H):
                        orders1H = r.ReadInt32();
                        break;
                    case nameof(TrendReport.Orders24H):
                        orders24H = r.ReadInt32();
                        break;
                    case nameof(TrendReport.BuyPrice):
                        buyPrice = r.ReadInt32();
                        break;
                    case nameof(TrendReport.BuyPriceChange24H):
                        ReadDecimal(r, ref buyPriceChange24H, type);
                        break;
                    case nameof(TrendReport.SellPrice):
                        sellPrice = r.ReadInt32();
                        break;
                    case nameof(TrendReport.SellPriceChange24H):
                        ReadDecimal(r, ref sellPriceChange24H, type);
                        break;
                    case nameof(TrendReport.Score):
                        ReadDecimal(r, ref score, type);
                        break;
                    case nameof(TrendReport.ScoreChange2W):
                        ReadDecimal(r, ref scoreChange2W, type);
                        break;
                    case nameof(TrendReport.Demand):
                        demand = r.ReadInt32();
                        break;
                    default:
                        r.SkipValue(); // This will be the MongoDB ID
                        break;
                }
            }

            r.ReadEndDocument();

            return new TrendReport(
                Year: SeasonYear.Create((ushort)year!),
                CardExternalId: CardExternalId.Create(new Guid(cardExternalId!)),
                MlbId: MlbId.Create(mlbId!.Value),
                CardName: CardName.Create(cardName!),
                PrimaryPosition: (Position)TypeDescriptor.GetConverter(typeof(Position)).ConvertFrom(position!)!,
                OverallRating: OverallRating.Create(overallRating!.Value),
                MetricsByDate: metrics!,
                Impacts: impacts!,
                IsBoosted: isBoosted,
                Orders1H: orders1H!.Value,
                Orders24H: orders24H!.Value,
                BuyPrice: buyPrice!.Value,
                BuyPriceChange24H: buyPriceChange24H!.Value,
                SellPrice: sellPrice!.Value,
                SellPriceChange24H: sellPriceChange24H!.Value,
                Score: score!.Value,
                ScoreChange2W: scoreChange2W!.Value,
                Demand: demand!.Value
            );
        }

        /// <summary>
        /// Maps members to fields
        ///
        /// Required when you want to refer to fields using LINQ:
        ///     var index = new CreateIndexModel(Builders.IndexKeys
        ///         .Ascending(x => x.Year))
        ///
        /// Otherwise, you need to use a string to reference the field name:
        ///     var index = new CreateIndexModel(Builders.IndexKeys
        ///         .Ascending(nameof(TrendReport.Year))
        /// </summary>
        public bool TryGetMemberSerializationInfo(string memberName,
            [UnscopedRef] out BsonSerializationInfo? serializationInfo)
        {
            switch (memberName)
            {
                case nameof(TrendReport.Year):
                    serializationInfo = new BsonSerializationInfo("Year", new Int32Serializer(), typeof(int));
                    return true;
                case nameof(TrendReport.CardExternalId):
                    serializationInfo =
                        new BsonSerializationInfo("CardExternalId", new StringSerializer(), typeof(string));
                    return true;
                default:
                    serializationInfo = null;
                    return false;
            }
        }

        /// <inheritdoc />
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TrendReport value)
        {
            var trendMetricsByDateSerializer = BsonSerializer.LookupSerializer<TrendMetricsByDate>();
            var trendImpactSerializer = BsonSerializer.LookupSerializer<TrendImpact>();

            var w = context.Writer;
            w.WriteStartDocument();

            w.WriteName(nameof(TrendReport.Year));
            w.WriteInt32(value.Year.Value);

            w.WriteName(nameof(TrendReport.CardExternalId));
            w.WriteString(value.CardExternalId.Value.ToString());

            w.WriteName(nameof(TrendReport.MlbId));
            w.WriteInt32(value.MlbId.Value);

            w.WriteName(nameof(TrendReport.CardName));
            w.WriteString(value.CardName.Value);

            w.WriteName(nameof(TrendReport.PrimaryPosition));
            w.WriteString(value.PrimaryPosition.GetDisplayName());

            w.WriteName(nameof(TrendReport.OverallRating));
            w.WriteInt32(value.OverallRating.Value);

            w.WriteName(nameof(TrendReport.MetricsByDate));
            w.WriteStartArray();
            foreach (var metric in value.MetricsByDate)
            {
                trendMetricsByDateSerializer.Serialize(context, args, metric);
            }

            w.WriteEndArray();

            w.WriteName(nameof(TrendReport.Impacts));
            w.WriteStartArray();
            foreach (var metric in value.Impacts)
            {
                trendImpactSerializer.Serialize(context, args, metric);
            }

            w.WriteEndArray();

            w.WriteName(nameof(TrendReport.IsBoosted));
            w.WriteBoolean(value.IsBoosted);

            w.WriteName(nameof(TrendReport.Orders1H));
            w.WriteInt32(value.Orders1H);

            w.WriteName(nameof(TrendReport.Orders24H));
            w.WriteInt32(value.Orders24H);

            w.WriteName(nameof(TrendReport.BuyPrice));
            w.WriteInt32(value.BuyPrice);

            w.WriteName(nameof(TrendReport.BuyPriceChange24H));
            w.WriteDecimal128(value.BuyPriceChange24H);

            w.WriteName(nameof(TrendReport.SellPrice));
            w.WriteInt32(value.SellPrice);

            w.WriteName(nameof(TrendReport.SellPriceChange24H));
            w.WriteDecimal128(value.SellPriceChange24H);

            w.WriteName(nameof(TrendReport.Score));
            w.WriteDecimal128(value.Score);

            w.WriteName(nameof(TrendReport.ScoreChange2W));
            w.WriteDecimal128(value.ScoreChange2W);

            w.WriteName(nameof(TrendReport.Demand));
            w.WriteInt32(value.Demand);

            w.WriteEndDocument();
        }
    }

    /// <summary>
    /// BSON serializer for <see cref="TrendMetricsByDate"/>
    /// </summary>
    private sealed class TrendMetricsByDateSerializer : SerializerBase<TrendMetricsByDate>
    {
        /// <inheritdoc />
        public override TrendMetricsByDate Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var r = context.Reader;

            // The field names may not be in a specific order, so have placeholders for the values
            string? date = null;
            int? buyPrice = null;
            int? sellPrice = null;
            decimal? battingScore = null;
            var significantBattingParticipation = false;
            decimal? pitchingScore = null;
            var significantPitchingParticipation = false;
            decimal? fieldingScore = null;
            var significantFieldingParticipation = false;
            decimal? battingAverage = null;
            decimal? onBasePercentage = null;
            decimal? slugging = null;
            decimal? earnedRunAverage = null;
            decimal? opponentsBattingAverage = null;
            decimal? strikeoutsPer9 = null;
            decimal? baseOnBallsPer9 = null;
            decimal? homeRunsPer9 = null;
            decimal? fieldingPercentage = null;
            int? demand = null;
            int? orderCount = null;

            r.ReadStartDocument();
            while (r.ReadBsonType() != BsonType.EndOfDocument)
            {
                var fieldName = r.ReadName();
                var type = r.CurrentBsonType;

                switch (fieldName)
                {
                    case nameof(TrendMetricsByDate.Date):
                        date = r.ReadString();
                        break;
                    case nameof(TrendMetricsByDate.BuyPrice):
                        buyPrice = r.ReadInt32();
                        break;
                    case nameof(TrendMetricsByDate.SellPrice):
                        sellPrice = r.ReadInt32();
                        break;
                    case nameof(TrendMetricsByDate.BattingScore):
                        ReadDecimal(r, ref battingScore, type);
                        break;
                    case nameof(TrendMetricsByDate.SignificantBattingParticipation):
                        significantBattingParticipation = r.ReadBoolean();
                        break;
                    case nameof(TrendMetricsByDate.PitchingScore):
                        ReadDecimal(r, ref pitchingScore, type);
                        break;
                    case nameof(TrendMetricsByDate.SignificantPitchingParticipation):
                        significantPitchingParticipation = r.ReadBoolean();
                        break;
                    case nameof(TrendMetricsByDate.FieldingScore):
                        ReadDecimal(r, ref fieldingScore, type);
                        break;
                    case nameof(TrendMetricsByDate.SignificantFieldingParticipation):
                        significantFieldingParticipation = r.ReadBoolean();
                        break;
                    case nameof(TrendMetricsByDate.BattingAverage):
                        ReadDecimal(r, ref battingAverage, type);
                        break;
                    case nameof(TrendMetricsByDate.OnBasePercentage):
                        ReadDecimal(r, ref onBasePercentage, type);
                        break;
                    case nameof(TrendMetricsByDate.Slugging):
                        ReadDecimal(r, ref slugging, type);
                        break;
                    case nameof(TrendMetricsByDate.EarnedRunAverage):
                        ReadDecimal(r, ref earnedRunAverage, type);
                        break;
                    case nameof(TrendMetricsByDate.OpponentsBattingAverage):
                        ReadDecimal(r, ref opponentsBattingAverage, type);
                        break;
                    case nameof(TrendMetricsByDate.StrikeoutsPer9):
                        ReadDecimal(r, ref strikeoutsPer9, type);
                        break;
                    case nameof(TrendMetricsByDate.BaseOnBallsPer9):
                        ReadDecimal(r, ref baseOnBallsPer9, type);
                        break;
                    case nameof(TrendMetricsByDate.HomeRunsPer9):
                        ReadDecimal(r, ref homeRunsPer9, type);
                        break;
                    case nameof(TrendMetricsByDate.FieldingPercentage):
                        ReadDecimal(r, ref fieldingPercentage, type);
                        break;
                    case nameof(TrendMetricsByDate.Demand):
                        demand = r.ReadInt32();
                        break;
                    case nameof(TrendMetricsByDate.OrderCount):
                        orderCount = r.ReadInt32();
                        break;
                }
            }

            r.ReadEndDocument();

            return new TrendMetricsByDate(Date: DateOnly.Parse(date!),
                BuyPrice: buyPrice!.Value,
                SellPrice: sellPrice!.Value,
                BattingScore: battingScore,
                SignificantBattingParticipation: significantBattingParticipation,
                PitchingScore: pitchingScore,
                SignificantPitchingParticipation: significantPitchingParticipation,
                FieldingScore: fieldingScore,
                SignificantFieldingParticipation: significantFieldingParticipation,
                BattingAverage: battingAverage,
                OnBasePercentage: onBasePercentage,
                Slugging: slugging,
                EarnedRunAverage: earnedRunAverage,
                OpponentsBattingAverage: opponentsBattingAverage,
                StrikeoutsPer9: strikeoutsPer9,
                BaseOnBallsPer9: baseOnBallsPer9,
                HomeRunsPer9: homeRunsPer9,
                FieldingPercentage: fieldingPercentage,
                Demand: demand ?? 0,
                OrderCount: orderCount ?? 0
            );
        }

        /// <inheritdoc />
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args,
            TrendMetricsByDate value)
        {
            var w = context.Writer;
            w.WriteStartDocument();

            w.WriteName(nameof(TrendMetricsByDate.Date));
            w.WriteString(value.Date.ToString("O"));

            w.WriteName(nameof(TrendMetricsByDate.BuyPrice));
            w.WriteInt32(value.BuyPrice);

            w.WriteName(nameof(TrendMetricsByDate.SellPrice));
            w.WriteInt32(value.SellPrice);

            w.WriteName(nameof(TrendMetricsByDate.BattingScore));
            if (value.BattingScore.HasValue)
            {
                w.WriteDecimal128(value.BattingScore.Value);
            }
            else
            {
                w.WriteNull();
            }

            w.WriteName(nameof(TrendMetricsByDate.SignificantBattingParticipation));
            w.WriteBoolean(value.SignificantBattingParticipation);

            w.WriteName(nameof(TrendMetricsByDate.PitchingScore));
            if (value.PitchingScore.HasValue)
            {
                w.WriteDecimal128(value.PitchingScore.Value);
            }
            else
            {
                w.WriteNull();
            }

            w.WriteName(nameof(TrendMetricsByDate.SignificantPitchingParticipation));
            w.WriteBoolean(value.SignificantPitchingParticipation);

            w.WriteName(nameof(TrendMetricsByDate.FieldingScore));
            if (value.FieldingScore.HasValue)
            {
                w.WriteDecimal128(value.FieldingScore.Value);
            }
            else
            {
                w.WriteNull();
            }

            w.WriteName(nameof(TrendMetricsByDate.SignificantFieldingParticipation));
            w.WriteBoolean(value.SignificantFieldingParticipation);

            w.WriteName(nameof(TrendMetricsByDate.BattingAverage));
            if (value.BattingAverage.HasValue)
            {
                w.WriteDecimal128(value.BattingAverage.Value);
            }
            else
            {
                w.WriteNull();
            }

            w.WriteName(nameof(TrendMetricsByDate.OnBasePercentage));
            if (value.OnBasePercentage.HasValue)
            {
                w.WriteDecimal128(value.OnBasePercentage.Value);
            }
            else
            {
                w.WriteNull();
            }

            w.WriteName(nameof(TrendMetricsByDate.Slugging));
            if (value.Slugging.HasValue)
            {
                w.WriteDecimal128(value.Slugging.Value);
            }
            else
            {
                w.WriteNull();
            }

            w.WriteName(nameof(TrendMetricsByDate.EarnedRunAverage));
            if (value.EarnedRunAverage.HasValue)
            {
                w.WriteDecimal128(value.EarnedRunAverage.Value);
            }
            else
            {
                w.WriteNull();
            }

            w.WriteName(nameof(TrendMetricsByDate.OpponentsBattingAverage));
            if (value.OpponentsBattingAverage.HasValue)
            {
                w.WriteDecimal128(value.OpponentsBattingAverage.Value);
            }
            else
            {
                w.WriteNull();
            }

            w.WriteName(nameof(TrendMetricsByDate.StrikeoutsPer9));
            if (value.StrikeoutsPer9.HasValue)
            {
                w.WriteDecimal128(value.StrikeoutsPer9.Value);
            }
            else
            {
                w.WriteNull();
            }

            w.WriteName(nameof(TrendMetricsByDate.BaseOnBallsPer9));
            if (value.BaseOnBallsPer9.HasValue)
            {
                w.WriteDecimal128(value.BaseOnBallsPer9.Value);
            }
            else
            {
                w.WriteNull();
            }

            w.WriteName(nameof(TrendMetricsByDate.HomeRunsPer9));
            if (value.HomeRunsPer9.HasValue)
            {
                w.WriteDecimal128(value.HomeRunsPer9.Value);
            }
            else
            {
                w.WriteNull();
            }

            w.WriteName(nameof(TrendMetricsByDate.FieldingPercentage));
            if (value.FieldingPercentage.HasValue)
            {
                w.WriteDecimal128(value.FieldingPercentage.Value);
            }
            else
            {
                w.WriteNull();
            }

            w.WriteName(nameof(TrendMetricsByDate.Demand));
            w.WriteInt32(value.Demand);

            w.WriteName(nameof(TrendMetricsByDate.OrderCount));
            w.WriteInt32(value.OrderCount);

            w.WriteEndDocument();
        }
    }

    /// <summary>
    /// BSON serializer for <see cref="TrendImpact"/>
    /// </summary>
    private sealed class TrendImpactSerializer : SerializerBase<TrendImpact>
    {
        /// <inheritdoc />
        public override TrendImpact Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var r = context.Reader;

            // The field names may not be in a specific order, so have placeholders for the values
            string? start = null;
            string? end = null;
            string? desc = null;
            int? demand = null;

            r.ReadStartDocument();
            while (r.ReadBsonType() != BsonType.EndOfDocument)
            {
                var fieldName = r.ReadName();

                switch (fieldName)
                {
                    case nameof(TrendImpact.Start):
                        start = r.ReadString();
                        break;
                    case nameof(TrendImpact.End):
                        end = r.ReadString();
                        break;
                    case nameof(TrendImpact.Description):
                        desc = r.ReadString();
                        break;
                    case nameof(TrendImpact.Demand):
                        demand = r.ReadInt32();
                        break;
                }
            }

            r.ReadEndDocument();

            return new TrendImpact(Start: DateOnly.Parse(start!), End: DateOnly.Parse(end!), Description: desc!,
                Demand: demand!.Value);
        }

        /// <inheritdoc />
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TrendImpact value)
        {
            var w = context.Writer;
            w.WriteStartDocument();

            w.WriteName(nameof(TrendImpact.Start));
            w.WriteString(value.Start.ToString("O"));

            w.WriteName(nameof(TrendImpact.End));
            w.WriteString(value.End.ToString("O"));

            w.WriteName(nameof(TrendImpact.Description));
            w.WriteString(value.Description);

            w.WriteName(nameof(TrendImpact.Demand));
            w.WriteInt32(value.Demand);

            w.WriteEndDocument();
        }
    }

    /// <summary>
    /// Reads a nullable decimal from <see cref="IBsonReader"/>
    /// </summary>
    /// <param name="r">The <see cref="IBsonReader"/></param>
    /// <param name="value">The decimal variable to assign to if the value exists</param>
    /// <param name="type">The <see cref="BsonType"/> of the value</param>
    private static void ReadDecimal(IBsonReader r, ref decimal? value, BsonType type)
    {
        if (type != BsonType.Null)
            value = new BsonDecimal128(r.ReadDecimal128()).AsDecimal;
        else
            r.ReadNull();
    }
}