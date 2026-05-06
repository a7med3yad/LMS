using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.DTOs.Enrollments;

public record EnrollRequestDto(
    [Required] Guid CourseId,
    string? VoucherCode,
    string? PaymentIntentId
);
