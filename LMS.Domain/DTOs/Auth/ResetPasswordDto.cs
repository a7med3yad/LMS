using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.DTOs.Auth;

public record ResetPasswordDto(
    [Required][EmailAddress] string Email,
    [Required][Length(6, 6)] string Otp,
    [Required][MinLength(8)] string NewPassword
);