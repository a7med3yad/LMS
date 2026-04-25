using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.Auth;

public record VerifyOtpDto(
    [Required][EmailAddress] string Email,
    [Required][Length(6, 6)] string Otp
);
