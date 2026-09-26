using Microsoft.EntityFrameworkCore;
using OMS.Application.Dtos;
using OMS.Application.Interfaces;
using OMS.Domain.Entities;
using OMS.Infrastructure.Persistence;

namespace OMS.Infrastructure.Services;

/// <summary>EF Core-backed implementation of IDepartmentService. Reference implementation for the "dedicated page edit" CRUD pattern's data access.</summary>
public class DepartmentService(AppDbContext db) : IDepartmentService
{
    public async Task<IReadOnlyList<DepartmentDto>> GetAllAsync(CancellationToken ct = default) =>
        await db.Departments
            .OrderBy(d => d.Name)
            .Select(d => ToDto(d))
            .ToListAsync(ct);

    public async Task<DepartmentDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.Departments.FindAsync([id], ct);
        return entity is null ? null : ToDto(entity);
    }

    public async Task<int> CreateAsync(DepartmentFormModel model, CancellationToken ct = default)
    {
        var entity = new Department
        {
            Name = model.Name,
            Code = model.Code,
            IsActive = model.IsActive
        };

        db.Departments.Add(entity);
        await db.SaveChangesAsync(ct);
        return entity.Id;
    }

    public async Task UpdateAsync(int id, DepartmentFormModel model, CancellationToken ct = default)
    {
        var entity = await db.Departments.FindAsync([id], ct)
            ?? throw new InvalidOperationException($"Department {id} not found.");

        entity.Name = model.Name;
        entity.Code = model.Code;
        entity.IsActive = model.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.Departments.FindAsync([id], ct);
        if (entity is null)
        {
            return;
        }

        db.Departments.Remove(entity);
        await db.SaveChangesAsync(ct);
    }

    private static DepartmentDto ToDto(Department d) => new()
    {
        Id = d.Id,
        Name = d.Name,
        Code = d.Code,
        IsActive = d.IsActive
    };
}
