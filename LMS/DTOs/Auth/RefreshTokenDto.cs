using System.ComponentModel.DataAnnotations;

namespace LMS.Api.DTOs.Auth;

public record RefreshTokenDto([Required] string RefreshToken);
