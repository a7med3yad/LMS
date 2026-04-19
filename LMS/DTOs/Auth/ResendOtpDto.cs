using System.ComponentModel.DataAnnotations;

namespace LMS.Api.DTOs.Auth;

public record ResendOtpDto([Required][EmailAddress] string Email);