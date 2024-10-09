using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;

/// <summary>
/// Defines a factory that creates an up-to-date <see cref="TrendReport"/>
/// </summary>
public interface ITrendReportFactory
{
    /// <summary>
    /// Creates a <see cref="TrendReport"/> for the specified season and card external ID
    /// </summary>
    /// <param name="year">The season</param>
    /// <param name="cardExternalId">The card external ID</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns><see cref="TrendReport"/></returns>
    Task<TrendReport> GetReport(SeasonYear year, CardExternalId cardExternalId, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a <see cref="TrendReport"/> for the specified season and MLB ID
    /// </summary>
    /// <param name="year">The season</param>
    /// <param name="mlbId">MLB ID</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns><see cref="TrendReport"/></returns>
    Task<TrendReport> GetReport(SeasonYear year, MlbId mlbId, CancellationToken cancellationToken);
}