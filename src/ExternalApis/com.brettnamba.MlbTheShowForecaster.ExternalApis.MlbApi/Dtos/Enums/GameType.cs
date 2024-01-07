using System.Runtime.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Enums;

/// <summary>
/// Enum representing game types
/// </summary>
public enum GameType
{
    /// <summary>
    /// Regular season
    /// </summary>
    [EnumMember(Value = "R")] RegularSeason,

    /// <summary>
    /// Playoffs
    /// </summary>
    [EnumMember(Value = "P")] Playoffs
}