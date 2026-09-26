using Microsoft.EntityFrameworkCore;
using OMS.Application.Dtos;
using OMS.Application.Interfaces;
using OMS.Infrastructure.Persistence;

namespace OMS.Infrastructure.Services;

/// <summary>
/// Reference implementation for wrapping several writes in one explicit database
/// transaction. Every other service in this template (EmployeeService,
/// DepartmentService, ...) relies on the implicit transaction SaveChangesAsync opens
/// for its single call; this one needs an explicit transaction because it spans two
/// SaveChangesAsync-worthy steps (re-pointing employees, then deleting the source
/// department) that must succeed or fail together.
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

            if (model.SimulateFailure)
            {
                // Thrown on purpose (wired to a checkbox in the UI) so the rollback below
                // actually runs: proves the employee moves just saved above get undone
                // along with the delete, instead of the merge landing half-applied.
                throw new InvalidOperationException(
                    "Simulated failure after moving employees but before deleting the source department.");
            }

            db.Departments.Remove(source);
            await db.SaveChangesAsync(ct);

            await transaction.CommitAsync(ct);
        }
        catch
        {
            // Disposing an uncommitted IDbContextTransaction would roll back on its own,
            // but doing it explicitly documents the intent for anyone reading this as the
            // template for their own multi-step transaction.
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
