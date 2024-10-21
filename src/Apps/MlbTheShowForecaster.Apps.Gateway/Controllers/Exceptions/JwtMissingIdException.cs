namespace com.brettnamba.MlbTheShowForecaster.Apps.Gateway.Controllers.Exceptions;

/// <summary>
/// Thrown when a JWT token is missing required information or claims
/// </summary>
public sealed class JwtMissingIdException : Exception
{
    public JwtMissingIdException(string? username, string? role)
    {
        Message = $"JWT token was missing username: {username}, role: {role}";
    }

    public override string Message { get; }
}