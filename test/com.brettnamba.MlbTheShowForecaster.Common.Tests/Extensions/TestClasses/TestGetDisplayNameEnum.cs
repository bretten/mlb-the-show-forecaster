using System.ComponentModel.DataAnnotations;

namespace com.brettnamba.MlbTheShowForecaster.Common.Tests.Extensions.TestClasses;

/// <summary>
/// Test enum
/// </summary>
public enum TestGetDisplayNameEnum
{
    EnumWithoutDisplayAttribute,

    [Display] EnumWithDisplayAttribute,

    [Display(Name = "Has DisplayAttribute and name")]
    EnumWithDisplayAttributeAndName
}