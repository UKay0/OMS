using Microsoft.EntityFrameworkCore;
using OMS.Application.Dtos;
using OMS.Application.Interfaces;
using OMS.Infrastructure.Persistence;
using OMS.Infrastructure.Security;

namespace OMS.Infrastructure.Services;

/// <summary>EF Core-backed implementation of IAuthService. Looks the username up in Users and verifies the PBKDF2 hash; never compares plain text.</summary>
public class AuthService(AppDbContext db) : IAuthService
{
    public async Task<UserDto?> ValidateCredentialsAsync(string username, string password, CancellationToken ct = default)
    {
        var user = await db.Users
            .SingleOrDefaultAsync(u => u.Username == username && u.IsActive, ct);

        if (user is null || !PasswordHasher.Verify(password, user.PasswordHash))
        {
            return null;
        }

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            DisplayName = user.DisplayName,
            Role = user.Role.ToString(),
            IsActive = user.IsActive
        };
    }
}
