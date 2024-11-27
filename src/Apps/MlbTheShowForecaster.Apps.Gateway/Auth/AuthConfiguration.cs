namespace com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Auth;

/// <summary>
/// Configuration for authentication and authorization
/// </summary>
/// <param name="AdminGroup">Name of the admin group</param>
/// <param name="ViewerGroup">Name of the viewer group</param>
/// <param name="CookieExpirationMinutes">The number of minutes until the cookie expires after authentication</param>
/// <param name="AccessTokenCookie">Name of the access token cookie</param>
/// <param name="RefreshTokenCookie">Name of the refresh token cookie</param>
/// <param name="TokenRefreshMinutes">The number of minutes until expiration an access token must have for it to be refreshed</param>
/// <param name="GroupsClaim">Name of the groups claim</param>
/// <param name="JwtAuthority">JWT authority</param>
/// <param name="JwtAudience">JWT Audience</param>
/// <param name="AllowCors">True to allows CORS, otherwise false</param>
public sealed record AuthConfiguration(
    string AdminGroup,
    string ViewerGroup,
    int CookieExpirationMinutes,
    string AccessTokenCookie,
    string RefreshTokenCookie,
    int TokenRefreshMinutes,
    string GroupsClaim,
    string JwtAuthority,
    string JwtAudience,
    bool AllowCors
);