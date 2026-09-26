using OMS.Domain.Common;

namespace OMS.Domain.Entities;

/// <summary>
/// Sample feature entity #1. Demonstrates the "edit via modal" CRUD pattern
/// (see Web/Components/Pages/Employees/EmployeeList.razor).
/// </summary>
public class Employee : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
