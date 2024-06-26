﻿using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

/// <summary>
/// Represents the ID that is given to a player by MLB
/// </summary>
public sealed class MlbId : ValueObject
{
    /// <summary>
    /// The underlying MLB ID value
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The MLB ID</param>
    [JsonConstructor]
    private MlbId(int value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a <see cref="MlbId"/>
    /// </summary>
    /// <param name="mlbId">The underlying MLB ID value</param>
    /// <returns>The created <see cref="MlbId"/></returns>
    /// <exception cref="InvalidMlbIdException">Thrown if the specified value is invalid (less than 1)</exception>
    public static MlbId Create(int mlbId)
    {
        if (mlbId < 1)
        {
            throw new InvalidMlbIdException($"MLB ID is not a valid ID as defined by MLB.com: {mlbId}");
        }

        return new MlbId(mlbId);
    }
}