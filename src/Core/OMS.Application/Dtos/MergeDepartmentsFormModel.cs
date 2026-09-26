using System.ComponentModel.DataAnnotations;

namespace OMS.Application.Dtos;

/// <summary>Bound by DepartmentMerge.razor's EditForm.</summary>
public class MergeDepartmentsFormModel
{
    [Required(ErrorMessage = "Choose a source department.")]
    public int? SourceDepartmentId { get; set; }

    [Required(ErrorMessage = "Choose a target department.")]
    public int? TargetDepartmentId { get; set; }
}
