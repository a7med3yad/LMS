using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.Vouchers;

public record CreateVoucherDto(
    [Required][MaxLength(50)] string Code,
    [Required] Guid CourseId,
    [Range(0, 100)] decimal DiscountPercent,
    decimal? DiscountAmount,
    [Range(1, 10000)] int MaxUses,
    [Required] DateTime ExpiresAt
);
