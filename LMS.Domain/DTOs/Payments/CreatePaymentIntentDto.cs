using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.DTOs.Payments;

public record CreatePaymentIntentDto(
    [Required] Guid CourseId,
    string? VoucherCode
);
