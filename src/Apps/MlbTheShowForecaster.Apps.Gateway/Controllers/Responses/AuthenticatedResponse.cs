namespace com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Controllers.Responses;

/// <summary>
/// Represents a successful authentication response
/// </summary>
/// <param name="Message">The message</param>
/// <param name="Username">The authenticated user</param>
/// <param name="Role">The user's role</param>
public sealed record AuthenticatedResponse(string Message, string Username, string Role);