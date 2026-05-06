using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.DTOs.Auth;

public record RefreshTokenDto([Required] string RefreshToken);
