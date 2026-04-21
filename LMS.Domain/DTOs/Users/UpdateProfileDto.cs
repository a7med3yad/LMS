using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.DTOs.Users;

public record UpdateProfileDto(
    [Required][MaxLength(200)] string FullName
);
