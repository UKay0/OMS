namespace OMS.Application.Interfaces;

/// <summary>
/// Common shape for a simple list/add/edit/delete feature service. New feature
/// interfaces (e.g. IEmployeeService) can extend this to pick up the standard CRUD
/// signature for free; it is a convention, not a requirement — a feature with more
/// exotic operations can just declare extra methods on its own interface.
/// </summary>
public interface ICrudService<TDto, TFormModel, TId>
{
    Task<IReadOnlyList<TDto>> GetAllAsync(CancellationToken ct = default);

    Task<TDto?> GetByIdAsync(TId id, CancellationToken ct = default);

    Task<TId> CreateAsync(TFormModel model, CancellationToken ct = default);

    Task UpdateAsync(TId id, TFormModel model, CancellationToken ct = default);

    Task DeleteAsync(TId id, CancellationToken ct = default);
}
