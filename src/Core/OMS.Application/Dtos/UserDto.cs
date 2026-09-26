namespace OMS.Application.Dtos;

/// <summary>Read-only shape for the Admin-only "User Management" page. Never carries the password hash.</summary>
public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
