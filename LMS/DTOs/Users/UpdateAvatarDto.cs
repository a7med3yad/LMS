using System.ComponentModel.DataAnnotations;

namespace LMS.DTOs.Users;

public record UpdateAvatarDto(
    [Required] string AvatarUrl
);
