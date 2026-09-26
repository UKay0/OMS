using System.ComponentModel.DataAnnotations;

namespace OMS.Application.Dtos;

/// <summary>Read-only shape returned to the UI for listing/displaying an Employee.</summary>
public class EmployeeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

/// <summary>
/// Bound by the Blazor EditForm (both the modal and, if copied, a page). DataAnnotations
/// drive client-side validation via DataAnnotationsValidator + ValidationSummary.
/// </summary>
public class EmployeeFormModel
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Department { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Role { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
