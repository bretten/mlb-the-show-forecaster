namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Players.EntityFramework;

/// <summary>
/// Constants for EF in the PlayerStatus subdomain
/// </summary>
public static class Constants
{
    /// <summary>
    /// The schema name
    /// </summary>
    public const string Schema = "players";

    /// <summary>
    /// Constants for the Players table
    /// </summary>
    public static class Players
    {
        /// <summary>
        /// Players table name
        /// </summary>
        public const string TableName = "players";

        /// <summary>
        /// Player primary key column name
        /// </summary>
        public const string Id = "id";

        /// <summary>
        /// Player MLB ID column name
        /// </summary>
        public const string MlbId = "mlb_id";

        /// <summary>
        /// Player first name column name
        /// </summary>
        public const string FirstName = "first_name";

        /// <summary>
        /// Player last name column name
        /// </summary>
        public const string LastName = "last_name";

        /// <summary>
        /// Player birthdate column name
        /// </summary>
        public const string Birthdate = "birthdate";

        /// <summary>
        /// Player position column name
        /// </summary>
        public const string Position = "position";

        /// <summary>
        /// Player MLB debut date column name
        /// </summary>
        public const string MlbDebutDate = "mlb_debut_date";

        /// <summary>
        /// The side the player bats on column name
        /// </summary>
        public const string BatSide = "bat_side";

        /// <summary>
        /// The throw arm column name
        /// </summary>
        public const string ThrowArm = "throw_arm";

        /// <summary>
        /// The team column name
        /// </summary>
        public const string Team = "team";

        /// <summary>
        /// Active status column name
        /// </summary>
        public const string Active = "active";
    }
}