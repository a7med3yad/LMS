namespace LMS.DTOs.Payments;

public record PaymentIntentResponseDto(
    string PaymentIntentId,
    string ClientSecret,
    decimal Amount,
    decimal OriginalPrice,
    decimal? DiscountAmount,
    string? VoucherCode,
    string Currency
);
