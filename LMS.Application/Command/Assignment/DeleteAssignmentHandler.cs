using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using MediatR;

namespace LMS.Application.Command.Assignment;

public class DeleteAssignmentHandler : IRequestHandler<DeleteAssignmentCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteAssignmentHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteAssignmentCommand req, CancellationToken ct)
    {
        var a = await _uow.Assignments.GetByIdAsync(req.AssignmentId, ct)
            ?? throw new NotFoundException("Assignment", req.AssignmentId);

        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, a.CourseId, ct))
            throw new ForbiddenException();

        _uow.Assignments.Remove(a);
        await _uow.SaveChangesAsync(ct);
    }
}
