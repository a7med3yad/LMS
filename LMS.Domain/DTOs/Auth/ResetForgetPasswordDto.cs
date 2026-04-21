using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.DTOs.Auth;

public record ResetForgetPasswordDto([Required][EmailAddress] string Email, [Required][MinLength(8)] string NewPassword);