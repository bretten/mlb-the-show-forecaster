namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi;

/// <summary>
/// Constants for the MLB API
/// </summary>
public static class Constants
{
    /// <summary>
    /// Base URL
    /// </summary>
    public const string BaseUrl = "https://statsapi.mlb.com/api";

    /// <summary>
    /// Parameter constants
    /// </summary>
    public static class Parameters
    {
        /// <summary>
        /// Parameter name for hitting
        /// </summary>
        public const string Hitting = "hitting";

        /// <summary>
        /// Parameter name for pitching
        /// </summary>
        public const string Pitching = "pitching";

        /// <summary>
        /// Parameter name for fielding
        /// </summary>
        public const string Fielding = "fielding";
    }
}