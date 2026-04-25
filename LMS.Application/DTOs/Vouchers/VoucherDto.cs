namespace LMS.Application.DTOs.Vouchers;

public record VoucherDto(
    Guid Id,
    string Code,
    Guid CourseId,
    string CourseTitleEn,
    decimal DiscountPercent,
    decimal? DiscountAmount,
    int MaxUses,
    int UsedCount,
    DateTime ExpiresAt,
    bool IsActive,
    DateTime CreatedAt
);
