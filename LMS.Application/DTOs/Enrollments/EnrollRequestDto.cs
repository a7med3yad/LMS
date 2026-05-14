using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.Enrollments;

public record EnrollRequestDto(
    [Required] Guid CourseId,
    string? VoucherCode,
    string? PaymentIntentId
);
