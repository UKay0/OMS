using System.ComponentModel.DataAnnotations;

namespace OMS.Application.Dtos;

/// <summary>Bound by DepartmentMerge.razor's EditForm.</summary>
public class MergeDepartmentsFormModel
{
    [Required(ErrorMessage = "Choose a source department.")]
    public int? SourceDepartmentId { get; set; }

    [Required(ErrorMessage = "Choose a target department.")]
    public int? TargetDepartmentId { get; set; }

    /// <summary>
    /// When true, the service throws after moving employees but before deleting the
    /// source department - purely so the UI can demonstrate that the transaction rolls
    /// the employee moves back too, instead of leaving a half-applied merge.
    /// </summary>
    public bool SimulateFailure { get; set; }
}
