using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Auth;
using com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Controllers;
using com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Controllers.Requests;
using com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Tests.Controllers;

public class AuthControllerTests
{
    [Fact]
    public async Task Login_ValidCredentials_ReturnsOk()
    {
        // Arrange
        var stubCognito = MockCognito(initAuthSuccess: true);
        var mockConfig = MockAuthConfig();

        var controller = GetAuthController(stubCognito.Object, mockConfig);

        // Act
        var actual = await controller.Login(new LoginCredentials(Username: "user", Password: "password"));

        // Assert
        Assert.IsType<OkObjectResult>(actual);
        var value = (((OkObjectResult)actual).Value as AuthenticatedResponse)!;
        Assert.Equal("user", value.Username);
        Assert.Equal("Admins", value.Role);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var stubCognito = MockCognito(initAuthSuccess: false);
        var mockConfig = MockAuthConfig();

        var controller = GetAuthController(stubCognito.Object, mockConfig);

        // Act
        var actual = await controller.Login(new LoginCredentials(Username: "user", Password: "password"));

        // Assert
        Assert.IsType<StatusCodeResult>(actual);
        Assert.Equal(StatusCodes.Status401Unauthorized, ((StatusCodeResult)actual).StatusCode);
    }

    [Fact]
    public async Task Logout_ValidToken_ReturnsNoContent()
    {
        // Arrange
        var stubCognito = MockCognito(signOutSuccess: true);
        var mockConfig = MockAuthConfig();

        var controller = GetAuthController(stubCognito.Object, mockConfig);

        // Act
        var actual = await controller.Logout();

        // Assert
        Assert.IsType<NoContentResult>(actual);
    }

    [Fact]
    public async Task Logout_ExpiredToken_ReturnsNoContent()
    {
        // Arrange
        var stubCognito = MockCognito(signOutSuccess: false);
        var mockConfig = MockAuthConfig();

        var controller = GetAuthController(stubCognito.Object, mockConfig);

        // Act
        var actual = await controller.Logout();

        // Assert
        Assert.IsType<NoContentResult>(actual);
    }

    [Fact]
    public async Task Verify_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var stubCognito = MockCognito();
        var mockConfig = MockAuthConfig();

        var controller = GetAuthController(stubCognito.Object, mockConfig);

        // Act
        var actual = await controller.Verify();

        // Assert
        Assert.IsType<StatusCodeResult>(actual);
        Assert.Equal(StatusCodes.Status401Unauthorized, ((StatusCodeResult)actual).StatusCode);
    }

    [Fact]
    public async Task Verify_Authenticated_ReturnsNoContent()
    {
        // Arrange
        var stubCognito = MockCognito(initAuthSuccess: true);
        var mockConfig = MockAuthConfig();
        var currentAccessToken = GenerateJwtToken();

        var controller = GetAuthController(stubCognito.Object, mockConfig);
        controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(
            new List<Claim>()
            {
                new Claim("username", "user"),
            }, "Test"
        ));
        var accessTokenCookie = $"{mockConfig.AccessTokenCookie}={currentAccessToken}";
        var refreshTokenCookie = $"{mockConfig.RefreshTokenCookie}=refresh";
        controller.ControllerContext.HttpContext.Request.Headers["Cookie"] =
            $"{accessTokenCookie}; {refreshTokenCookie}";

        // Act
        var actual = await controller.Verify();

        // Assert
        Assert.IsType<OkObjectResult>(actual);
        var value = (((OkObjectResult)actual).Value as AuthenticatedResponse)!;
        Assert.Equal("user", value.Username);
        Assert.Equal("Admins", value.Role);
    }

    private static Mock<IAmazonCognitoIdentityProvider> MockCognito(bool initAuthSuccess = false,
        bool signOutSuccess = false)
    {
        var token = GenerateJwtToken();
        var stubCognito = new Mock<IAmazonCognitoIdentityProvider>();

        // Mock log in
        if (initAuthSuccess)
        {
            var authResult = new AuthenticationResultType()
            {
                AccessToken = token,
                RefreshToken = "refresh"
            };
            stubCognito.Setup(x => x.InitiateAuthAsync(It.IsAny<InitiateAuthRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new InitiateAuthResponse() { AuthenticationResult = authResult });
        }
        else
        {
            stubCognito.Setup(x => x.InitiateAuthAsync(It.IsAny<InitiateAuthRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotAuthorizedException("Not Authorized"));
        }

        // Mock log out
        if (signOutSuccess)
        {
            stubCognito.Setup(
                    x => x.GlobalSignOutAsync(It.IsAny<GlobalSignOutRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GlobalSignOutResponse());
        }
        else
        {
            stubCognito.Setup(
                    x => x.GlobalSignOutAsync(It.IsAny<GlobalSignOutRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotAuthorizedException("Access Token has expired"));
        }

        return stubCognito;
    }

    private static AuthConfiguration MockAuthConfig(bool allowCors = false)
    {
        return new AuthConfiguration(AdminGroup: "Admins",
            ViewerGroup: "Viewers",
            AccessTokenCookie: "jwtAccessCookie",
            RefreshTokenCookie: "jwtRefreshCookie",
            TokenRefreshMinutes: 1,
            GroupsClaim: "cognito:groups",
            JwtAuthority: "authority",
            JwtAudience: "audience",
            AllowCors: allowCors);
    }

    private static AuthController GetAuthController(IAmazonCognitoIdentityProvider cognito, AuthConfiguration config)
    {
        var controller = new AuthController(cognito, config);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        return controller;
    }

    private static string GenerateJwtToken()
    {
        var key = new byte[32];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(key);
        var securityKey = new SymmetricSecurityKey(key);

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("cognito:groups", "Admins"),
            new Claim("username", "user"),
        };

        var token = new JwtSecurityToken(
            issuer: "issuer",
            audience: "audience",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}