using OMS.Application.Dtos;

namespace OMS.Application.Interfaces;

/// <summary>
/// Validates login credentials against the seeded Users table and issues the claims
/// the Login page uses to sign the user in. Implemented in OMS.Infrastructure/Services/AuthService.cs.
/// </summary>
public interface IAuthService
{
    /// <summary>Returns the matching user's DTO, or null if the username/password pair doesn't match an active user.</summary>
    Task<UserDto?> ValidateCredentialsAsync(string username, string password, CancellationToken ct = default);
}
