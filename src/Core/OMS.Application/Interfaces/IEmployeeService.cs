using OMS.Application.Dtos;

namespace OMS.Application.Interfaces;

/// <summary>
/// Application-layer contract for Employee CRUD. Implemented in
/// OMS.Infrastructure/Services/EmployeeService.cs, consumed by the Employees pages in OMS.Web.
/// </summary>
public interface IEmployeeService : ICrudService<EmployeeDto, EmployeeFormModel, int>
{
}
