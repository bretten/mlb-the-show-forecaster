using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore;

/// <summary>
/// Constants for EF in the GameCards subdomain
/// </summary>
public static class Constants
{
    /// <summary>
    /// The schema name
    /// </summary>
    public const string Schema = "game_cards";

    /// <summary>
    /// Table and column names for <see cref="Listing"/>
    /// </summary>
    public static class Listings
    {
        /// <summary>
        /// Table name
        /// </summary>
        public const string TableName = "listings";

        /// <summary>
        /// Primary key column name
        /// </summary>
        public const string Id = "id";

        /// <summary>
        /// The external ID of the card that this listing is for
        /// </summary>
        public const string CardExternalId = "card_external_id";

        /// <summary>
        /// The current, best buy price
        /// </summary>
        public const string BuyPrice = "buy_price";

        /// <summary>
        /// The current, best sell price
        /// </summary>
        public const string SellPrice = "sell_price";

        /// <summary>
        /// Key names for <see cref="Listing"/>
        /// </summary>
        public static class Keys
        {
            /// <summary>
            /// Primary key
            /// </summary>
            public const string PrimaryKey = $"{TableName}_pkey";
        }

        /// <summary>
        /// Index names for <see cref="Listing"/>
        /// </summary>
        public static class Indexes
        {
            /// <summary>
            /// For querying by the card's external ID
            /// </summary>
            public const string ExternalId = $"{TableName}_{CardExternalId}_idx";
        }
    }

    /// <summary>
    /// Table and column names for <see cref="ListingHistoricalPrice"/>
    /// </summary>
    public static class ListingHistoricalPrices
    {
        /// <summary>
        /// Table name
        /// </summary>
        public const string TableName = "listing_historical_prices";

        /// <summary>
        /// Foreign key column name that references <see cref="Listing"/>
        /// </summary>
        public const string ListingId = "listing_id";

        /// <summary>
        /// The date of the card's listing prices
        /// </summary>
        public const string Date = "date";

        /// <summary>
        /// The best buy price for the day
        /// </summary>
        public const string BuyPrice = "buy_price";

        /// <summary>
        /// The best sell price for the day
        /// </summary>
        public const string SellPrice = "sell_price";

        /// <summary>
        /// Key names for <see cref="ListingHistoricalPrice"/>
        /// </summary>
        public static class Keys
        {
            /// <summary>
            /// Primary key
            /// </summary>
            public const string PrimaryKey = $"{TableName}_pkey";

            /// <summary>
            /// <see cref="ListingHistoricalPrice"/> references <see cref="Listing"/>
            /// </summary>
            public const string ListingsForeignKeyConstraint = $"{TableName}_{Listings.TableName}_{Listings.Id}_fkey";
        }
    }
}