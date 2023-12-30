using Microsoft.EntityFrameworkCore;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.EntityFramework.TestClasses;

public class TestDbContext : DbContext
{
    public DbSet<TestEntity>? TestEntities { get; set; }
}