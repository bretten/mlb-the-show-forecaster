using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;

public static class Dependencies
{
    public static void AddRabbitMq(this IServiceCollection services, IList<Assembly> assembliesToScan)
    {
    }
}