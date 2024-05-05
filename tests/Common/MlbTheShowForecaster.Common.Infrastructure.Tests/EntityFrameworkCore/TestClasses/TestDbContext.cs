using Microsoft.EntityFrameworkCore;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.EntityFrameworkCore.TestClasses;

public class TestDbContext : DbContext
{
    public DbSet<TestEntity> TestEntities { get; private init; } = null!;

    protected TestDbContext()
    {
    }

    public TestDbContext(DbContextOptions options) : base(options)
    {
    }
}