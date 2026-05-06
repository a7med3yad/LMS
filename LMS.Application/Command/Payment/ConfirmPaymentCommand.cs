using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Domain.Models.Enums;
using MediatR;

namespace LMS.Application.Command.Payment;

public record ConfirmPaymentCommand(Guid PaymentId, string ProviderTransactionId)
    : IRequest;

public class ConfirmPaymentHandler : IRequestHandler<ConfirmPaymentCommand>
{
    private readonly IUnitOfWork _uow;

    public ConfirmPaymentHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(ConfirmPaymentCommand req, CancellationToken ct)
    {
        var payment = await _uow.Payments.GetByIdAsync(req.PaymentId, ct)
            ?? throw new NotFoundException("Payment", req.PaymentId);

        if (payment.Status == PaymentStatus.Success) return;

        var enrollment = await _uow.Enrollments.GetByIdAsync(payment.EnrollmentId, ct)
            ?? throw new NotFoundException("Enrollment", payment.EnrollmentId);

        payment.Status = PaymentStatus.Success;
        payment.ProviderTransactionId = req.ProviderTransactionId;
        payment.PaidAt = DateTime.UtcNow;

        enrollment.Status = EnrollmentStatus.Active;
        enrollment.PaidAmount = payment.Amount;

        _uow.Payments.Update(payment);
        _uow.Enrollments.Update(enrollment);

        await _uow.SaveChangesAsync(ct);
    }
}
