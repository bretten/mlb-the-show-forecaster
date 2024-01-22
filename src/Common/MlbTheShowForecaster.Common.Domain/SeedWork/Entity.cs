using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;

/// <summary>
/// Represents a uniquely identifiable entity
/// </summary>
public abstract class Entity : IEquatable<Entity>
{
    /// <summary>
    /// Internal collection of domain events raised by this Entity
    /// </summary>
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Unique ID
    /// </summary>
    public Guid Id { get; private init; }

    /// <summary>
    /// Returns the domain events raised by this entity as a read-only collection
    /// </summary>
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id">Unique ID</param>
    protected Entity(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// Raises a domain event
    /// </summary>
    /// <param name="domainEvent">The domain event to raise</param>
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears all domain events
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    /// Determines if the specified Entity is equal to this one
    /// </summary>
    /// <param name="other">The Entity that is being checked</param>
    /// <returns>True if the Entity IDs and types match, otherwise false</returns>
    public bool Equals(Entity? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != GetType())
        {
            return false;
        }

        return other.Id == Id;
    }

    /// <summary>
    /// Determines if the specified object is an Entity and equal to this one
    /// </summary>
    /// <param name="obj">The object that is being checked</param>
    /// <returns>True if object is an Entity of the same type and the IDs are equal</returns>
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

        if (obj.GetType() != GetType() || obj is not Entity entity)
        {
            return false;
        }

        return entity.Id == Id;
    }

    /// <summary>
    /// Determines the hash code by using the Entity's ID
    /// </summary>
    /// <returns>The hash code</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode() * 31; // Prime number
    }

    public static bool operator ==(Entity? left, Entity? right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    public static bool operator !=(Entity? left, Entity? right)
    {
        return !(left == right);
    }
}