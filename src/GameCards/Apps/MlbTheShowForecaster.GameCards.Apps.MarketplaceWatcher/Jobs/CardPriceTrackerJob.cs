using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs.Io;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs;

/// <summary>
/// Job that gets current and historical card prices
/// </summary>
public sealed class CardPriceTrackerJob : BaseJob<SeasonJobInput, CardPriceTrackerJobResult>, IDisposable
{
    /// <summary>
    /// Updates the current and historical card prices
    /// </summary>
    private readonly ICardPriceTracker _cardPriceTracker;

    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<CardPriceTrackerJob> _logger;

    /// <summary>
    /// Service name
    /// </summary>
    private const string S = nameof(ICardPriceTracker);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="cardPriceTracker">Updates the current and historical card prices</param>
    /// <param name="logger">Logger</param>
    public CardPriceTrackerJob(ICardPriceTracker cardPriceTracker, ILogger<CardPriceTrackerJob> logger)
    {
        _cardPriceTracker = cardPriceTracker;
        _logger = logger;
    }

    /// <inheritdoc />
    public override async Task<CardPriceTrackerJobResult> Execute(SeasonJobInput input,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"{S} - {input.Year.Value}");
        var result = await _cardPriceTracker.TrackCardPrices(input.Year, cancellationToken);
        _logger.LogInformation($"{S} - Total cards = {result.TotalCards}");
        _logger.LogInformation($"{S} - Total new listings = {result.TotalNewListings}");
        _logger.LogInformation($"{S} - Total updated listings = {result.TotalUpdatedListings}");
        _logger.LogInformation($"{S} - Total unchanged listings = {result.TotalUnchangedListings}");
        return new CardPriceTrackerJobResult(TotalCards: result.TotalCards, TotalNewListings: result.TotalNewListings,
            TotalUpdatedListings: result.TotalUpdatedListings);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        _cardPriceTracker.Dispose();
    }
}