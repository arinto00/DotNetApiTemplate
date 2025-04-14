namespace ApiTemplate.Application.Common.Interfaces;

/// <summary>
/// Service to get information about the current user
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the current user ID
    /// </summary>
    string? UserId { get; }
    
    /// <summary>
    /// Gets whether the current request is from an authenticated user
    /// </summary>
    bool IsAuthenticated { get; }
}
