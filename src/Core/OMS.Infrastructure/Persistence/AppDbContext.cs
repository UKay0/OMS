using Microsoft.EntityFrameworkCore;
using OMS.Domain.Entities;

namespace OMS.Infrastructure.Persistence;

/// <summary>
/// The single EF Core context for OMS. Each feature adds one DbSet here; row shape
/// and constraints for that entity live in its own IEntityTypeConfiguration under
/// Configurations/, auto-discovered by ApplyConfigurationsFromAssembly below.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<Department> Departments => Set<Department>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
