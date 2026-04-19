using System.ComponentModel.DataAnnotations;

namespace LMS.Api.DTOs.Auth;

public record VerifyOtpDto(
    [Required][EmailAddress] string Email,
    [Required][Length(6, 6)] string Otp
);
