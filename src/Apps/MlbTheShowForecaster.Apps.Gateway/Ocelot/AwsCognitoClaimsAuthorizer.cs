using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Ocelot.Authorization;
using Ocelot.DownstreamRouteFinder.UrlMatcher;
using Ocelot.Responses;

namespace com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Ocelot;

/// <summary>
/// https://github.com/ThreeMammals/Ocelot/issues/679#issuecomment-662905004
///
/// In the case of AWS cognito, its claim type name is "cognito:group". So use "cognito/" instead and replace to what AWS sends
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class AwsCognitoClaimsAuthorizer : IClaimsAuthorizer
{
    /// <summary>
    /// The default <see cref="ClaimsAuthorizer"/> that is normally injected by Ocelot
    /// </summary>
    private readonly ClaimsAuthorizer _claimsAuthorizer;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="claimsAuthorizer">The default <see cref="ClaimsAuthorizer"/> that is normally injected by Ocelot</param>
    public AwsCognitoClaimsAuthorizer(ClaimsAuthorizer claimsAuthorizer)
    {
        _claimsAuthorizer = claimsAuthorizer;
    }

    /// <inheritdoc />
    public Response<bool> Authorize(ClaimsPrincipal claimsPrincipal, Dictionary<string, string> routeClaimsRequirement,
        List<PlaceholderNameAndValue> urlPathPlaceholderNameAndValues)
    {
        var updatedClaimsRequirement = new Dictionary<string, string>();
        foreach (var claim in routeClaimsRequirement)
        {
            // If we see the custom AWS cognito claim, replace it with its actual value
            if (claim.Key.StartsWith("cognito/"))
            {
                var key = claim.Key.Replace("cognito/", "cognito:");
                updatedClaimsRequirement.Add(key, claim.Value);
            }
            else
            {
                // Otherwise, just add the claim as it was originally defined
                updatedClaimsRequirement.Add(claim.Key, claim.Value);
            }
        }

        return _claimsAuthorizer.Authorize(claimsPrincipal, updatedClaimsRequirement, urlPathPlaceholderNameAndValues);
    }
}