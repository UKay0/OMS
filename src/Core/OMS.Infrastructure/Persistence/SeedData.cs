using Microsoft.EntityFrameworkCore;
using OMS.Domain.Entities;
using OMS.Domain.Enums;
using OMS.Infrastructure.Security;

namespace OMS.Infrastructure.Persistence;

/// <summary>
/// Runs once at startup (from Program.cs, after Database.Migrate()) to guarantee the
/// app is usable on a brand-new database: an Admin login exists, and the two sample
/// CRUD features have a few rows so the list pages aren't empty on first run.
/// This is what satisfies "hardcoded credentials" without actually hardcoding a
/// bypass in the login code path itself.
/// </summary>
public static class SeedData
{
    public const string AdminUsername = "admin";
    public const string AdminPassword = "Admin@123";

    public static async Task SeedAsync(AppDbContext db, CancellationToken ct = default)
    {
        if (!await db.Users.AnyAsync(ct))
        {
            db.Users.Add(new User
            {
                Username = AdminUsername,
                PasswordHash = PasswordHasher.Hash(AdminPassword),
                DisplayName = "System Administrator",
                Role = UserRole.Admin,
                IsActive = true
            });

            db.Users.Add(new User
            {
                Username = "user",
                PasswordHash = PasswordHasher.Hash("User@123"),
                DisplayName = "Sample User",
                Role = UserRole.User,
                IsActive = true
            });
        }

        if (!await db.Departments.AnyAsync(ct))
        {
            db.Departments.AddRange(
                new Department { Name = "Engineering", Code = "ENG", IsActive = true },
                new Department { Name = "Human Resources", Code = "HR", IsActive = true },
                new Department { Name = "Sales", Code = "SALES", IsActive = true });
        }

        if (!await db.Employees.AnyAsync(ct))
        {
            db.Employees.AddRange(
                new Employee { Name = "Alice Johnson", Email = "alice.johnson@oms.local", Department = "Engineering", Role = "Software Engineer", IsActive = true },
                new Employee { Name = "Bob Smith", Email = "bob.smith@oms.local", Department = "Sales", Role = "Account Executive", IsActive = true });
        }

        await db.SaveChangesAsync(ct);
    }
}
