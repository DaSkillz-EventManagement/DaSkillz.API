﻿namespace Domain.DTOs.User.Response;

public class UserUpdatedResponseDto
{
    public Guid UserId { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Avatar { get; set; }
    public bool? IsPremiumUser { get; set; } = false;
}
