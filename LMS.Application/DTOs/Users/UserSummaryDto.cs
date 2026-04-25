namespace LMS.Application.DTOs.Users;

public record UserSummaryDto(
    string Id,
    string FullName,
    string Email,
    string? AvatarUrl,
    string Role
);
