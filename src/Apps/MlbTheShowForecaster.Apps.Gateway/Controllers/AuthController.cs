using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Auth;
using com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Controllers.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Controllers.Requests;
using com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Controllers.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Controllers;

/// <summary>
/// Authenticates users
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    /// <summary>
    /// <see cref="IAmazonCognitoIdentityProvider"/>
    /// </summary>
    private readonly IAmazonCognitoIdentityProvider _cognitoIdentityProvider;

    /// <summary>
    /// Configuration
    /// </summary>
    private readonly AuthConfiguration _config;

    /// <summary>
    /// Cookie options for same site cookies
    /// </summary>
    private CookieOptions SameSiteCookie => new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = DateTime.UtcNow.AddMinutes(_config.CookieExpirationMinutes)
    };

    /// <summary>
    /// Cookie options when CORS is active (development mode)
    /// </summary>
    private CookieOptions CorsCookie => new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.None,
        Expires = DateTime.UtcNow.AddMinutes(_config.CookieExpirationMinutes)
    };

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="cognitoIdentityProvider"><see cref="IAmazonCognitoIdentityProvider"/></param>
    /// <param name="config">Configuration</param>
    public AuthController(IAmazonCognitoIdentityProvider cognitoIdentityProvider, AuthConfiguration config)
    {
        _cognitoIdentityProvider = cognitoIdentityProvider;
        _config = config;
    }

    /// <summary>
    /// Allows a user to log in
    /// </summary>
    /// <param name="request">Login request</param>
    /// <returns><see cref="IActionResult"/></returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCredentials request)
    {
        try
        {
            var authResponse = await InitiateAuth(request.Username, request.Password);
            var accessToken = new JsonWebToken(authResponse.AuthenticationResult.AccessToken);

            // Store the tokens in cookies
            SetCookies(accessToken: accessToken.EncodedToken, authResponse.AuthenticationResult.RefreshToken);

            var user = GetJwtUserInfo(accessToken);

            return Ok(new AuthenticatedResponse(Message: "Login successful", Username: user.Username, Role: user.Role));
        }
        catch (NotAuthorizedException)
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }
    }

    /// <summary>
    /// Allows a user to log out
    /// </summary>
    /// <returns><see cref="IActionResult"/></returns>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        // Get the access token so it can be invalidated
        var token = HttpContext.Request.Cookies[_config.AccessTokenCookie];

        RemoveCookies();

        // Invalidate the access token by signing out
        try
        {
            await _cognitoIdentityProvider.GlobalSignOutAsync(new GlobalSignOutRequest()
            {
                AccessToken = token
            });
        }
        catch (NotAuthorizedException e)
        {
            // If the token has expired, the logout was still successful due to removal of cookies
            if (e.Message == "Access Token has expired") return NoContent();
        }

        return NoContent();
    }

    /// <summary>
    /// Verifies a user is still authenticated by checking the tokens in the cookies
    /// </summary>
    /// <returns><see cref="IActionResult"/></returns>
    [HttpPost("verify")]
    public async Task<IActionResult> Verify()
    {
        if (!HttpContext.User.Identity?.IsAuthenticated ?? false)
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        // Get the tokens
        var token = new JsonWebToken(HttpContext.Request.Cookies[_config.AccessTokenCookie]);
        var refreshToken = HttpContext.Request.Cookies[_config.RefreshTokenCookie]!;

        // Check if the token has expired
        var tokenTimeLeft = token.ValidTo - DateTime.UtcNow;
        if (tokenTimeLeft.Minutes <= _config.TokenRefreshMinutes)
        {
            // We are initiating auth via refresh token, so no refresh token will be returned
            var authResponse = await InitiateAuth(refreshToken);

            // Update the cookies with the new access token. Refresh token remains the same
            SetCookies(accessToken: authResponse.AuthenticationResult.AccessToken, refreshToken);
        }

        var user = GetJwtUserInfo(token);

        return Ok(new AuthenticatedResponse(Message: "Verified", Username: user.Username, Role: user.Role));
    }

    /// <summary>
    /// Authenticates a user against AWS Cognito using a username and password
    /// </summary>
    /// <param name="username">The username</param>
    /// <param name="password">The password</param>
    /// <returns><see cref="InitiateAuthResponse"/></returns>
    private async Task<InitiateAuthResponse> InitiateAuth(string username, string password)
    {
        var authParameters = new Dictionary<string, string>();
        authParameters.Add("USERNAME", username);
        authParameters.Add("PASSWORD", password);

        var request = new InitiateAuthRequest
        {
            ClientId = _config.JwtAudience,
            AuthParameters = authParameters,
            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
        };

        return await _cognitoIdentityProvider.InitiateAuthAsync(request);
    }

    /// <summary>
    /// Authenticates a user against AWS Cognito using a refresh token
    /// </summary>
    /// <param name="refreshToken">The refresh token</param>
    /// <returns><see cref="InitiateAuthResponse"/></returns>
    private async Task<InitiateAuthResponse> InitiateAuth(string refreshToken)
    {
        var authParameters = new Dictionary<string, string>();
        authParameters.Add("REFRESH_TOKEN", refreshToken);

        var request = new InitiateAuthRequest
        {
            ClientId = _config.JwtAudience,
            AuthParameters = authParameters,
            AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
        };

        return await _cognitoIdentityProvider.InitiateAuthAsync(request);
    }

    /// <summary>
    /// Extracts user information from the JWT token
    /// </summary>
    /// <param name="jsonWebToken">The JWT token</param>
    /// <returns>User information</returns>
    /// <exception cref="JwtMissingIdException">Thrown when the JWT token is missing information or claims</exception>
    private JwtUserInfo GetJwtUserInfo(JsonWebToken jsonWebToken)
    {
        var usernameFound = jsonWebToken.TryGetPayloadValue<string>("username", out var user);
        var role = jsonWebToken.Claims.FirstOrDefault(x => x.Type == _config.GroupsClaim)?.Value;
        if (!usernameFound || string.IsNullOrWhiteSpace(role))
        {
            throw new JwtMissingIdException(user, role);
        }

        return new JwtUserInfo()
        {
            Username = user,
            Role = role
        };
    }

    /// <summary>
    /// Sets the tokens in cookies
    /// </summary>
    /// <param name="accessToken">The access token</param>
    /// <param name="refreshToken">The refresh token</param>
    private void SetCookies(string accessToken, string refreshToken)
    {
        RemoveCookies();
        var cookieOptions = _config.AllowCors ? CorsCookie : SameSiteCookie;
        Response.Cookies.Append(_config.AccessTokenCookie, accessToken, cookieOptions);
        Response.Cookies.Append(_config.RefreshTokenCookie, refreshToken, cookieOptions);
    }

    /// <summary>
    /// Removes cookies that store tokens
    /// </summary>
    private void RemoveCookies()
    {
        var cookieOptions = _config.AllowCors ? CorsCookie : SameSiteCookie;

        Response.Cookies.Delete(_config.AccessTokenCookie, cookieOptions);
        Response.Cookies.Delete(_config.RefreshTokenCookie, cookieOptions);
    }

    private class JwtUserInfo
    {
        public string Username { get; init; } = null!;
        public string Role { get; init; } = null!;
    }
}