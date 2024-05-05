using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.EntityFrameworkCore.TestClasses;

public readonly record struct TestEntityCreatedEvent : IDomainEvent;