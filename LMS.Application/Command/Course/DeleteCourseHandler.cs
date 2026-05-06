using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using MediatR;

namespace LMS.Application.Command.Course;

public class DeleteCourseHandler : IRequestHandler<DeleteCourseCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteCourseHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteCourseCommand request, CancellationToken ct)
    {
        var course = await _uow.Courses.GetByIdAsync(request.CourseId, ct)
            ?? throw new NotFoundException("Course", request.CourseId);

        if (course.InstructorId != request.RequesterId) throw new ForbiddenException();

        _uow.Courses.Remove(course);
        await _uow.SaveChangesAsync(ct);
    }
}