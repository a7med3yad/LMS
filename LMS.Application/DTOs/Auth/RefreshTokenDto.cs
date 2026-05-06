using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.Auth;

public record RefreshTokenDto([Required] string RefreshToken);
