namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Commands.UpdatePlayer.Exceptions;

/// <summary>
/// Thrown when <see cref="UpdatePlayerCommandHandler"/> does not specify a team when a player signs a contract
/// </summary>
public sealed class MissingTeamContractSigningException : Exception
{
    public MissingTeamContractSigningException(string? message) : base(message)
    {
    }
}