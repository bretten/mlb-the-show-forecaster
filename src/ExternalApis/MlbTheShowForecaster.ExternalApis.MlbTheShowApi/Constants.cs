namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi;

/// <summary>
/// Constants for the MLB The Show API
/// </summary>
public static class Constants
{
    /// <summary>
    /// Base URL for MLB the Show 2024
    /// </summary>
    public const string BaseUrl2024 = "https://mlb24.theshow.com";

    /// <summary>
    /// Base URL for MLB the Show 2023
    /// </summary>
    public const string BaseUrl2023 = "https://mlb23.theshow.com";

    /// <summary>
    /// Base URL for MLB the Show 2022
    /// </summary>
    public const string BaseUrl2022 = "https://mlb22.theshow.com";

    /// <summary>
    /// Base URL for MLB the Show 2021
    /// </summary>
    public const string BaseUrl2021 = "https://mlb21.theshow.com";

    /// <summary>
    /// Item types
    /// </summary>
    public static class ItemTypes
    {
        /// <summary>
        /// A MLB player card
        /// </summary>
        public const string MlbCard = "mlb_card";

        /// <summary>
        /// A stadium
        /// </summary>
        public const string Stadium = "stadium";

        /// <summary>
        /// Baseball equipment
        /// </summary>
        public const string Equipment = "equipment";

        /// <summary>
        /// Sponsorship
        /// </summary>
        public const string Sponsorship = "sponsorship";

        /// <summary>
        /// Unlockables like avatars
        /// </summary>
        public const string Unlockable = "unlockable";
    }

    /// <summary>
    /// Card series
    /// </summary>
    public static class Series
    {
        /// <summary>
        /// Live series
        /// </summary>
        public const string Live = "Live";
    }
}