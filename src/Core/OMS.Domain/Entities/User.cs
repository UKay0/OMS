using OMS.Domain.Common;
using OMS.Domain.Enums;

namespace OMS.Domain.Entities;

/// <summary>
/// An account that can sign in to OMS. Credentials are validated against this table
/// (not a hardcoded literal) so the "hardcoded admin" requirement is satisfied via
/// seeded data instead of an in-code shortcut — see Infrastructure/Persistence/SeedData.cs.
/// </summary>
public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;

    /// <summary>PBKDF2 hash produced by Infrastructure/Security/PasswordHasher. Never store plain text.</summary>
    public string PasswordHash { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.User;

    public bool IsActive { get; set; } = true;
}
