namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;

/// <summary>
/// Options for <see cref="IForecastReportGenerator"/>
/// </summary>
public sealed class ForecastReportOptions
{
    /// <summary>
    /// Templates that the report uses
    /// </summary>
    public Dictionary<string, string> Templates { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="templates">Templates that the report uses</param>
    public ForecastReportOptions(Dictionary<string, string> templates)
    {
        Templates = templates;
    }
}