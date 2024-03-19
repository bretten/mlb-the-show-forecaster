using System.Runtime.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;

/// <summary>
/// The different Item types available in the API
/// </summary>
public enum ItemType
{
    /// <summary>
    /// A MLB player card
    /// </summary>
    [EnumMember(Value = "mlb_card")] MlbCard,

    /// <summary>
    /// A stadium
    /// </summary>
    [EnumMember(Value = "stadium")] Stadium,

    /// <summary>
    /// Baseball equipment
    /// </summary>
    [EnumMember(Value = "equipment")] Equipment,

    /// <summary>
    /// Sponsorship
    /// </summary>
    [EnumMember(Value = "sponsorship")] Sponsorship,

    /// <summary>
    /// Unlockables like avatars
    /// </summary>
    [EnumMember(Value = "unlockable")] Unlockable
}