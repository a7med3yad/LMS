using LMS.Domain.Models.Enums;

namespace LMS.Application.DTOs.Payments;

public record PaymentDto(
    Guid Id,
    Guid StudentId,
    Guid CourseId,
    decimal Amount,
    PaymentStatus Status,
    string Provider,
    string? ProviderTransactionId,
    DateTime CreatedAt,
    DateTime? PaidAt
);
