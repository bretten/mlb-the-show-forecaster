using System.ComponentModel;
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
            ITrendReporter.SortField.Ovr => nameof(TrendReport.OverallRating),
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
        BsonSerializer.TryRegisterSerializer(typeof(TrendReport), new TrendReportSerializer());
        BsonSerializer.TryRegisterSerializer(typeof(TrendMetricsByDate), new TrendMetricsByDateSerializer());
        BsonSerializer.TryRegisterSerializer(typeof(TrendImpact), new TrendImpactSerializer());
    }

    /// <summary>
    /// Configuration for MongoDB
    /// </summary>
    public sealed record MongoDbTrendReporterConfig(string Database, string Collection);

    /// <summary>
    /// BSON serializer for <see cref="TrendReport"/>
    /// </summary>
    private sealed class TrendReportSerializer : SerializerBase<TrendReport>
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

            var r = context.Reader;
            r.ReadStartDocument();
            while (r.ReadBsonType() != BsonType.EndOfDocument)
            {
                var fieldName = r.ReadName();
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
                Impacts: impacts!
            );
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
                        Action(ref battingScore, type);
                        break;
                    case nameof(TrendMetricsByDate.SignificantBattingParticipation):
                        significantBattingParticipation = r.ReadBoolean();
                        break;
                    case nameof(TrendMetricsByDate.PitchingScore):
                        Action(ref pitchingScore, type);
                        break;
                    case nameof(TrendMetricsByDate.SignificantPitchingParticipation):
                        significantPitchingParticipation = r.ReadBoolean();
                        break;
                    case nameof(TrendMetricsByDate.FieldingScore):
                        Action(ref fieldingScore, type);
                        break;
                    case nameof(TrendMetricsByDate.SignificantFieldingParticipation):
                        significantFieldingParticipation = r.ReadBoolean();
                        break;
                    case nameof(TrendMetricsByDate.BattingAverage):
                        Action(ref battingAverage, type);
                        break;
                    case nameof(TrendMetricsByDate.OnBasePercentage):
                        Action(ref onBasePercentage, type);
                        break;
                    case nameof(TrendMetricsByDate.Slugging):
                        Action(ref slugging, type);
                        break;
                    case nameof(TrendMetricsByDate.EarnedRunAverage):
                        Action(ref earnedRunAverage, type);
                        break;
                    case nameof(TrendMetricsByDate.OpponentsBattingAverage):
                        Action(ref opponentsBattingAverage, type);
                        break;
                    case nameof(TrendMetricsByDate.StrikeoutsPer9):
                        Action(ref strikeoutsPer9, type);
                        break;
                    case nameof(TrendMetricsByDate.BaseOnBallsPer9):
                        Action(ref baseOnBallsPer9, type);
                        break;
                    case nameof(TrendMetricsByDate.HomeRunsPer9):
                        Action(ref homeRunsPer9, type);
                        break;
                    case nameof(TrendMetricsByDate.FieldingPercentage):
                        Action(ref fieldingPercentage, type);
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
                FieldingPercentage: fieldingPercentage
            );

            void Action(ref decimal? value, BsonType type)
            {
                if (type != BsonType.Null)
                    value = new BsonDecimal128(r.ReadDecimal128()).AsDecimal;
                else
                    r.ReadNull();
            }
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
                }
            }

            r.ReadEndDocument();

            return new TrendImpact(Start: DateOnly.Parse(start!), End: DateOnly.Parse(end!), Description: desc!);
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

            w.WriteEndDocument();
        }
    }
}