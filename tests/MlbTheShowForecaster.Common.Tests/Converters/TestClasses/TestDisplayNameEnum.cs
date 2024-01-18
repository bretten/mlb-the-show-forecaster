using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using com.brettnamba.MlbTheShowForecaster.Common.Converters;

namespace com.brettnamba.MlbTheShowForecaster.Common.Tests.Converters.TestClasses;

/// <summary>
/// Test enum
/// </summary>
[TypeConverter(typeof(EnumDisplayNameConverter))]
public enum TestDisplayNameEnum
{
    [Display(Name = "Member A")] MemberA = 0,

    [Display(Name = "Member B")] MemberB = 1,

    [Display(Name = "Member C")] MemberC = 2,

    MemberD = 3,

    [Display(Name = "")] MemberE = 4,

    [Display(Name = " ")] MemberF = 5,

    [Display(Name = null)] MemberG = 6
}