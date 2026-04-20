using System.ComponentModel.DataAnnotations;

namespace LMS.DTOs.Enrollments;

public record EnrollRequestDto(
    [Required] Guid CourseId,
    string? VoucherCode,
    string? PaymentIntentId
);
