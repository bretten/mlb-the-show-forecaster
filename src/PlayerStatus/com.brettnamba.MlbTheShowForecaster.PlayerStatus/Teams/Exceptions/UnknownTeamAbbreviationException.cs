﻿using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.Exceptions;

/// <summary>
/// Thrown when a <see cref="TeamAbbreviation"/> is provided but does not correspond to a <see cref="Team"/>
/// </summary>
public sealed class UnknownTeamAbbreviationException : Exception
{
    public UnknownTeamAbbreviationException(string? message) : base(message)
    {
    }
}