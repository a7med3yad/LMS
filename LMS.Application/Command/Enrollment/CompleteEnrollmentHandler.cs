using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Domain.Models.Enums;
using MediatR;

namespace LMS.Application.Command.Enrollment;

public class CompleteEnrollmentHandler : IRequestHandler<CompleteEnrollmentCommand>
{
    private readonly IUnitOfWork _uow;
    public CompleteEnrollmentHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(CompleteEnrollmentCommand req, CancellationToken ct)
    {
        var e = await _uow.Enrollments.GetByIdAsync(req.EnrollmentId, ct)
            ?? throw new NotFoundException("Enrollment", req.EnrollmentId);

        if (e.StudentId != req.StudentId) throw new ForbiddenException();

        e.Status = EnrollmentStatus.Completed;
        e.CompletedAt = DateTime.UtcNow;
        _uow.Enrollments.Update(e);
        await _uow.SaveChangesAsync(ct);
    }
}
