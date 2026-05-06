using System.ComponentModel.DataAnnotations;

namespace LMS.Application.Api.DTOs.Auth;

public record ForgotPasswordDto([Required][EmailAddress] string Email);
