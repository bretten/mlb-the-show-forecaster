namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;

/// <summary>
/// Request for getting a player's roster entries
/// </summary>
/// <param name="PlayerMlbId">The MLB ID of the player</param>
public sealed record GetPlayerRosterEntriesRequest(int PlayerMlbId);