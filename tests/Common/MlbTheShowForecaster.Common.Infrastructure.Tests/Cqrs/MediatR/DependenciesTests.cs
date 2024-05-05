using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Cqrs.MediatR;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Cqrs.TestClasses;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Cqrs.MediatR;

public class DependenciesTests
{
    [Fact]
    public void AddMediatRCqrs_ServicesCollection_RegistersMediatRCqrs()
    {
        // Arrange
        var s = new ServiceCollection();
        var assembliesToScan = new List<Assembly>()
        {
            GetType().Assembly
        };

        // Act
        s.AddMediatRCqrs(assembliesToScan);
        var actual = s.BuildServiceProvider();

        /*
         * Assert
         */
        // ICommandSender registered as MediatRCommandSender
        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(ICommandSender)).Lifetime);
        Assert.IsType<MediatRCommandSender>(actual.GetRequiredService<ICommandSender>());

        // ICommandHandler<TestCommand> registered as concrete TestCommandHandler
        Assert.Equal(ServiceLifetime.Transient,
            s.First(x => x.ServiceType == typeof(ICommandHandler<TestCommand>)).Lifetime);
        Assert.IsType<TestCommandHandler>(actual.GetRequiredService<ICommandHandler<TestCommand>>());

        // IRequestHandler<MediatRCommand<TestCommand>> registered as MediatRCommandHandler<TestCommand>
        Assert.Equal(ServiceLifetime.Transient,
            s.First(x => x.ServiceType == typeof(IRequestHandler<MediatRCommand<TestCommand>>)).Lifetime);
        Assert.IsType<MediatRCommandHandler<TestCommand>>(actual
            .GetRequiredService<IRequestHandler<MediatRCommand<TestCommand>>>());

        // IQuerySender registered as MediatRQuerySender
        Assert.Equal(ServiceLifetime.Transient, s.First(x => x.ServiceType == typeof(IQuerySender)).Lifetime);
        Assert.IsType<MediatRQuerySender>(actual.GetRequiredService<IQuerySender>());

        // IQueryHandler<TestQuery, TestResponse> registered as concrete TestQueryHandler
        Assert.Equal(ServiceLifetime.Transient,
            s.First(x => x.ServiceType == typeof(IQueryHandler<TestQuery, TestResponse>)).Lifetime);
        Assert.IsType<TestQueryHandler>(actual.GetRequiredService<IQueryHandler<TestQuery, TestResponse>>());

        // IRequestHandler<MediatRQuery<TestQuery, TestResponse>, TestResponse> registered as MediatRQueryHandler<TestQuery, TestResponse>
        Assert.Equal(ServiceLifetime.Transient,
            s.First(x => x.ServiceType == typeof(IRequestHandler<MediatRQuery<TestQuery, TestResponse>, TestResponse>))
                .Lifetime);
        Assert.IsType<MediatRQueryHandler<TestQuery, TestResponse>>(
            actual.GetRequiredService<IRequestHandler<MediatRQuery<TestQuery, TestResponse>, TestResponse>>());
    }
}