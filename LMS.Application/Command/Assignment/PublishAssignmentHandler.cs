using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using MediatR;

namespace LMS.Application.Command.Assignment;

public class PublishAssignmentHandler : IRequestHandler<PublishAssignmentCommand>
{
    private readonly IUnitOfWork _uow;
    public PublishAssignmentHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(PublishAssignmentCommand req, CancellationToken ct)
    {
        var a = await _uow.Assignments.GetByIdAsync(req.AssignmentId, ct)
            ?? throw new NotFoundException("Assignment", req.AssignmentId);

        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, a.CourseId, ct))
            throw new ForbiddenException();

        a.IsPublished = true;
        _uow.Assignments.Update(a);
        await _uow.SaveChangesAsync(ct);
    }
}
