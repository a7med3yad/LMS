using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.Auth;

public record ResendOtpDto([Required][EmailAddress] string Email);