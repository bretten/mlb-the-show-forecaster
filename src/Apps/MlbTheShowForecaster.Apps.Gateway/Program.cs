using System.Diagnostics;
using System.Net;
using System.Reflection;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime;
using com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Auth;
using com.brettnamba.MlbTheShowForecaster.Apps.Gateway.GatewayConfig;
using com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Ocelot;
using com.brettnamba.MlbTheShowForecaster.Apps.Gateway.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

const string jobHubUri = "job-hub";

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine($"Running {FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()!.Location).ProductVersion}");

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);
builder.Configuration.AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>(true);
}

// Gateway config
var gatewayConfig = builder.Configuration.GetSection("TargetApps").Get<GatewayConfiguration>()!;

// Ocelot config
builder.ConfigureOcelot(gatewayConfig);

// Services
builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddHealthChecks();

// AWS Cognito
builder.Services.AddTransient<IAmazonCognitoIdentityProvider, AmazonCognitoIdentityProviderClient>(_ =>
    new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(),
        RegionEndpoint.GetBySystemName(builder.Configuration["Aws:Region"])));
// Authentication
var authConfig = GetAuthConfig(builder);
builder.Services.AddSingleton(authConfig);
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options => JwtBearerOptions(authConfig, options));

// Authorization (using AWS Cognito roles)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthConstants.AdminPolicy,
        policy => policy.RequireClaim(authConfig.GroupsClaim, authConfig.AdminGroup));
    options.AddPolicy(AuthConstants.ViewerPolicy,
        policy => policy.RequireClaim(authConfig.GroupsClaim, authConfig.ViewerGroup));
    options.AddPolicy(AuthConstants.AnyRolePolicy,
        policy => policy.RequireClaim(authConfig.GroupsClaim, authConfig.AdminGroup, authConfig.ViewerGroup));
});

// CORS (only allowed in development mode, such as developing in a separate SPA)
var allowCors = authConfig.AllowCors && builder.Environment.IsDevelopment();
if (allowCors)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigins",
            corsPolicyBuilder =>
            {
                var trustedOrigins = builder.Configuration.GetSection("CorsTrustedOrigins").Get<string[]>();
                if (trustedOrigins == null)
                {
                    throw new ArgumentException("No trusted origins specified");
                }

                corsPolicyBuilder.WithOrigins(trustedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
    });
}

// SPA
var spaConfig = builder.Configuration.GetRequiredSection("Spa");
var useSpa = spaConfig.GetValue<bool>("Active");
var spaRoot = spaConfig.GetValue<string>("RootPath")!;
var spaPathsToReserve = spaConfig.GetSection("PathsToReserve").Get<string[]>()!;
if (useSpa)
{
    builder.Services.AddSpaStaticFiles(configuration => { configuration.RootPath = spaRoot; });
}

// SignalR multiplexer
builder.Services.AddSingleton<SignalRMultiplexer.Options>(_ =>
{
    var interval = TimeSpan.Parse(builder.Configuration["SignalRMultiplexer:Interval"] ??
                                  throw new ArgumentException("Missing SignalR interval"));
    var hubs = gatewayConfig.All.Select(x => new RelayedHub($"{x.Url}/{jobHubUri}", x.Methods)).ToHashSet();
    return new SignalRMultiplexer.Options(interval, hubs);
});
builder.Services.AddSingleton<HubCurrentState>();
builder.Services.AddHostedService<SignalRMultiplexer>();

// Ocelot should be last
builder.Services.AddOcelot();
builder.Services.AddAwsCognitoClaimsAuthorizer();

var app = builder.Build();

if (allowCors)
{
    app.UseCors("AllowSpecificOrigins");
}

// Routing and auth
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
if (useSpa)
{
    UseSpaAndGateway(app, Path.Combine(builder.Environment.ContentRootPath, spaRoot), spaPathsToReserve);
}
else
{
    await UseOnlyGateway(app);
}

await app.RunAsync();
return;

static async Task UseOnlyGateway(IApplicationBuilder app)
{
    // In order to use both Ocelot and ASP.NET controllers, you need UseEndpoints => MapControllers, not just MapControllers (https://github.com/Burgyn/MMLib.SwaggerForOcelot/issues/277#issuecomment-2004186620)
#pragma warning disable ASP0014
    app.UseEndpoints(
        routeBuilder =>
        {
            routeBuilder.MapControllers();
            routeBuilder.MapHub<GatewayHub>($"/{jobHubUri}");
            routeBuilder.MapHealthChecks("/healthz")
                .RequireAuthorization();
        });
#pragma warning restore ASP0014

    // Needs to be before UseOcelot (https://stackoverflow.com/a/63472914/1251396)
    app.UseWebSockets();

    // Ocelot should be last
    await app.UseOcelot();
}

static void UseSpaAndGateway(IApplicationBuilder app, string sourcePath, string[] pathsToReserve)
{
    app.UseStaticFiles();
    app.UseSpaStaticFiles();

    // SPA
    var reservedPaths = pathsToReserve.Select(x => $"/{x}") // Reserve endpoints for SPA
        .Append("/"); // Reserve the root
    app.MapWhen(context => reservedPaths.Any(x => context.Request.Path == x),
        applicationBuilder =>
        {
            applicationBuilder.UseSpa(spaBuilder => { spaBuilder.Options.SourcePath = sourcePath; });
        });

    // Needs to be before UseOcelot (https://stackoverflow.com/a/63472914/1251396)
    app.UseWebSockets();

    async void ApiMapping(IApplicationBuilder applicationBuilder)
    {
        // In order to use both Ocelot and ASP.NET controllers, you need UseEndpoints => MapControllers, not just MapControllers (https://github.com/Burgyn/MMLib.SwaggerForOcelot/issues/277#issuecomment-2004186620)
#pragma warning disable ASP0014
        applicationBuilder.UseEndpoints(routeBuilder =>
        {
            routeBuilder.MapControllers();
            routeBuilder.MapHub<GatewayHub>($"/{jobHubUri}");
        });
#pragma warning restore ASP0014

        // Ocelot should be last
        await applicationBuilder.UseOcelot();
    }

    app.Map("/api", ApiMapping);
    app.UseEndpoints(routeBuilder =>
    {
        routeBuilder.MapHealthChecks("/healthz")
            .RequireAuthorization();
    });
}

static AuthConfiguration GetAuthConfig(WebApplicationBuilder builder)
{
    var authConfigSection = builder.Configuration.GetRequiredSection("Auth");
    return new AuthConfiguration(
        AdminGroup: authConfigSection.GetValue<string>("AdminGroup") ??
                    throw new ArgumentException("Missing admin group"),
        ViewerGroup: authConfigSection.GetValue<string>("ViewerGroup") ??
                     throw new ArgumentException("Missing viewer group"),
        CookieExpirationMinutes: authConfigSection.GetValue<int>("CookieExpirationMinutes"),
        AccessTokenCookie: authConfigSection.GetValue<string>("AccessTokenCookie") ??
                           throw new ArgumentException("Missing access token"),
        RefreshTokenCookie: authConfigSection.GetValue<string>("RefreshTokenCookie") ??
                            throw new ArgumentException("Missing refresh token"),
        TokenRefreshMinutes: authConfigSection.GetValue<int>("TokenRefreshMinutes"),
        GroupsClaim: authConfigSection.GetValue<string>("Jwt:GroupsClaim") ??
                     throw new ArgumentException("Missing groups claim"),
        JwtAuthority: authConfigSection.GetValue<string>("Jwt:Authority") ??
                      throw new ArgumentException("Missing authority"),
        JwtAudience: authConfigSection.GetValue<string>("Jwt:Audience") ??
                     throw new ArgumentException("Missing audience"),
        AllowCors: authConfigSection.GetValue<bool>("AllowCors")
    );
}

static void JwtBearerOptions(AuthConfiguration authConfig, JwtBearerOptions options)
{
    options.Authority = authConfig.JwtAuthority;
    options.Audience = authConfig.JwtAudience;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false, // AWS cognito does not return "aud" attribute
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        // Validates the audience using a claim from the access token sine "aud" is not returned
        AudienceValidator = (_, securityToken, _) =>
        {
            var jwtToken = securityToken as JsonWebToken;

            // AWS Cognito client ID
            var clientIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "client_id");

            if (clientIdClaim != null)
            {
                // Validate the client ID against the audience
                return clientIdClaim.Value == options.Audience;
            }

            return false;
        }
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var cognitoProvider =
                context.HttpContext.RequestServices.GetRequiredService<IAmazonCognitoIdentityProvider>();

            if (context.SecurityToken is not JsonWebToken token)
            {
                context.Fail("No access token");
                return;
            }

            try
            {
                var accessToken = token.EncodedToken;

                // Verifies the user is still authenticated against AWS Cognito
                var userResponse = await cognitoProvider.GetUserAsync(new GetUserRequest
                {
                    AccessToken = accessToken
                });

                if (userResponse.HttpStatusCode != HttpStatusCode.OK)
                {
                    context.Fail("Invalid token");
                }
            }
            catch (NotAuthorizedException)
            {
                context.Fail("Not authorized");
            }
        },
        OnMessageReceived = context => // (in other words, on an HTTP request)
        {
            // If the user is trying to log in, no action needed
            var action = (string?)context.HttpContext.GetRouteValue("action");
            if (!string.IsNullOrWhiteSpace(action) && action == "Login")
            {
                return Task.CompletedTask;
            }

            // Any other endpoint requires authentication, so extract the token from the cookie
            var cookie = context.HttpContext.Request.Cookies[authConfig.AccessTokenCookie];
            if (!string.IsNullOrEmpty(cookie))
            {
                context.Token = cookie; // Set the JWT token from the cookie
            }

            return Task.CompletedTask;
        }
    };
}