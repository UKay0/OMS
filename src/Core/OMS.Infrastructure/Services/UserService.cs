using Microsoft.EntityFrameworkCore;
using OMS.Application.Dtos;
using OMS.Application.Interfaces;
using OMS.Infrastructure.Persistence;

namespace OMS.Infrastructure.Services;

/// <summary>EF Core-backed implementation of IUserService, used by the Admin-only User Management page.</summary>
public class UserService(AppDbContext db) : IUserService
{
    public async Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken ct = default) =>
        await db.Users
            .OrderBy(u => u.Username)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                DisplayName = u.DisplayName,
                Role = u.Role.ToString(),
                IsActive = u.IsActive
            })
            .ToListAsync(ct);
}
