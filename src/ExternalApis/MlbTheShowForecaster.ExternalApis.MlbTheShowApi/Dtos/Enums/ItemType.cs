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
    [EnumMember(Value = Constants.ItemTypes.MlbCard)]
    MlbCard,

    /// <summary>
    /// A stadium
    /// </summary>
    [EnumMember(Value = Constants.ItemTypes.Stadium)]
    Stadium,

    /// <summary>
    /// Baseball equipment
    /// </summary>
    [EnumMember(Value = Constants.ItemTypes.Equipment)]
    Equipment,

    /// <summary>
    /// Sponsorship
    /// </summary>
    [EnumMember(Value = Constants.ItemTypes.Sponsorship)]
    Sponsorship,

    /// <summary>
    /// Unlockables like avatars
    /// </summary>
    [EnumMember(Value = Constants.ItemTypes.Unlockable)]
    Unlockable
}