namespace OMS.Domain.Enums;

/// <summary>
/// Roles recognized by authorization. Add new values here as the app grows;
/// gate a page for a role with [Authorize(Roles = nameof(UserRole.Admin))] and
/// filter NavMenu links the same way.
/// </summary>
public enum UserRole
{
    Admin,
    User
}
