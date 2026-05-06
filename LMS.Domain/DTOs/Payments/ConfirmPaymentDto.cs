using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.DTOs.Payments;

public record ConfirmPaymentDto(
    [Required] string PaymentIntentId
);
