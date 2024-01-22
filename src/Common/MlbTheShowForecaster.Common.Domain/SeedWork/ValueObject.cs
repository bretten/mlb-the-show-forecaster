namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;

/// <summary>
/// Defined by its nested value(s) and is immutable. It is equal to another ValueObject if all the contained values
/// are the same
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    /// The nested values that make up the whole ValueObject
    ///
    /// The values are used to determine equality to another ValueObject by comparing each ValueObject's
    /// collection of nested values
    /// </summary>
    /// <returns>The values</returns>
    protected virtual IEnumerable<object?> GetNestedValues()
    {
        return GetType()
            .GetProperties()
            .Select(x => x.GetValue(this));
    }

    /// <summary>
    /// Determines if the other ValueObject is equal to this one by comparing their nested values
    /// </summary>
    /// <param name="other">The ValueObject to compare to</param>
    /// <returns>True if the nested values are equal</returns>
    public bool Equals(ValueObject? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != GetType())
        {
            return false;
        }

        return GetNestedValues().SequenceEqual(other.GetNestedValues());
    }

    /// <summary>
    /// Determines if the other ValueObject is equal to this one by comparing their nested values
    /// </summary>
    /// <param name="obj">The object to compare to</param>
    /// <returns>True if the nested values are equal</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        if (obj is not ValueObject valueObject)
        {
            return false;
        }

        return GetNestedValues().SequenceEqual(valueObject.GetNestedValues());
    }

    /// <summary>
    /// Combines the hash codes of all the nested values to determine the whole ValueObject's hash code
    /// </summary>
    /// <returns>The hash code</returns>
    public override int GetHashCode()
    {
        return GetNestedValues().Aggregate(default(int), HashCode.Combine);
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !Equals(left, right);
    }
}