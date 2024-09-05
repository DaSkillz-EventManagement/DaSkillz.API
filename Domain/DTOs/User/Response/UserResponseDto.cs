namespace Domain.DTOs.User.Response;

public class UserResponseDto
{
    public Guid UserId { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string Status { get; set; } = null!;
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? Avatar { get; set; }
}
