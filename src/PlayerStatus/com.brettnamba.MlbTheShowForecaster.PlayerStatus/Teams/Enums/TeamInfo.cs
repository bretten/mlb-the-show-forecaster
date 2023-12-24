using System.ComponentModel.DataAnnotations;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.Enums;

/// <summary>
/// All the MLB teams. The enum members are the team abbreviations (eg, SEA).
/// The constant integers of members are the ID's given by the MLB. And the Display
/// attribute contains the full name.
/// </summary>
public enum TeamInfo
{
    [Display(Name = "Atlanta Braves")] ATL = 144,

    [Display(Name = "Arizona Diamondbacks")]
    AZ = 109,

    [Display(Name = "Baltimore Orioles")] BAL = 110,

    [Display(Name = "Boston Red Sox")] BOS = 111,

    [Display(Name = "Chicago Cubs")] CHC = 112,

    [Display(Name = "Cincinnati Reds")] CIN = 113,

    [Display(Name = "Cleveland Guardians")]
    CLE = 114,

    [Display(Name = "Colorado Rockies")] COL = 115,

    [Display(Name = "Chicago White Sox")] CWS = 145,

    [Display(Name = "Detroit Tigers")] DET = 116,

    [Display(Name = "Houston Astros")] HOU = 117,

    [Display(Name = "Kansas City Royals")] KC = 118,

    [Display(Name = "Los Angeles Angels")] LAA = 108,

    [Display(Name = "Los Angeles Dodgers")]
    LAD = 119,

    [Display(Name = "Miami Marlins")] MIA = 146,

    [Display(Name = "Milwaukee Brewers")] MIL = 158,

    [Display(Name = "Minnesota Twins")] MIN = 142,

    [Display(Name = "New York Mets")] NYM = 121,

    [Display(Name = "New York Yankees")] NYY = 147,

    [Display(Name = "Oakland Athletics")] OAK = 133,

    [Display(Name = "Philadelphia Phillies")]
    PHI = 143,

    [Display(Name = "Pittsburgh Pirates")] PIT = 134,

    [Display(Name = "San Diego Padres")] SD = 135,

    [Display(Name = "Seattle Mariners")] SEA = 136,

    [Display(Name = "San Francisco Giants")]
    SF = 137,

    [Display(Name = "St. Louis Cardinals")]
    STL = 138,

    [Display(Name = "Tampa Bay Rays")] TB = 139,

    [Display(Name = "Texas Rangers")] TEX = 140,

    [Display(Name = "Toronto Blue Jays")] TOR = 141,

    [Display(Name = "Washington Nationals")]
    WSH = 120
}