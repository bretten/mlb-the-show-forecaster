namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;

public sealed record GetPlayerSeasonStatsByGameRequest(int PlayerMlbId, int Season);