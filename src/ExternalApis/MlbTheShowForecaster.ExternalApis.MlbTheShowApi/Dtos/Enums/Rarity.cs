using System.Runtime.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;

/// <summary>
/// The rarity of an Item
/// </summary>
public enum Rarity
{
    /// <summary>
    /// Diamond, the most rare
    /// </summary>
    [EnumMember(Value = "diamond")] Diamond,

    /// <summary>
    /// Gold
    /// </summary>
    [EnumMember(Value = "gold")] Gold,

    /// <summary>
    /// Silver
    /// </summary>
    [EnumMember(Value = "silver")] Silver,

    /// <summary>
    /// Bronze
    /// </summary>
    [EnumMember(Value = "bronze")] Bronze,

    /// <summary>
    /// Common, the least rare
    /// </summary>
    [EnumMember(Value = "common")] Common
}