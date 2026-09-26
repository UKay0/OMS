namespace OMS.Domain.Common;

/// <summary>
/// Shared audit fields for every persisted entity. Inherit this when adding a new
/// entity so new features automatically get Id/CreatedAt/UpdatedAt without repeating them.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
