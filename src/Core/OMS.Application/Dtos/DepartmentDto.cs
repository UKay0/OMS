using System.ComponentModel.DataAnnotations;

namespace OMS.Application.Dtos;

/// <summary>Read-only shape returned to the UI for listing/displaying a Department.</summary>
public class DepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

/// <summary>Bound by the dedicated DepartmentEdit page's EditForm.</summary>
public class DepartmentFormModel
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Code { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
