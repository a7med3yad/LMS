using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.Users;

public record UpdateAvatarDto(
    [Required] string AvatarUrl
);
