using OMS.Application.Dtos;

namespace OMS.Application.Interfaces;

/// <summary>
/// Merges one department into another: every employee in the source department is
/// moved to the target department, then the source department is deleted. Both steps
/// succeed or fail together. Implemented in
/// OMS.Infrastructure/Services/DepartmentMergeService.cs, consumed by
/// OMS.Web/Components/Pages/Departments/DepartmentMerge.razor.
/// </summary>
public interface IDepartmentMergeService
{
    Task MergeAsync(MergeDepartmentsFormModel model, CancellationToken ct = default);
}
