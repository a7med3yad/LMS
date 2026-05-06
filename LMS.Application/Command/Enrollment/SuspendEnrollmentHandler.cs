using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Domain.Models.Enums;
using MediatR;

namespace LMS.Application.Command.Enrollment;

public class SuspendEnrollmentHandler : IRequestHandler<SuspendEnrollmentCommand>
{
    private readonly IUnitOfWork _uow;
    public SuspendEnrollmentHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(SuspendEnrollmentCommand req, CancellationToken ct)
    {
        var e = await _uow.Enrollments.GetByIdAsync(req.EnrollmentId, ct)
            ?? throw new NotFoundException("Enrollment", req.EnrollmentId);

        e.Status = EnrollmentStatus.Suspended;
        _uow.Enrollments.Update(e);
        await _uow.SaveChangesAsync(ct);
    }
}