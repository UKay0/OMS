using OMS.Application.Dtos;

namespace OMS.Application.Interfaces;

/// <summary>
/// Reference implementation of a multi-step write wrapped in an explicit database
/// transaction: moving every employee out of one department and deleting it either
/// both happen or neither does. Implemented in
/// OMS.Infrastructure/Services/DepartmentMergeService.cs, consumed by
/// OMS.Web/Components/Pages/Departments/DepartmentMerge.razor.
/// </summary>
public interface IDepartmentMergeService
{
    Task MergeAsync(MergeDepartmentsFormModel model, CancellationToken ct = default);
}
