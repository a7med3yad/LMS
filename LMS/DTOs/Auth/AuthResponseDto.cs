namespace LMS.Api.DTOs.Auth;

public record AuthResponseDto(
    string AccessToken,
    string RefreshToken,
    string UserId,
    string FullName,
    string Email,
    string Role
);