using OMS.Application.Dtos;

namespace OMS.Application.Interfaces;

/// <summary>
/// Read access to the Users table for the Admin-only "User Management" page.
/// Kept separate from IAuthService (login) since listing users and validating a
/// login are different concerns. Implemented in OMS.Infrastructure/Services/UserService.cs.
/// </summary>
public interface IUserService
{
    Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken ct = default);
}
