using System.Text;
using com.brettnamba.MlbTheShowForecaster.Common.Application.FileSystems;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Repositories;
using Microsoft.Extensions.Logging;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;

/// <inheritdoc />
public sealed class ForecastReportPublisher : IForecastReportPublisher
{
    /// <summary>
    /// The <see cref="PlayerCardForecast"/> repository
    /// </summary>
    private readonly IForecastRepository _forecastRepository;

    /// <summary>
    /// Report generator for forecasts
    /// </summary>
    private readonly IForecastReportGenerator _forecastReportGenerator;

    /// <summary>
    /// File system
    /// </summary>
    private readonly IFileSystem _fileSystem;

    /// <summary>
    /// The output path for the report
    /// </summary>
    private readonly string _outputPath;

    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<ForecastReportPublisher> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="forecastRepository">The <see cref="PlayerCardForecast"/> repository</param>
    /// <param name="forecastReportGenerator">Report generator for forecasts</param>
    /// <param name="fileSystem">File system</param>
    /// <param name="outputPath">The output path for the report</param>
    /// <param name="logger">Logger</param>
    public ForecastReportPublisher(IForecastRepository forecastRepository,
        IForecastReportGenerator forecastReportGenerator, IFileSystem fileSystem, string outputPath,
        ILogger<ForecastReportPublisher> logger)
    {
        _forecastRepository = forecastRepository;
        _forecastReportGenerator = forecastReportGenerator;
        _fileSystem = fileSystem;
        _outputPath = outputPath;
        if (string.IsNullOrWhiteSpace(_outputPath))
        {
            throw new ArgumentException("No output path specified");
        }

        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Publish(SeasonYear year, DateOnly date)
    {
        var forecasts = await _forecastRepository.GetImpactedForecasts(date);

        var report = await _forecastReportGenerator.Generate(year, forecasts, date);

        await using var reportStream = new MemoryStream(Encoding.UTF8.GetBytes(report.Html));
        await _fileSystem.StoreFile(reportStream, _outputPath, true);
        _logger.LogInformation($"Report published at {_outputPath}");
    }
}