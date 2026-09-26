using Microsoft.EntityFrameworkCore;
using OMS.Application.Dtos;
using OMS.Application.Interfaces;
using OMS.Infrastructure.Persistence;

namespace OMS.Infrastructure.Services;

/// <summary>
/// Moves every employee out of one department into another and deletes the source
/// department. EmployeeService/DepartmentService each rely on the implicit transaction
/// a single SaveChangesAsync call opens; this operation spans two separate
/// SaveChangesAsync-worthy steps (re-pointing employees, then deleting the source
/// department) that must succeed or fail together, so it opens its own explicit
/// database transaction around both.
/// </summary>
public class DepartmentMergeService(AppDbContext db) : IDepartmentMergeService
{
    public async Task MergeAsync(MergeDepartmentsFormModel model, CancellationToken ct = default)
    {
        if (model.SourceDepartmentId is null || model.TargetDepartmentId is null)
        {
            throw new InvalidOperationException("Both a source and target department are required.");
        }

        if (model.SourceDepartmentId == model.TargetDepartmentId)
        {
            throw new InvalidOperationException("Source and target department must be different.");
        }

        // BeginTransactionAsync groups the employee re-point and the department delete
        // below into one atomic unit. Without it, each SaveChangesAsync would commit on
        // its own - a failure after the first one would leave employees moved but the
        // source department still sitting there.
        await using var transaction = await db.Database.BeginTransactionAsync(ct);

        try
        {
            var source = await db.Departments.FindAsync([model.SourceDepartmentId.Value], ct)
                ?? throw new InvalidOperationException("Source department not found.");
            var target = await db.Departments.FindAsync([model.TargetDepartmentId.Value], ct)
                ?? throw new InvalidOperationException("Target department not found.");

            var employees = await db.Employees
                .Where(e => e.Department == source.Name)
                .ToListAsync(ct);

            foreach (var employee in employees)
            {
                employee.Department = target.Name;
                employee.UpdatedAt = DateTime.UtcNow;
            }

            await db.SaveChangesAsync(ct);

            db.Departments.Remove(source);
            await db.SaveChangesAsync(ct);

            await transaction.CommitAsync(ct);
        }
        catch
        {
            // Disposing an uncommitted IDbContextTransaction would roll back on its own,
            // but rolling back explicitly here makes the intent clear.
            await transaction.RollbackAsync(ct);

            // AppDbContext is registered Scoped, and in this Blazor Server app that scope
            // lives for the whole SignalR circuit, not just this one request - so without
            // clearing the tracker, the Employee entities this method modified above would
            // stay marked Modified in memory even though the DB rolled back, and the next
            // unrelated SaveChangesAsync on this circuit would silently re-apply them.
            db.ChangeTracker.Clear();
            throw;
        }
    }
}
