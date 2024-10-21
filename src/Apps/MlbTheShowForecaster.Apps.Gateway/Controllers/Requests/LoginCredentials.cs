namespace com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Controllers.Requests;

/// <summary>
/// Login request with a username and password
/// </summary>
/// <param name="Username">The username</param>
/// <param name="Password">The password</param>
public sealed record LoginCredentials(string Username, string Password);