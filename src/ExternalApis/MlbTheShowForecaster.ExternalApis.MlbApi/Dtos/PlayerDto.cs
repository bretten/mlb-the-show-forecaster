using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;

/// <summary>
/// Represents a MLB player
/// </summary>
public readonly record struct PlayerDto
{
    /// <summary>
    /// The player's MLB ID
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; init; }

    /// <summary>
    /// The player's first name
    /// </summary>
    [JsonPropertyName("useName")]
    public string FirstName { get; init; }

    /// <summary>
    /// The player's last name
    /// </summary>
    [JsonPropertyName("useLastName")]
    public string LastName { get; init; }

    /// <summary>
    /// The player's birthdate
    /// </summary>
    [JsonPropertyName("birthDate")]
    public DateTime Birthdate { get; init; }

    /// <summary>
    /// The player's primary position
    /// </summary>
    [JsonPropertyName("primaryPosition")]
    public PositionDto Position { get; init; }

    /// <summary>
    /// The player's MLB debut date
    /// </summary>
    [JsonPropertyName("mlbDebutDate")]
    public DateTime MlbDebutDate { get; init; }

    /// <summary>
    /// The side the player bats on
    /// </summary>
    [JsonPropertyName("batSide")]
    public ArmSideDto BatSide { get; init; }

    /// <summary>
    /// The arm the player throws with
    /// </summary>
    [JsonPropertyName("pitchHand")]
    public ArmSideDto ThrowArm { get; init; }

    /// <summary>
    /// The player's current team
    /// </summary>
    [JsonPropertyName("currentTeam")]
    public CurrentTeamDto CurrentTeam { get; init; }

    /// <summary>
    /// True if the player is active, otherwise false
    /// </summary>
    [JsonPropertyName("active")]
    public bool Active { get; init; }
}