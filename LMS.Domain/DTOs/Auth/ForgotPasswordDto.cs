using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.Api.DTOs.Auth;

public record ForgotPasswordDto([Required][EmailAddress] string Email);
