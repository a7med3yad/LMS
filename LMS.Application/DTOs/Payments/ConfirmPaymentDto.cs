using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.Payments;

public record ConfirmPaymentDto(
    [Required] string PaymentIntentId
);
