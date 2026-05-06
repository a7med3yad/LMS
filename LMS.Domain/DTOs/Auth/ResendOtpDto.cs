using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.DTOs.Auth;

public record ResendOtpDto([Required][EmailAddress] string Email);