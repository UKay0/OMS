using OMS.Domain.Common;

namespace OMS.Domain.Entities;

/// <summary>
/// Sample feature entity #2. Demonstrates the "edit via dedicated page" CRUD pattern
/// (see Web/Components/Pages/Departments/DepartmentEdit.razor), independent of the
/// modal pattern used by Employee so both templates exist side by side.
/// </summary>
public class Department : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
