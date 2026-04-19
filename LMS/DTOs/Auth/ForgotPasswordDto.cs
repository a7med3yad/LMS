using System.ComponentModel.DataAnnotations;

namespace LMS.Api.DTOs.Auth;

public record ForgotPasswordDto([Required][EmailAddress] string Email);
