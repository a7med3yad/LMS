using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Domain.Models.Enums;
using MediatR;

namespace LMS.Application.Command.Course;

public class PublishCourseHandler : IRequestHandler<PublishCourseCommand>
{
    private readonly IUnitOfWork _uow;
    public PublishCourseHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(PublishCourseCommand request, CancellationToken ct)
    {
        var course = await _uow.Courses.GetByIdAsync(request.CourseId, ct)
            ?? throw new NotFoundException("Course", request.CourseId);

        if (course.InstructorId != request.RequesterId) throw new ForbiddenException();

        course.Status = CourseStatus.Published;
        course.UpdatedAt = DateTime.UtcNow;
        _uow.Courses.Update(course);
        await _uow.SaveChangesAsync(ct);
    }
}
