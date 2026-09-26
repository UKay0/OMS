using OMS.Application.Dtos;

namespace OMS.Application.Interfaces;

/// <summary>
/// Application-layer contract for Department CRUD. Implemented in
/// OMS.Infrastructure/Services/DepartmentService.cs, consumed by the Departments pages in OMS.Web.
/// </summary>
public interface IDepartmentService : ICrudService<DepartmentDto, DepartmentFormModel, int>
{
}
