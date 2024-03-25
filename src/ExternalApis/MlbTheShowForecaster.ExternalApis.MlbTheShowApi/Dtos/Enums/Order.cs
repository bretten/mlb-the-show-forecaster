using System.Runtime.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;

/// <summary>
/// Ascending or descending
/// </summary>
public enum Order
{
    /// <summary>
    /// Ascending
    /// </summary>
    [EnumMember(Value = "asc")] Ascending,

    /// <summary>
    /// Descending
    /// </summary>
    [EnumMember(Value = "desc")] Descending
}