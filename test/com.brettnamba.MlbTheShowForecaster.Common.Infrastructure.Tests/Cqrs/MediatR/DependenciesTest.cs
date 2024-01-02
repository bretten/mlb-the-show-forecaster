using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Cqrs.MediatR;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Cqrs.TestClasses;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Cqrs.MediatR;

public class DependenciesTest
{
    [Fact]
    public void AddMediatRCqrs_ServicesCollection_RegistersMediatRCqrs()
    {
        // Arrange
        var mockServices = Mock.Of<IServiceCollection>();
        var assembliesToScan = new List<Assembly>()
        {
            GetType().Assembly
        };

        // Act
        mockServices.AddMediatRCqrs(assembliesToScan);

        /*
         * Assert
         */
        // ICommandSender registered as MediatRCommandSender
        Mock.Get(mockServices).Verify(s => s.Add(It.Is<ServiceDescriptor>(x =>
            x.ServiceType == typeof(ICommandSender) &&
            x.ImplementationType == typeof(MediatRCommandSender) &&
            x.Lifetime == ServiceLifetime.Transient)), Times.Once);

        // Concrete ICommandHandler<TestCommand> registered as TestCommandHandler
        Mock.Get(mockServices).Verify(s => s.Add(It.Is<ServiceDescriptor>(x =>
            x.ServiceType == typeof(ICommandHandler<TestCommand>) &&
            x.ImplementationType == typeof(TestCommandHandler) &&
            x.Lifetime == ServiceLifetime.Transient)), Times.Once);

        // IRequestHandler<MediatRCommand<TestCommand>> registered as MediatRCommandHandler<TestCommand>
        Mock.Get(mockServices).Verify(s => s.Add(It.Is<ServiceDescriptor>(x =>
            x.ServiceType == typeof(IRequestHandler<MediatRCommand<TestCommand>>) &&
            x.ImplementationType == typeof(MediatRCommandHandler<TestCommand>) &&
            x.Lifetime == ServiceLifetime.Transient)), Times.Once);

        // IQuerySender registered as MediatRQuerySender
        Mock.Get(mockServices).Verify(s => s.Add(It.Is<ServiceDescriptor>(x =>
            x.ServiceType == typeof(IQuerySender) &&
            x.ImplementationType == typeof(MediatRQuerySender) &&
            x.Lifetime == ServiceLifetime.Transient)), Times.Once);

        // Concrete IQueryHandler<TestQuery, TestResponse> registered as TestQueryHandler
        Mock.Get(mockServices).Verify(s => s.Add(It.Is<ServiceDescriptor>(x =>
            x.ServiceType == typeof(IQueryHandler<TestQuery, TestResponse>) &&
            x.ImplementationType == typeof(TestQueryHandler) &&
            x.Lifetime == ServiceLifetime.Transient)), Times.Once);

        // IRequestHandler<MediatRQuery<TestQuery, TestResponse>, TestResponse> registered as MediatRQueryHandler<TestQuery, TestResponse>
        Mock.Get(mockServices).Verify(s => s.Add(It.Is<ServiceDescriptor>(x =>
            x.ServiceType == typeof(IRequestHandler<MediatRQuery<TestQuery, TestResponse>, TestResponse>) &&
            x.ImplementationType == typeof(MediatRQueryHandler<TestQuery, TestResponse>) &&
            x.Lifetime == ServiceLifetime.Transient)), Times.Once);
    }
}