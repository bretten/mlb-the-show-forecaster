using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Repositories;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services;

/// <summary>
/// Service that finds <see cref="Player"/>s using <see cref="IPlayerRepository"/>
/// </summary>
public sealed class PlayerSearchService : IPlayerSearchService
{
    /// <summary>
    /// The <see cref="IPlayerRepository"/>
    /// </summary>
    private readonly IPlayerRepository _playerRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerRepository">The <see cref="IPlayerRepository"/></param>
    public PlayerSearchService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    /// <summary>
    /// Finds a player based on their name and team
    /// </summary>
    /// <param name="name">The player's name</param>
    /// <param name="team">The player's team</param>
    /// <returns>The <see cref="Player"/> or null if no player was found</returns>
    /// <exception cref="PlayerSearchCouldNotBeRefinedException">Thrown when there are multiple matches for a <see cref="Player"/></exception>
    public async Task<Player?> FindPlayer(string name, Team team)
    {
        // The name may be a first and last name, so get all possible combinations
        var nameCombinations = GetAllPossibleNameCombinations(name);

        // Check the domain's Players for each possible name combination
        var matches = new List<Player>();
        foreach (var nameCombination in nameCombinations)
        {
            // Gets all players in the domain that match this name combination
            var players = await _playerRepository.GetAllByName(nameCombination.First, nameCombination.Last);

            matches.AddRange(players.Where(x => x.Team == team));
        }

        // The criteria of both a name and team is too specific to result in multiple matches. This is an edge case that needs to be examined if it ever occurs
        if (matches.Count > 1)
        {
            throw new PlayerSearchCouldNotBeRefinedException(
                $"Multiple matches for {name}, {team.Abbreviation}: {string.Join(",", matches.Select(x => x.MlbId))}");
        }

        return matches.FirstOrDefault();
    }

    /// <summary>
    /// Given a full name, gets all possible combinations of a first and last name
    /// </summary>
    /// <param name="name">The full name</param>
    /// <returns>All possible combinations of the first and last name</returns>
    private List<FirstAndLastName> GetAllPossibleNameCombinations(string name)
    {
        var possibleNames = new List<FirstAndLastName>();
        var parts = name.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < parts.Length; i++)
        {
            var firstPart = parts.Take(i);
            var secondPart = parts.Skip(i);
            possibleNames.Add(new FirstAndLastName(PersonName.Create(string.Join(" ", firstPart)),
                PersonName.Create(string.Join(" ", secondPart))));
        }

        return possibleNames;
    }

    /// <summary>
    /// Represents a first and last name
    /// </summary>
    private sealed class FirstAndLastName
    {
        public PersonName First { get; }
        public PersonName Last { get; }

        public FirstAndLastName(PersonName first, PersonName last)
        {
            First = first;
            Last = last;
        }
    }
}