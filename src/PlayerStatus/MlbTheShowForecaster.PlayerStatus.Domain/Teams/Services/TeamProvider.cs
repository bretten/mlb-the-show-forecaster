using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;

/// <summary>
/// Service that provides Teams based on different parameters
/// </summary>
public sealed class TeamProvider : ITeamProvider
{
    /// <summary>
    /// Maps <see cref="MlbId"/> to the corresponding <see cref="Team"/>
    /// </summary>
    private readonly Dictionary<MlbId, Team> _mlbIdToTeam;

    /// <summary>
    /// Maps <see cref="TeamAbbreviation"/> to the corresponding <see cref="Team"/>
    /// </summary>
    private readonly Dictionary<TeamAbbreviation, Team> _abbreviationToTeam;

    /// <summary>
    /// Constructor
    /// </summary>
    public TeamProvider()
    {
        // Create lookups based on keys
        var teams = Enum.GetValues(typeof(TeamInfo)).Cast<TeamInfo>().Select(Team.Create).ToList();
        _mlbIdToTeam = teams.ToDictionary(k => k.MlbId, v => v);
        _abbreviationToTeam = teams.ToDictionary(k => k.Abbreviation, v => v);
    }

    /// <summary>
    /// Gets a <see cref="Team"/> by <see cref="MlbId"/>
    /// </summary>
    /// <param name="mlbId">The <see cref="MlbId"/></param>
    /// <returns>The corresponding <see cref="Team"/></returns>
    /// <exception cref="UnknownTeamMlbIdException">Thrown when the <see cref="MlbId"/> does not correspond to a <see cref="Team"/></exception>
    public Team GetBy(MlbId mlbId)
    {
        if (!_mlbIdToTeam.TryGetValue(mlbId, out var value))
        {
            throw new UnknownTeamMlbIdException($"Cannot get Team due to unknown Team MLB ID: {mlbId.Value}");
        }

        return value;
    }

    /// <summary>
    /// Gets a <see cref="Team"/> by <see cref="TeamAbbreviation"/>
    /// </summary>
    /// <param name="abbreviation">The <see cref="TeamAbbreviation"/></param>
    /// <returns>The corresponding <see cref="Team"/></returns>
    /// <exception cref="UnknownTeamAbbreviationException">Thrown when the <see cref="TeamAbbreviation"/> does not correspond to a <see cref="Team"/></exception>
    public Team GetBy(TeamAbbreviation abbreviation)
    {
        if (!_abbreviationToTeam.TryGetValue(abbreviation, out var value))
        {
            throw new UnknownTeamAbbreviationException(
                $"Cannot get Team due to unknown Team abbreviation: {abbreviation}");
        }

        return value;
    }

    /// <summary>
    /// Retrieves a <see cref="Team"/> by its common names or abbreviations
    /// </summary>
    /// <param name="name">The name of the team</param>
    /// <returns>The corresponding <see cref="Team"/> or null</returns>
    public Team? GetBy(string name)
    {
        switch (name.ToLower())
        {
            case "ari":
            case "az":
            case "arizona":
            case "diamondbacks":
            case "arizona diamondbacks":
                return Team.Create(TeamInfo.AZ);
            case "atl":
            case "braves":
            case "atlanta":
            case "atlanta braves":
                return Team.Create(TeamInfo.ATL);
            case "bal":
            case "orioles":
            case "baltimore":
            case "baltimore orioles":
                return Team.Create(TeamInfo.BAL);
            case "bos":
            case "red sox":
            case "boston":
            case "boston red sox":
                return Team.Create(TeamInfo.BOS);
            case "chc":
            case "cubs":
            case "chicago":
            case "chicago cubs":
            case "chi cubs":
                return Team.Create(TeamInfo.CHC);
            case "cin":
            case "reds":
            case "cincinnati":
            case "cincinnati reds":
                return Team.Create(TeamInfo.CIN);
            case "cle":
            case "guardians":
            case "cleveland":
            case "cleveland guardians":
                return Team.Create(TeamInfo.CLE);
            case "col":
            case "rockies":
            case "colorado":
            case "colorado rockies":
                return Team.Create(TeamInfo.COL);
            case "cha":
            case "cws":
            case "white sox":
            case "chicago white sox":
            case "chi white sox":
                return Team.Create(TeamInfo.CWS);
            case "det":
            case "tigers":
            case "detroit":
            case "detroit tigers":
                return Team.Create(TeamInfo.DET);
            case "hou":
            case "astros":
            case "houston":
            case "houston astros":
                return Team.Create(TeamInfo.HOU);
            case "kc":
            case "kansas city":
            case "royals":
            case "kansas city royals":
                return Team.Create(TeamInfo.KC);
            case "ana":
            case "laa":
            case "angels":
            case "los angeles":
            case "los angeles angels":
            case "la angels":
                return Team.Create(TeamInfo.LAA);
            case "lan":
            case "lad":
            case "dodgers":
            case "la dodgers":
            case "los angeles dodgers":
                return Team.Create(TeamInfo.LAD);
            case "mia":
            case "marlins":
            case "miami":
            case "miami marlins":
                return Team.Create(TeamInfo.MIA);
            case "mil":
            case "brewers":
            case "milwaukee":
            case "milwaukee brewers":
                return Team.Create(TeamInfo.MIL);
            case "min":
            case "twins":
            case "minneapolis":
            case "minnesota":
            case "minnesota twins":
                return Team.Create(TeamInfo.MIN);
            case "nym":
            case "mets":
            case "new york mets":
            case "ny mets":
                return Team.Create(TeamInfo.NYM);
            case "nya":
            case "nyy":
            case "yankees":
            case "new york yankees":
            case "ny yankees":
                return Team.Create(TeamInfo.NYY);
            case "oak":
            case "athletics":
            case "oakland":
            case "oakland athletics":
                return Team.Create(TeamInfo.OAK);
            case "phi":
            case "phillies":
            case "philadelphia":
            case "philadelphia phillies":
                return Team.Create(TeamInfo.PHI);
            case "pit":
            case "pirates":
            case "pittsburgh":
            case "pittsburgh pirates":
                return Team.Create(TeamInfo.PIT);
            case "sdn":
            case "sd":
            case "padres":
            case "san diego":
            case "san diego padres":
                return Team.Create(TeamInfo.SD);
            case "sea":
            case "mariners":
            case "seattle":
            case "seattle mariners":
                return Team.Create(TeamInfo.SEA);
            case "sfn":
            case "sf":
            case "giants":
            case "san francisco":
            case "san francisco giants":
                return Team.Create(TeamInfo.SF);
            case "sln":
            case "stl":
            case "cardinals":
            case "st. louis":
            case "st louis":
            case "st. louis cardinals":
                return Team.Create(TeamInfo.STL);
            case "tba":
            case "tb":
            case "rays":
            case "tampa bay":
            case "tampa bay rays":
                return Team.Create(TeamInfo.TB);
            case "tex":
            case "rangers":
            case "texas":
            case "texas rangers":
                return Team.Create(TeamInfo.TEX);
            case "tor":
            case "blue jays":
            case "toronto":
            case "toronto blue jays":
                return Team.Create(TeamInfo.TOR);
            case "wsh":
            case "was":
            case "nationals":
            case "washington":
            case "washington nationals":
                return Team.Create(TeamInfo.WSH);
            default:
                return null;
        }
    }
}