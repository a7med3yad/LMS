using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.Payments;

public record CreatePaymentDto(
    [Required] Guid CourseId,
    [Required] string Provider
);
