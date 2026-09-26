using Microsoft.EntityFrameworkCore;
using OMS.Application.Dtos;
using OMS.Application.Interfaces;
using OMS.Domain.Entities;
using OMS.Infrastructure.Persistence;

namespace OMS.Infrastructure.Services;

/// <summary>EF Core-backed implementation of IEmployeeService.</summary>
public class EmployeeService(AppDbContext db) : IEmployeeService
{
    public async Task<IReadOnlyList<EmployeeDto>> GetAllAsync(CancellationToken ct = default) =>
        await db.Employees
            .OrderBy(e => e.Name)
            .Select(e => ToDto(e))
            .ToListAsync(ct);

    public async Task<EmployeeDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.Employees.FindAsync([id], ct);
        return entity is null ? null : ToDto(entity);
    }

    public async Task<int> CreateAsync(EmployeeFormModel model, CancellationToken ct = default)
    {
        var entity = new Employee
        {
            Name = model.Name,
            Email = model.Email,
            Department = model.Department,
            Role = model.Role,
            IsActive = model.IsActive
        };

        db.Employees.Add(entity);
        await db.SaveChangesAsync(ct);
        return entity.Id;
    }

    public async Task UpdateAsync(int id, EmployeeFormModel model, CancellationToken ct = default)
    {
        var entity = await db.Employees.FindAsync([id], ct)
            ?? throw new InvalidOperationException($"Employee {id} not found.");

        entity.Name = model.Name;
        entity.Email = model.Email;
        entity.Department = model.Department;
        entity.Role = model.Role;
        entity.IsActive = model.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.Employees.FindAsync([id], ct);
        if (entity is null)
        {
            return;
        }

        db.Employees.Remove(entity);
        await db.SaveChangesAsync(ct);
    }

    private static EmployeeDto ToDto(Employee e) => new()
    {
        Id = e.Id,
        Name = e.Name,
        Email = e.Email,
        Department = e.Department,
        Role = e.Role,
        IsActive = e.IsActive
    };
}
