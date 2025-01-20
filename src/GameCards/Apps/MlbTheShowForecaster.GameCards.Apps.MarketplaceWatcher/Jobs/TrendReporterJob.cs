using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs.Io;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs.Io;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Reports.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs;

/// <summary>
/// Job that updates trend reports for all player cards
/// </summary>
public sealed class TrendReporterJob : BaseJob<SeasonJobInput, TrendReporterJobResult>, IDisposable
{
    /// <summary>
    /// Gets all player cards
    /// </summary>
    private readonly IPlayerCardRepository _playerCardRepository;

    /// <summary>
    /// Updates trend reports
    /// </summary>
    private readonly ITrendReporter _trendReporter;

    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<TrendReporterJob> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerCardRepository">Gets all player cards</param>
    /// <param name="trendReporter">Updates trend reports</param>
    /// <param name="logger">Logger</param>
    public TrendReporterJob(IPlayerCardRepository playerCardRepository, ITrendReporter trendReporter,
        ILogger<TrendReporterJob> logger)
    {
        _playerCardRepository = playerCardRepository;
        _trendReporter = trendReporter;
        _logger = logger;
    }

    /// <inheritdoc />
    public override async Task<TrendReporterJobResult> Execute(SeasonJobInput input,
        CancellationToken cancellationToken = default)
    {
        var playerCards = (await _playerCardRepository.GetAll(input.Year)).ToList();

        var updatedReports = 0;
        foreach (var playerCard in playerCards)
        {
            try
            {
                await _trendReporter.UpdateTrendReport(input.Year, playerCard.ExternalId, cancellationToken);
                updatedReports++;
            }
            catch (TrendReportFactoryMissingDataException e)
            {
                // Expected if a player has not seen any play time
                _logger.LogWarning(e,
                    $"Trend report missing data for: {playerCard.Name.Value} ({playerCard.ExternalId.AsStringDigits})");
            }
        }

        return new TrendReporterJobResult(TotalPlayerCards: playerCards.Count, UpdatedReports: updatedReports);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
    }
}