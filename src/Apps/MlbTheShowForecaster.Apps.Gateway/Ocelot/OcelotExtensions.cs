using Ocelot.Authorization;

namespace com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Ocelot;

/// <summary>
/// Ocelot extensions
/// </summary>
public static class OcelotExtensions
{
    /// <summary>
    /// Replaces the default <see cref="IClaimsAuthorizer"/> with <see cref="AwsCognitoClaimsAuthorizer"/>
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns>Updated <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddAwsCognitoClaimsAuthorizer(this IServiceCollection services)
    {
        // Remove the default IClaimsAuthorizer
        var serviceDescriptor = services.First(x => x.ServiceType == typeof(IClaimsAuthorizer));
        services.Remove(serviceDescriptor);

        // Replace the original IClaimsAuthorizer with the AWS Cognito version
        var awsDescriptor = new ServiceDescriptor(serviceDescriptor.ImplementationType!,
            serviceDescriptor.ImplementationType!, serviceDescriptor.Lifetime);
        services.Add(awsDescriptor);

        services.AddTransient<IClaimsAuthorizer, AwsCognitoClaimsAuthorizer>();

        return services;
    }
}