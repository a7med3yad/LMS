using System.ComponentModel.DataAnnotations;

namespace LMS.DTOs.Payments;

public record ConfirmPaymentDto(
    [Required] string PaymentIntentId
);
