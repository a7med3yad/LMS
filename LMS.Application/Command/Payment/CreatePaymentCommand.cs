using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Payments;
using LMS.Domain.Models;
using LMS.Domain.Models.Enums;
using MediatR;

namespace LMS.Application.Command.Payment;

public record CreatePaymentCommand(Guid StudentId, CreatePaymentDto Dto)
    : IRequest<PaymentDto>;

public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, PaymentDto>
{
    private readonly IUnitOfWork _uow;

    public CreatePaymentHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<PaymentDto> Handle(CreatePaymentCommand req, CancellationToken ct)
    {
        var course = await _uow.Courses.GetByIdAsync(req.Dto.CourseId, ct)
            ?? throw new NotFoundException("Course", req.Dto.CourseId);

        if (await _uow.Enrollments.IsEnrolledAsync(req.StudentId, course.Id, ct))
            throw new ConflictException("Already enrolled in this course.");

        var enrollment = new Domain.Models.Enrollment
        {
            Id         = Guid.NewGuid(),
            StudentId  = req.StudentId,
            CourseId   = course.Id,
            Status     = EnrollmentStatus.PendingPayment,
            PaidAmount = 0
        };

        await _uow.Enrollments.AddAsync(enrollment, ct);

        var payment = new Domain.Models.Payment
        {
            Id           = Guid.NewGuid(),
            StudentId    = req.StudentId,
            CourseId     = course.Id,
            EnrollmentId = enrollment.Id,
            Amount       = course.Price,
            Status       = PaymentStatus.Pending,
            Provider     = req.Dto.Provider,
            CreatedAt    = DateTime.UtcNow
        };

        await _uow.Payments.AddAsync(payment, ct);
        await _uow.SaveChangesAsync(ct);

        return new PaymentDto(
            payment.Id, payment.StudentId, payment.CourseId,
            payment.Amount, payment.Status, payment.Provider,
            payment.ProviderTransactionId,
            payment.CreatedAt, payment.PaidAt
        );
    }
}
