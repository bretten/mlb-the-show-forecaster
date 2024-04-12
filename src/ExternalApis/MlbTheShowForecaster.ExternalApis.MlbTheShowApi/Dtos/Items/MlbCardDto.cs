using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

/// <summary>
/// A specific type of <see cref="ItemDto"/> that represents an MLB player
/// </summary>
/// <param name="Uuid">The unique ID</param>
/// <param name="Type">The type of item</param>
/// <param name="ImageUrl">The URL of the image associated with the item</param>
/// <param name="Name">The name of the item</param>
/// <param name="Rarity">The rarity of the item</param>
/// <param name="IsSellable">True if the item is sellable on the marketplace, otherwise false</param>
/// <param name="Series">The series the item is a part of</param>
/// <param name="TeamShortName">The player's team name abbreviation</param>
/// <param name="DisplayPosition">The player's primary position</param>
/// <param name="Overall">The player's overall rating</param>
/// <param name="Stamina">Pitcher's stamina</param>
/// <param name="PitchingClutch">Pitcher's ability to pitch with runners in scoring position</param>
/// <param name="HitsPerBf">Pitcher's ability to prevent hits</param>
/// <param name="KPerBf">Pitcher's ability to cause a batter to swing and miss</param>
/// <param name="BbPerBf">Pitcher's ability to prevent walks</param>
/// <param name="HrPerBf">Pitcher's ability to prevent home runs</param>
/// <param name="PitchVelocity">The pitcher's velocity</param>
/// <param name="PitchControl">The pitcher's control</param>
/// <param name="PitchMovement">The pitcher's ability to throw breaking pitches</param>
/// <param name="ContactLeft">The batter's ability to make contact with left handed pitchers</param>
/// <param name="ContactRight">The batter's ability to make contact with right handed pitchers</param>
/// <param name="PowerLeft">The batter's power against left handed pitchers</param>
/// <param name="PowerRight">The batter's power against right handed pitchers</param>
/// <param name="PlateVision">The batter's ability to see the ball and prevent strikeouts</param>
/// <param name="PlateDiscipline">The batter's ability to check a swing</param>
/// <param name="BattingClutch">The batter's ability to hit with runners in scoring position</param>
/// <param name="BuntingAbility">The batter's bunting ability</param>
/// <param name="DragBuntingAbility">The batter's drag bunting ability</param>
/// <param name="HittingDurability">The ability of a player to prevent injury when batting</param>
/// <param name="FieldingDurability">The ability of a player to prevent injury when fielding</param>
/// <param name="FieldingAbility">The player's fielding ability</param>
/// <param name="ArmStrength">The player's ability to throw the ball with velocity and distance</param>
/// <param name="ArmAccuracy">The player's ability to throw the ball accurately</param>
/// <param name="ReactionTime">The ability of a fielder to react when a batter makes contact with the ball</param>
/// <param name="Blocking">The ability of a catcher to block wild pitches</param>
/// <param name="Speed">The speed of the player</param>
/// <param name="BaseRunningAbility">How well the player can run around the bases</param>
/// <param name="BaseRunningAggression">How likely it is the player can steal a base</param>
public sealed record MlbCardDto(
    ObfuscatedIdDto Uuid,
    string Type,
    string ImageUrl,
    string Name,
    string Rarity,
    bool IsSellable,
    [property: JsonPropertyName("series")]
    string Series,
    [property: JsonPropertyName("team_short_name")]
    string TeamShortName,
    [property: JsonPropertyName("display_position")]
    string DisplayPosition,
    [property: JsonPropertyName("ovr")]
    int Overall,
    [property: JsonPropertyName("stamina")]
    int Stamina,
    [property: JsonPropertyName("pitching_clutch")]
    int PitchingClutch,
    [property: JsonPropertyName("hits_per_bf")]
    int HitsPerBf,
    [property: JsonPropertyName("k_per_bf")]
    int KPerBf,
    [property: JsonPropertyName("bb_per_bf")]
    int BbPerBf,
    [property: JsonPropertyName("hr_per_bf")]
    int HrPerBf,
    [property: JsonPropertyName("pitch_velocity")]
    int PitchVelocity,
    [property: JsonPropertyName("pitch_control")]
    int PitchControl,
    [property: JsonPropertyName("pitch_movement")]
    int PitchMovement,
    [property: JsonPropertyName("contact_left")]
    int ContactLeft,
    [property: JsonPropertyName("contact_right")]
    int ContactRight,
    [property: JsonPropertyName("power_left")]
    int PowerLeft,
    [property: JsonPropertyName("power_right")]
    int PowerRight,
    [property: JsonPropertyName("plate_vision")]
    int PlateVision,
    [property: JsonPropertyName("plate_discipline")]
    int PlateDiscipline,
    [property: JsonPropertyName("batting_clutch")]
    int BattingClutch,
    [property: JsonPropertyName("bunting_ability")]
    int BuntingAbility,
    [property: JsonPropertyName("drag_bunting_ability")]
    int DragBuntingAbility,
    [property: JsonPropertyName("hitting_durability")]
    int HittingDurability,
    [property: JsonPropertyName("fielding_durability")]
    int FieldingDurability,
    [property: JsonPropertyName("fielding_ability")]
    int FieldingAbility,
    [property: JsonPropertyName("arm_strength")]
    int ArmStrength,
    [property: JsonPropertyName("arm_accuracy")]
    int ArmAccuracy,
    [property: JsonPropertyName("reaction_time")]
    int ReactionTime,
    [property: JsonPropertyName("blocking")]
    int Blocking,
    [property: JsonPropertyName("speed")]
    int Speed,
    [property: JsonPropertyName("baserunning_ability")]
    int BaseRunningAbility,
    [property: JsonPropertyName("baserunning_aggression")]
    int BaseRunningAggression
) : ItemDto(Uuid, Type, ImageUrl, Name, Rarity, IsSellable);