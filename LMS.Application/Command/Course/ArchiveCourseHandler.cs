using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Domain.Models.Enums;
using MediatR;

namespace LMS.Application.Command.Course;

public class ArchiveCourseHandler : IRequestHandler<ArchiveCourseCommand>
{
    private readonly IUnitOfWork _uow;
    public ArchiveCourseHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(ArchiveCourseCommand request, CancellationToken ct)
    {
        var course = await _uow.Courses.GetByIdAsync(request.CourseId, ct)
            ?? throw new NotFoundException("Course", request.CourseId);

        course.Status = CourseStatus.Archived;
        course.UpdatedAt = DateTime.UtcNow;
        _uow.Courses.Update(course);
        await _uow.SaveChangesAsync(ct);
    }
}
