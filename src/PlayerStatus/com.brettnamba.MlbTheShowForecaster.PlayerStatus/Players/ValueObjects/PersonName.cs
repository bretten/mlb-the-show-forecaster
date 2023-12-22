using com.brettnamba.MlbTheShowForecaster.Core.SeedWork;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Players.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Players.ValueObjects;

/// <summary>
/// A person's first or last name
/// </summary>
public sealed class PersonName : ValueObject
{
    /// <summary>
    /// The underlying name value
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The name</param>
    private PersonName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a <see cref="PersonName"/>
    /// </summary>
    /// <param name="name">The underlying name value</param>
    /// <returns>The <see cref="PersonName"/></returns>
    /// <exception cref="EmptyPersonNameException">Thrown if the underlying name value is empty</exception>
    public static PersonName Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new EmptyPersonNameException("A person's name cannot be empty");
        }

        return new PersonName(name);
    }
}