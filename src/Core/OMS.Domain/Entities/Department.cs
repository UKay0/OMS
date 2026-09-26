using OMS.Domain.Common;

namespace OMS.Domain.Entities;

/// <summary>
/// An organizational department that employees belong to. Added/edited via a
/// dedicated page (see Web/Components/Pages/Departments/DepartmentEdit.razor).
/// </summary>
public class Department : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
