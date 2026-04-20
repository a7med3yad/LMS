namespace LMS.DTOs.Vouchers;

public record UpdateVoucherDto(
    decimal? DiscountPercent,
    decimal? DiscountAmount,
    int? MaxUses,
    DateTime? ExpiresAt,
    bool? IsActive
);
