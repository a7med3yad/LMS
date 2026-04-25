using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.Auth;

public record ChangePasswordDto([Required][MinLength(8)] string CurrentPassword,[Required][MinLength(8)] string NewPassword);
