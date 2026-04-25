using LMS.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.Auth;

public record RegisterDto(
    [Required] string FullName,
    [Required][EmailAddress] string Email,
    [Required][MinLength(8)] string Password,
    [Required] UserRole Role
);