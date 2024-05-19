using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Database;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Database;

public class DbAtomicDatabaseOperationTests
{
    [Fact]
    public async Task GetCurrentActiveConnection_NoParams_ReturnsConnection()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var mockConnection = Mock.Of<DbConnection>();

        var mockDbSource = new MockDbDataSource(mockConnection, Mock.Of<DbTransaction>());

        var op = new DbAtomicDatabaseOperation(mockDbSource);

        // Act
        var actual = await op.GetCurrentActiveConnection(cToken);

        // Assert
        Assert.Equal(mockConnection, actual);
    }

    [Fact]
    public async Task GetCurrentActiveTransaction_NoParams_ReturnsTransaction()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var mockTransaction = Mock.Of<DbTransaction>();
        var mockConnection = new MockDbConnection(mockTransaction);

        var mockDbSource = new MockDbDataSource(mockConnection, mockTransaction);

        var op = new DbAtomicDatabaseOperation(mockDbSource);

        // Act
        var actual = await op.GetCurrentActiveTransaction(cToken);

        // Assert
        Assert.Equal(mockTransaction, actual);
        Assert.True(mockConnection.IsTransactionStarted);
    }

    [Fact]
    public async Task Dispose_NoParams_DbResourcesDisposed()
    {
        // Arrange
        var mockTransaction = new MockDbTransaction();
        var mockConnection = new MockDbConnection(mockTransaction);

        var mockDbSource = new MockDbDataSource(mockConnection, mockTransaction);

        var op = new DbAtomicDatabaseOperation(mockDbSource);

        // Act
        await op.GetCurrentActiveTransaction();
        op.Dispose();

        // Assert
        Assert.True(mockConnection.IsDisposed);
        Assert.True(mockTransaction.IsDisposed);
        Assert.True(mockDbSource.IsDisposed);
    }

    [Fact]
    public async Task DisposeAsync_NoParams_DbResourcesDisposed()
    {
        // Arrange
        var mockTransaction = new Mock<DbTransaction>();
        var mockConnection = new MockDbConnection(mockTransaction.Object);

        var mockDbSource = new MockDbDataSource(mockConnection, mockTransaction.Object);

        var op = new DbAtomicDatabaseOperation(mockDbSource);

        // Act
        await op.GetCurrentActiveTransaction();
        await op.DisposeAsync();

        // Assert
        Assert.True(mockConnection.IsDisposedAsync);
        mockTransaction.Verify(x => x.DisposeAsync(), Times.Once);
        Assert.True(mockDbSource.IsDisposedAsync);
    }

    private sealed class MockDbDataSource : DbDataSource
    {
        public override string ConnectionString => "Data Source=:memory:";
        public DbConnection DbConnection { get; }
        public DbTransaction DbTransaction { get; }
        public bool IsDisposed { get; private set; }
        public bool IsDisposedAsync { get; private set; }

        public MockDbDataSource(DbConnection dbConnection, DbTransaction dbTransaction)
        {
            DbConnection = dbConnection;
            DbTransaction = dbTransaction;
        }

        protected override DbConnection CreateDbConnection()
        {
            return DbConnection;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            IsDisposed = true;
        }

        protected override ValueTask DisposeAsyncCore()
        {
            IsDisposedAsync = true;
            return base.DisposeAsyncCore();
        }
    }

    private sealed class MockDbConnection : DbConnection
    {
        public DbTransaction DbTransaction { get; }
        public bool IsTransactionStarted { get; private set; }
        public bool IsDisposed { get; private set; }
        public bool IsDisposedAsync { get; private set; }

        public MockDbConnection(DbTransaction dbTransaction)
        {
            DbTransaction = dbTransaction;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            IsTransactionStarted = true;
            return DbTransaction;
        }

        public override void ChangeDatabase(string databaseName)
        {
        }

        public override void Close()
        {
        }

        public override void Open()
        {
        }

        [AllowNull] public override string ConnectionString { get; set; }
        public override string Database { get; } = null!;
        public override ConnectionState State => ConnectionState.Open;
        public override string DataSource { get; } = null!;
        public override string ServerVersion { get; } = null!;

        protected override DbCommand CreateDbCommand()
        {
            return null!;
        }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }

        public override ValueTask DisposeAsync()
        {
            IsDisposedAsync = true;
            return base.DisposeAsync();
        }
    }

    private sealed class MockDbTransaction : DbTransaction
    {
        public bool IsDisposed { get; private set; }

        public override void Commit()
        {
        }

        public override void Rollback()
        {
        }

        protected override DbConnection? DbConnection { get; } = null!;
        public override IsolationLevel IsolationLevel => IsolationLevel.Unspecified;

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }
}