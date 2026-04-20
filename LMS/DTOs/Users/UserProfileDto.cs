namespace LMS.DTOs.Users;

public record UserProfileDto(
    string Id,
    string FullName,
    string Email,
    string? AvatarUrl,
    string Role,
    bool IsActive,
    bool IsVerified,
    DateTime CreatedAt
);
