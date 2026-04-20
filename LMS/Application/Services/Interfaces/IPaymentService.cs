using LMS.DTOs.Enrollments;
using LMS.DTOs.Payments;

namespace LMS.Application.Services.Interfaces;

public interface IPaymentService
{
    Task<PaymentIntentResponseDto> CreatePaymentIntentAsync(Guid studentId, CreatePaymentIntentDto dto, CancellationToken ct = default);

    Task<EnrollmentDto> ConfirmPaymentAndEnrollAsync(Guid studentId, ConfirmPaymentDto dto, CancellationToken ct = default);

    Task HandleStripeWebhookAsync(string payload, string stripeSignature, CancellationToken ct = default);

    Task<PaymentIntentResponseDto> ApplyVoucherToIntentAsync(Guid studentId, ApplyVoucherDto dto, CancellationToken ct = default);

    Task<decimal> GetCourseRevenueAsync(Guid courseId, Guid instructorId, CancellationToken ct = default);
}
