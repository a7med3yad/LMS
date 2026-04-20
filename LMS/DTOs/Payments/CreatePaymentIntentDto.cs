using System.ComponentModel.DataAnnotations;

namespace LMS.DTOs.Payments;

public record CreatePaymentIntentDto(
    [Required] Guid CourseId,
    string? VoucherCode
);
