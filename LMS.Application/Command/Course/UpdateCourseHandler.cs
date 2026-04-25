using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Courses;
using MediatR;

namespace LMS.Application.Command.Course;

public class UpdateCourseHandler : IRequestHandler<UpdateCourseCommand, CourseDto>
{
    private readonly IUnitOfWork _uow;

    public UpdateCourseHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<CourseDto> Handle(UpdateCourseCommand request, CancellationToken ct)
    {
        var course = await _uow.Courses.GetCourseWithDetailsAsync(request.CourseId, ct)
            ?? throw new NotFoundException("Course", request.CourseId);

        if (course.InstructorId != request.RequesterId)
            throw new ForbiddenException();

        if (request.Dto.TitleAr is not null) course.TitleAr = request.Dto.TitleAr;
        if (request.Dto.TitleEn is not null) course.TitleEn = request.Dto.TitleEn;
        if (request.Dto.DescriptionAr is not null) course.DescriptionAr = request.Dto.DescriptionAr;
        if (request.Dto.DescriptionEn is not null) course.DescriptionEn = request.Dto.DescriptionEn;
        if (request.Dto.ThumbnailUrl is not null) course.ThumbnailUrl = request.Dto.ThumbnailUrl;
        if (request.Dto.Price.HasValue) course.Price = request.Dto.Price.Value;

        course.UpdatedAt = DateTime.UtcNow;
        _uow.Courses.Update(course);
        await _uow.SaveChangesAsync(ct);

        return new CourseDto(
            course.Id, course.TitleAr, course.TitleEn,
            course.DescriptionAr, course.DescriptionEn,
            course.ThumbnailUrl, course.Price, course.Status,
            new LMS.Domain.DTOs.Users.UserSummaryDto(
                course.Instructor.Id.ToString(), course.Instructor.FullName,
                course.Instructor.Email!, course.Instructor.AvatarUrl,
                course.Instructor.Role.ToString()),
            course.Enrollments.Count, course.Materials.Count,
            course.CreatedAt, course.UpdatedAt);
    }
}