namespace com.brettnamba.MlbTheShowForecaster.Core.SeedWork;

/// <summary>
/// Represents an Entity that other Entities depend on. In order to edit a contained Entity,
/// the whole Aggregate Root must be edited
/// </summary>
public abstract class AggregateRoot : Entity
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id">Unique ID</param>
    protected AggregateRoot(Guid id) : base(id)
    {
    }
}