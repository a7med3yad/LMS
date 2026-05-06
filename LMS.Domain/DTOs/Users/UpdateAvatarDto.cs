using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.DTOs.Users;

public record UpdateAvatarDto(
    [Required] string AvatarUrl
);
