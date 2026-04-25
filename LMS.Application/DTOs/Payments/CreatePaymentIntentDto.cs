using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.Payments;

public record CreatePaymentIntentDto(
    [Required] Guid CourseId,
    string? VoucherCode
);
