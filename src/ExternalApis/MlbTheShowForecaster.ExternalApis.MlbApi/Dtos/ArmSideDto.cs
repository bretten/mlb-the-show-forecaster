using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;

/// <summary>
/// The arm (left, right or switch) that is used for throwing and hitting
/// </summary>
/// <param name="Code">A code representing the arm side (R, L, S)</param>
/// <param name="Description">A description of the arm (Right, Left, Switch)</param>
public readonly record struct ArmSideDto(
    [property: JsonPropertyName("code")]
    string Code,
    [property: JsonPropertyName("description")]
    string Description
);