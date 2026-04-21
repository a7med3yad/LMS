using LMS.Application.Services.Interfaces;
using LMS.Domain.DTOs.Enrollments;
using LMS.Domain.DTOs.Payments;

namespace LMS.Application.Services;

public class PaymentService : IPaymentService
{
    public Task<PaymentIntentResponseDto> CreatePaymentIntentAsync(Guid studentId, CreatePaymentIntentDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<EnrollmentDto> ConfirmPaymentAndEnrollAsync(Guid studentId, ConfirmPaymentDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task HandleStripeWebhookAsync(string payload, string stripeSignature, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<PaymentIntentResponseDto> ApplyVoucherToIntentAsync(Guid studentId, ApplyVoucherDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<decimal> GetCourseRevenueAsync(Guid courseId, Guid instructorId, CancellationToken ct = default)
        => throw new NotImplementedException();
}
